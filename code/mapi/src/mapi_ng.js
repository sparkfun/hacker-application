var module, cs;
module = angular.module('mutableApi', []);

module.controller('mApiController', ['$scope', function($scope) {
	cs = $scope; //dbg

	$scope.debug = false; //Enable debug text in View
	
	$scope.labs = [];
	
	$scope.addLab = function(cfg) {
		if (!cfg) {
			cfg = { "name": "Lab " + ($scope.labs.length + 1), "selectedApi": false, "history":[],"lastCall": { "request": "", "response": "" } };
		}
		$scope.labs.push(cfg);
	}
	
	$scope.removeLab = function(lab) {
		$scope.labs.splice($scope.labs.indexOf(lab), 1);
	}
	
	$scope.makeApiCall = function(lab, method, methodCfg) {
		var url, params = {}, options = {}, data = {}, httpMethod = 'GET', paramString = '';
		console.log('$makeApiCall, method=' + method + ', lab/methodCfg:');
		console.log(lab);
		console.log(methodCfg);
		url = lab.selectedApi.cfg.baseUrl + (method ? method : '');
		params = lab.queryParameters;
		console.log('params:url='+url);
		console.log(params);
		if (lab.selectedApi.cfg.queryMethods.length <= 1) {
			httpMethod = lab.selectedApi.cfg.queryMethods[0];
		}
		//if (lab.selectedApi.cfg.queryParameters.length > 0) { //Higher-level params
			angular.forEach(lab.selectedApi.cfg.queryParameters, function(paramCfg, paramName) {
				console.log('paramName='+paramName);
				console.log(paramCfg);
				data[paramName] = paramCfg['default'] || null;
			});
		//} else {
			//console.log('no lab.queryParameters?:');
			//console.log(lab.queryParameters);
		//}
		if (methodCfg !== false) {
			if (methodCfg.hasOwnProperty("queryParameters") && methodCfg.queryParameters.length > 0) { //Method params
				console.log('considering methodCfg.queryParameters:');
				angular.forEach(methodCfg.queryParameters, function(paramCfg, paramName) {
					console.log('method paramName='+paramName);
					console.log(paramCfg);
					data[paramName] = lab.queryParameters[paramName].value;
				});
			}
			//} else {
				//console.log('no methodCfg.queryParameters?:');
				//console.log(methodCfg);
			//}
			angular.forEach(data, function(val, param) { //Construct final parameters for GET
				paramString += param + '=' + val + '&';
				console.log('final paramString='+paramString);
			});
		
			if (methodCfg.hasOwnProperty('responseHandler')) {
				options['responseHandler'] = methodCfg.responseHandler;
			}
		
		} else {
			if (lab.selectedApi.cfg.hasOwnProperty("callable") && lab.selectedApi.cfg.callable === true) {
				if (lab.selectedApi.cfg.hasOwnProperty("responseHandler")) {
					options['responseHandler'] = lab.selectedApi.cfg.responseHandler;
				}
			}
		}
		if (paramString.length > 0 && httpMethod === 'GET') {
			
			//var finalParamString = paramString.substring(0, -1);
			//console.log('finalParamString='+finalParamString);
			url += '?' + paramString;
			console.log('final url='+url);
		}
		
		console.log('pre-xhr, data:');
		console.log(data);
		options['data'] = data;
		options['method'] = httpMethod;
		options['apiMethod'] = method;
		options['paramString'] = paramString;
		//options['callback_args'] = [ lab, method, methodCfg ];
		options['lab'] = lab;
		console.log(options);
		makeHttpRequest(url, options, $scope.getApiCallResponse);
		
	}

	$scope.getApiCallResponse = function(responseText, xhr) {
		//console.log('responseText/xhr:');
		//console.log(responseText);
		console.log(xhr);
		if (xhr.hasOwnProperty("_options") && xhr._options.hasOwnProperty("responseHandler") && typeof xhr._options.responseHandler == "function") {
			console.log('returning response to _customCallback...');
			xhr._options.responseHandler(responseText, xhr);
		}
		$scope.parseAndLogResponse(responseText, xhr);
	}
	
	$scope.parseAndLogResponse = function(responseText, xhr) {
		var logXhr = new XMLHttpRequest();
		var url = new URL('http://localhost/pp/mapi/log_api_call.php');
		var responseURL = xhr.responseURL;
		var data = { "url": "", "url_long": "", "method": "", "parameters": "", "response_text": "", "status": "", "status_text": "" };
		var formData = new FormData();
		//console.log('$parseAndLogResponse, responseText/xhr:');
		//console.log(responseText);
		console.log(xhr);
		logXhr.addEventListener('load', $scope.successfulLogAndParse);
		logXhr.addEventListener('error', $scope.failedLogAndParse);
		logXhr.addEventListener('abort', httpRequestCanceled);
		
		
		//if (responseURL) {
			if (responseURL.length <= 255) {
				data['url'] = responseURL;
			} else {
				data['url_long'] = responseURL;
			}
		//}
		data['status'] = xhr.status;
		data['status_text'] = xhr.statusText;
		if (xhr.hasOwnProperty("_options")) {
			data['method'] = xhr._options.apiMethod;
			data['parameters'] = JSON.stringify(xhr._options.data);
			data['response_text'] = responseText;
		}
		//url.searchParams.set('data', data);
		formData.append("data", JSON.stringify(data));
		console.log('final FormData:');
		console.log(formData);
		logXhr.open("POST", url);
		logXhr.send(formData);
		
	}
	
	$scope.successfulLogAndParse = function() {
		console.log('$successfulLogAndParse ... this:');
		console.log(this);
		
		$scope.$apply();
	}
	
	$scope.failedLogAndParse = function() {
		console.log('$failedLogAndParse?! this:');
		console.log(this);
		alert('$failedLogAndParse!');
		
		$scope.$apply();
	}
	
	$scope.fetchWebPage = function(lab, method, methodCfg) {
		var url, params = {}, options = {}, data = {}, httpMethod = 'GET', paramString = '';
		var xhr;
		var hardCodedEleSelector = '#dictionary > dl';
		console.log('$fetchWebPage, method=' + method + ', lab/methodCfg:');
		console.log(lab);
		console.log(methodCfg);
		url = lab.selectedApi.cfg.baseUrl + (method ? method : '');
		params = lab.queryParameters;
		console.log('params:url='+url);
		console.log(params);
		
		angular.forEach(lab.selectedApi.cfg.queryParameters, function(paramCfg, paramName) {
			console.log('paramName='+paramName);
			console.log(paramCfg);
			data[paramName] = paramCfg['default'] || null;
			paramString += '&' + paramName + '=' + lab.queryParameters[paramName].value || paramCfg['default'];
		});
		
		if (paramString.length > 0) {
			paramString = paramString.substring(1);
		}
		
		if (lab.selectedApi.cfg.hasOwnProperty('responseHandler') && typeof lab.selectedApi.cfg.responseHandler == "function") {
			console.log('appending responseHandler to fetchWebpage');
			data['responseHandler'] = lab.selectedApi.cfg.responseHandler;
		}
		
		data['url'] = url + (paramString.length > 0 ? '?' + paramString : '');
		data['paramString'] = paramString;
		data['eleSelector'] = hardCodedEleSelector; //FIXME
		
		console.log(data);
		console.log('PARAMSTRING='+paramString);
		
		switch(httpMethod) {
			case 'GET':
				/*$.get('fetch_html.php', { url: url }, function(data, xhr) {
					console.log('GET fetch_html success!');
					console.log(data);
					console.log(xhr);
					$scope.parseAndLogResponse(data, xhr);
				});*/
					
				//makeHttpRequest('http://localhost/pp/mapi/fetch_html.php', { url: url, lab: lab, method: httpMethod, apiMethod: method, data: data }, $scope.getApiCallResponse);
				xhr = makeHttpRequest('http://localhost/pp/mapi/fetch_html.php', { url: url, lab: lab, method: 'POST', apiMethod: method, data: data, paramString: paramString, eleSelector: hardCodedEleSelector }, $scope.getApiCallResponse);
				break;
			case 'POST':
				/*$.post('fetch_html.php', { url: url, params: params}, function(data, xhr) {
					console.log('POST fetch_html success!');
					console.log(data);
					console.log(xhr);
					$scope.parseAndLogResponse(data, xhr);
				});*/
				xhr = makeHttpRequest('fetch_html.php', { url: url, lab: lab, method: httpMethod, apiMethod: method, data: data }, $scope.getApiCallResponse);
				break;
			default:
				console.log('ERROR: Empty httpMethod?');
		}
	}
	
	$scope.loadData = function(data, ref) {
		var obj = JSON.parse(data);
		console.log('$loadData:');
		console.log(data);
		console.log(ref);
		$scope.apis = [];
		
		angular.forEach(obj, function(v, k) {
			$scope.apis.push({"name": k, "cfg": v});
		});
		$scope.$apply();
	}
	
	$scope.init = function() {
		$scope.addLab();
		$scope.addLab();
		makeHttpRequest('http://localhost/pp/mapi/fetch_api.php?api=_all', {}, $scope.loadData);
	}
	
	$scope.init(); //Launch app, fetch APIs
}]);





