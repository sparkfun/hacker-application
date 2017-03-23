app.controller('MainController', function($scope, $route, $location, $cookies, mainService, editService) {

    $scope.view = {};
    $scope.view.search = '';

    let cookie = $cookies.getObject('loggedin');

    $scope.user = cookie;

    //populates main page with the meal information from database
    mainService.meals().then(function(returnedMeals) {
        $scope.view.mealsArray = returnedMeals.data
    });

    //updates the total number of pounds of food saved on main page when each meal is purchased
    editService.calculate().then(function(returned) {
        $scope.view.restArray = returned.data.restaurants
        $scope.pounds = returned.data.pounds
    });

    //when meal is purchased, updates meal item in database with user id of purchaser
    $scope.updateMeal = function(meal) {
        let cookie = $cookies.getObject('loggedin');
        let id = cookie.id;

        let updatedMeal = {
            id: meal.id,
            restaurant_id: meal.restaurant_id,
            name: meal.mealName,
            details: meal.details,
            dietary: meal.dietary,
            pickup: meal.pickup,
            price: meal.price,
            user_id: id
        };

        mainService.update(updatedMeal).then(function(returnedMeals) {})
    };

    //shopping cart info below
    $scope.totalCost = 0;
    $scope.totalTax = 0;
    $scope.shoppingCart = [];

    var taxRate = .18;

    //adds item to shopping cart upon user action
    $scope.addItem = function(meal) {
        $scope.shoppingCart.push(meal);
        updatePrice();
    };

    //updates price in shopping cart when a meal is added
    function updatePrice() {
        var sum = 0;
        var taxSum = 0;

        for (var i = 0; i < $scope.shoppingCart.length; i++) {
            sum += Number($scope.shoppingCart[i].price);
            taxSum += ($scope.shoppingCart[i].price * taxRate)
        }
        $scope.totalTax = taxSum;
        $scope.totalCost = sum + taxSum;
    };

    //logout
    $scope.logout = function() {
        $cookies.remove('loggedin');
        $route.reload();
    };

});
