angular.module('alphaGame').service('appStorage', function($q) {
	var self = this;
	
	this.storageName = 'alphaGame';
	this.data = {};
	this.previousData;
	
	this.loadData = function(callback) {
		var storageName = self.storageName;
		var localData;
		/* REMOVE? Storage as Chrome app in Chrome.local.storage
		chrome.storage.sync.get(storageName, function(keys) {
			console.log('Sync keys:', keys);
			if (keys.hasOwnProperty(storageName)) {
				if (keys[storageName].hasOwnProperty('data')) {
					self.data = keys[storageName].data;
					console.log('Found data, set to: ', self.data);
				} else {
					console.log('CANNOT FIND data!');
				}
			} else {
				console.log('STORAGE NOT FOUND WITH storageName: ', self.storageName);
			}
			
			if (typeof callback == 'function')
				callback(self.data);
		});*/
		if (localStorage.hasOwnProperty('alphaGame')) {
			//Load data		 
			localData = JSON.parse(localStorage[storageName]);
			self.data = localData;
			self.previousData = localData;
			console.log('storageName ' + storageName + ', localData: ', localData);
		} else {
			//Initialize new game data - TODO
			console.log('No localStorage data found - sending back to Ctrl for data initialization...');
		}
		
		if (typeof callback == "function")
			callback(self.data);
	}
	
	this.saveData = function() {
		var storageName = self.storageName;
		var storageData = JSON.stringify(self.data);
		localStorage[self.storageName] = storageData;
		console.log('AppStorage.saveData:: storageData =', storageData);
	}
	
	
});