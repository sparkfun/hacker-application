// DIRECTIVES

angular.module('wctApp')

////////////////////////////////////////////////////////////
// date-now - attribute
//
// I needed a solution to showing the current year for the copyright, but did not want to create a scoped object
// http://stackoverflow.com/questions/22962468/angularjs-display-current-date
// Apply the given date filter to 'new Date()'
// usage: <p>© <span date-now="yyyy"></span> WillCallTickets, Inc. All Rights Reserved.</p>
////////////////////////////////////////////////////////////

.directive('dateNow', ['$filter', function($filter) {
  return {
    restrict: 'A',
    link: function( $scope, $element, $attrs) {
      $element.text($filter('date')(new Date(), $attrs.dateNow));
    }
  };
}])


////////////////////////////////////////////////////////////
// toggle - attribute
//
// allows us to add a tooltip attrib to an anchor element - fancy angular version of title attrib
////////////////////////////////////////////////////////////

.directive('toggle', function(){
  return {
    restrict: 'A',
    link: function (scope, element, attrs) {
      if (attrs.toggle == "tooltip") {
        $(element).tooltip();
      }
    }
  }
})


////////////////////////////////////////////////////////////
// wct-brochure-display - element
// Displays a panel with brochure information - see home page
//
// usage: <wct-brochure-display item="item" ng-repeat="item in view.BrochureService.listing"></wct-brochure-display>
////////////////////////////////////////////////////////////

.directive('wctBrochureDisplay', ['BrochureService', function(BrochureService) {
  return {
    restrict: 'E',
    scope: {
      item: '<'
    },
    templateUrl: 'partials/brochurePanel.html',
    controller: ['$scope', function ($scope) {
      $scope.view = {};
      $scope.view.BrochureService = BrochureService;
    }]
  }
}])


////////////////////////////////////////////////////////////
// wct-add-brochure - element
// Displays a panel to add a new brochure - see admin/brochures
//
// The controller gives us the hooks to the brochure service
// usage: <wct-add-brochure></wct-add-brochure>
////////////////////////////////////////////////////////////

.directive('wctAddBrochure', ['BrochureService', function(BrochureService){
  return {
    restrict: 'E',
    scope: {
    },
    templateUrl: 'partials/admin/brochures/add.html',
    controller: function($scope) {
      $scope.view = {};
      $scope.view.errors = [];
      
      $scope.cancel = function(){
        $scope.view.errors = [];
        BrochureService.setAddMode(false);
      };
      
      $scope.submitBrochure = function(form){
        $scope.view.errors = [];
        if(form.brochure){
          var _brochure = angular.copy(form.brochure);
          BrochureService.addBrochure(_brochure)
          .then(function(data){
            
            form.brochure = {
              title: '',
              abstract: '',
              description: ''
            };
            
            form.$setPristine();
            form.$setUntouched();
            BrochureService.setAddMode(false);
          })
          .catch(function(err){
            $scope.view.errors.push(err);
          });
        }
      }
    }
  }
}]);