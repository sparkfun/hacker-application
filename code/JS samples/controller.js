angular.module('MainApp')
.controller('ProjectController', function($scope){
	$scope.message = "Hello Projects!";
})
.controller('HomeController', function($scope){
	$scope.message = "Hello Home!";
})
.controller('EmailController', function($scope,$http) {
        $scope.resultMessage;
        $scope.fullname="";
        $scope.email="";
        $scope.message=""

        $scope.sendEmail =function(){
            console.log("about to email - before mailJSON");
            var mailJSON ={
                "key": "rgXsCj0PROJbEHOk_7uHrA",
                "message": {
                  "html": ""+$scope.message,
                  "text": ""+$scope.message,
                  "subject": "*** Web Dev message from " + $scope.fullname + " ***",
                  "from_email": $scope.email,
                  "from_name": "" + $scope.fullname,
                  "to": [
                    {
                      "email": "ericapeharda@gmail.com",
                      "name": "EP Web Dev",
                      "type": "to"
                    }
                  ],
                  "important": false,
                  "track_opens": null,
                  "track_clicks": null,
                  "auto_text": null,
                  "auto_html": null,
                  "inline_css": null,
                  "url_strip_qs": null,
                  "preserve_recipients": null,
                  "view_content_link": null,
                  "tracking_domain": null,
                  "signing_domain": null,
                  "return_path_domain": null
                },
                "async": false,
                "ip_pool": "Main Pool"
            };
            var apiURL = "https://mandrillapp.com/api/1.0/messages/send.json";
            $http.post(apiURL, mailJSON).
              success(function(data, status, headers, config) {                
                $scope.form={};
                console.log('successful email send.');
                console.log('status: ' + status);
                console.log('data: ' + data);
                console.log('headers: ' + headers);
                console.log('config: ' + config);
                $scope.myForm.$setUntouched();
                $scope.resultMessage="Thank you!  You message was successfully sent";
              }).error(function(data, status, headers, config) {
                console.log('error sending email.');
                console.log('status: ' + status);
                $scope.resultMessage="Your message failed to send.  Please email ericapeharda@gmail.com";
              });
        };
})
.controller('AboutController', function($scope){
	$scope.message = "Hello About!"
});
