////////////////////////////////////////////////////////////
// MembersForm Controller
//
// The membersForm controller employs a number of services to
// handle the various form submissions made to a members' collections
// Note the passing of methods in the submit form method
////////////////////////////////////////////////////////////

angular.module('wctApp')
.controller('MembersFormsController',
  ['$scope', '$stateParams', '$http',
    'ContextService',
    '$state',
    'Config',
    'Show', 'ShowDate', 'ShowTicket',
    'Product', 'ProductSku',
    
    function ($scope, $stateParams, $http,
              ContextService,
              $state,
              Config,
              Show, ShowDate, ShowTicket,
              Product, ProductSku) {
      
      $scope.view = {};
      $scope.view.ContextService = ContextService;
      var _successMsg = '';
      $scope.view.getSuccessMessage = function(){
        return _successMsg;
      };
      $scope.view.setSuccessMessage = function(msg){
        return _successMsg = msg;
      };
      
      $scope.cancelForm = function(form){
        cleanupFormAndReturn(form);
      };
      
      $scope.resetForm = function(form){
        $state.reload();
      };
      
      // reset everything!
      var cleanupFormAndReturn = function(form){
        $scope.view.ContextService.currentConfig = null;
        $scope.view.ContextService.currentShow = null;
        $scope.view.ContextService.currentShowDate = null;
        $scope.view.ContextService.currentShowTicket = null;
        $scope.view.ContextService.currentProduct = null;
        $scope.view.ContextService.currentProductSku = null;
        
        // clean up the form and return
        form.entity = {};
        form.$setPristine();
        form.$setUntouched();
        
        redirectToPreviousState();
      };
      
      // establish the redirect path
      var redirectToPreviousState = function(){
        var currentState = $state.current.name;
        var goto = 'members.shows';
        
        if(currentState.indexOf('members.products') !== -1) {
          goto = 'members.products';
        } else if (currentState.indexOf('members.configs') !== -1) {
          goto = 'members.configs';
        }
        $state.go(goto);
      };
      
      // Keep an eye on datetime-picker as it may always report due
      //  to formatting changes from model date to date format
      // Also be aware that boolean values will report as well,
      //  due to the form using strings vs boolean
      $scope.submitForm = function(form, context) {
        $scope.view.errors = null;
        
        if(form.$valid && form.entity) {
          var _entity = angular.copy(form.entity);
          var formSubmit = null;
          var listToRefresh = null;
          var refreshMethod = null;
          var setCurrent = null;
          
          // select form submission by model
          if(context === 'config') {
            _entity.member_id = $scope.view.ContextService.currentMember.id;
            formSubmit = Config.processForm(form, _entity,
              $scope.view.ContextService.currentConfig);
            listToRefresh = $scope.$parent.view.configList;
            refreshMethod = $scope.$parent.populateConfig;
            setCurrent = $scope.view.ContextService.setCurrentConfig;
          } else if(context === 'show') {
            _entity.member_id = $scope.view.ContextService.currentMember.id;
            formSubmit = Show.processForm(form, _entity,
              $scope.view.ContextService.currentShow);
            listToRefresh = $scope.$parent.view.showList;
            refreshMethod = $scope.$parent.populateShowList;
            setCurrent = $scope.view.ContextService.setCurrentShow;
          } else if (context === 'showdate') {
            formSubmit = ShowDate.processForm(form, _entity,
              $scope.view.ContextService.currentShowDate,
              $scope.view.ContextService.currentShow);
            listToRefresh = $scope.$parent.view.showList;
            refreshMethod = $scope.$parent.populateShowList;
            setCurrent = $scope.view.ContextService.setCurrentShowDate;
          }  else if (context === 'showticket') {
            formSubmit = ShowTicket.processForm(form, _entity,
              $scope.view.ContextService.currentShowTicket,
              $scope.view.ContextService.currentShowDate);
            listToRefresh = $scope.$parent.view.showList;
            refreshMethod = $scope.$parent.populateShowList;
            setCurrent = $scope.view.ContextService.setCurrentShowTicket;
          } else if (context === 'product') {
            _entity.member_id = $scope.view.ContextService.currentMember.id;
            formSubmit = Product.processForm(form, _entity,
              $scope.view.ContextService.currentProduct);
            listToRefresh = $scope.$parent.view.productList;
            refreshMethod = $scope.$parent.populateProductList;
            setCurrent = $scope.view.ContextService.setCurrentProduct;
          } else if (context === 'productsku') {
            formSubmit = ProductSku.processForm(form, _entity,
              $scope.view.ContextService.currentProductSku,
              $scope.view.ContextService.currentProduct);
            listToRefresh = $scope.$parent.view.productList;
            refreshMethod = $scope.$parent.populateProductList;
            setCurrent = $scope.view.ContextService.setCurrentProductSku;
          }
          
          return formSubmit
          .then(function(data){
            
            var idx = (Array.isArray(data)) ? data[0] : data;
            
            listToRefresh = null;
            refreshMethod();
            
            // enable a delay for success msg to show  and then hide
            $scope.view.setSuccessMessage('changes saved.');
            setTimeout(function(){
              $scope.$apply(function(){
                $scope.view.setSuccessMessage('');
              });
            }, 2500);
            
            // set the current object based on context
            if(setCurrent){
              setCurrent(idx).then(function(data){
                if(context === 'config') {
                  $scope.view.ContextService.currentConfig = data;
                } else if(context === 'product') {
                  $scope.view.ContextService.currentProduct = data;
                } else if(context === 'productsku') {
                  $scope.view.ContextService.currentProductSku = data;
                } else if(context === 'show') {
                  $scope.view.ContextService.currentShow = data;
                } else if(context === 'showdate') {
                  $scope.view.ContextService.currentShowDate = data;
                } else if(context === 'showticket') {
                  $scope.view.ContextService.currentShowTicket = data;
                }
              });
            }
          })
          .catch(function(err){
            $scope.view.errors = err;
          });
        }
      };
      
    }]);