function makeHttpRequest(url, options, callback) {
	var finalUrl, xhr, method = options.hasOwnProperty("method") ? options['method'] : 'GET', formData = new FormData();
	console.log('makeHttpRequest url/options/callback:'+url);
	console.log(options);
	console.log(callback);
	if (url.length > 0) {
			console.log(url);
			finalUrl = new URL(url);
	}
	xhr = new XMLHttpRequest();
	xhr.addEventListener('load', httpRequestDataLoaded);
	xhr.addEventListener('error', httpRequestFailed);
	xhr.addEventListener('progress', httpRequestUpdateProgress);
	xhr.addEventListener('abort', httpRequestCanceled);
	if (options.hasOwnProperty("_api")) {
		xhr['_api'] = options._api;
	}
	if (options.hasOwnProperty("data")) {
		angular.forEach(options.data, function(val, key) {
			finalUrl.searchParams.set(key, val);
			formData.append(key, val);
		});
		
		console.log(finalUrl);
	}
	xhr['_options'] = options;
	if (callback && typeof callback == "function") {
		//callback(xhr.responseText, xhr);
		xhr['_customCallback'] = callback;
	}
	if (typeof method === "string" && method.toLowerCase() === 'get') {
		xhr.open('GET', url);
	} else if (method && method.toLowerCase() === 'post') {
		//var boundary = '-------------------------------' + Date.now().toString(16);
		//xhr.setRequestHeader("Content-Type", "multipart\/form-data; boundary=" + boundary);
				
		xhr.open('POST', url);
	}
	console.log('final formData:');
	console.log(formData);
	//xhr.open(method, url);
	xhr.send(formData);
	return xhr;
}

