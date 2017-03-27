////////////////////////////////////////////////////////////
// Store Controller
//
// The store controller employs the Context, Store and Cart
// services and initializes the stripe_publish_key, and show
// and product catalogs
////////////////////////////////////////////////////////////

angular.module('wctApp')
.controller('StoreController',
  [ '$scope', '$location', '$window', '$stateParams',
    'ContextService', 'StoreService', 'CartService',
    '$http', '$state',
    
    function ($scope, $location, $window, $stateParams,
              ContextService, StoreService, CartService, $http, $state) {
      
      // console.log('STORE CONTROLLER', $stateParams);
      
      $scope.view = {};
      $scope.view.ContextService = ContextService;
      $scope.view.StoreService = StoreService;
      $scope.view.CartService = CartService;
      
      // TODO replace implementation to be more secure
      // this makes sense for development and keeping the authority of values in one place
      $scope.view.stripe_publish_key = '';
      $scope.view.ContextService.STRIPE_PUBLISH_KEY()
      .then(function (data) {
        $scope.view.stripe_publish_key = data.data;
      });
      
      // Navigation helper
      $scope.isRouteActive = function(route) {
        return $location.path().indexOf(route) !== -1;
      };
      
      // populate show listing
      $scope.view.showCatalog = null;
      $scope.populateShowCatalog = function(){
        $scope.view.StoreService.getStoreShowCatalog()
        .then(function(data){
          $scope.view.showCatalog = data;
        })
      };
      
      // populate product listing
      $scope.view.productCatalog = null;
      $scope.populateProductCatalog = function(){
        $scope.view.StoreService.getStoreProductCatalog()
        .then(function(data){
          $scope.view.productCatalog = data;
        })
      };
      
      // initialize
      $scope.populateShowCatalog();
      $scope.populateProductCatalog();
      
    }]);