function httpRequestDataLoaded() {
	var segments = this.responseURL.split('/');
	var segment = segments[segments.length - 2];
	console.log(segments);
	console.log(segment);
	console.log('httpRequestDataLoaded::');
	console.log(this);
	console.log('responseText length:'+this.responseText.length);
	if (this.hasOwnProperty("_options") && this._options.hasOwnProperty("lab")) {
		var history = { "responseURL": this.responseURL, "responseText": this.responseText };
		this._options.lab.history.push(history);
		this._options.lab.lastCall.response = this.responseText;
		//this._options.lab.responseObj = JSON.parse(this.responseText);
	}
	if (this.hasOwnProperty('_customCallback')) {
		console.log('customCallback...');
		console.log(this._customCallback);
		this._customCallback(this.responseText, this);
	} else {
		return this.responseText;
	}
	return this;
	
}

function httpRequestFailed(evt) {
	console.log('An error occurred with the request:');
	console.log(evt);
}

function httpRequestCanceled(evt) {
	console.log('Cancelled request:');
	console.log(evt);
}

function httpRequestUpdateProgress(evt) {
	var percentComplete = 'unknown';
	if (evt.lengthComputable) {
		var percentComplete = evt.loaded / evt.total;
	}
	return percentComplete;
}


//SWAPI Pure JS Scrape test
var swapiSchemaEndpoints = [
	'http://swapi.co/api/films/schema',
	'http://swapi.co/api/people/schema',
	'http://swapi.co/api/planets/schema',
	'http://swapi.co/api/species/schema',
	'http://swapi.co/api/starships/schema',
	'http://swapi.co/api/vehicles/schema'
];

var starWarsAPISchema = {
	"films":{},
	"people":{},
	"planets":{},
	"species":{},
	"starships":{},
	"vehicles":{}
	
};

function scrapeStarWarsAPI(endpoints, schema) {
	var options = { "method": "GET", "api": "swapi" };
	var timeout = 0;
	var timeoutInc = 5000; //Milliseconds to wait before firing next request
	for (var i = 0;i < endpoints.length;i++) {
		var endpoint = endpoints[i];
		var xhr = makeHttpRequest(endpoint, options);
		var timeoutId = window.setTimeout(makeHttpRequest, timeout, endpoint, options);
		var segment = endpoint.split('/')[-2];
		timeout += timeoutInc;
		console.log('scrapeStarWarsAPI:: Made request ' + (i+1) + ' of ' + endpoints.length);
		
	}
}

function processSchema(schema, options) {
	//TODO Process Schema from a structure-identifying call
	var out = {};
	
}

function parseSchemaLine(line) {
	
}
