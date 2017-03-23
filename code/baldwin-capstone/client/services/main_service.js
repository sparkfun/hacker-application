app.service('mainService', function($http) {
    return {
        meals: function() {
            return $http.get('./index');
        },
        getId: function(id) {
            return $http.get(`/index/${id}`);
        },
        update: function(meal) {
            return $http.put('./index', meal);
        },
        new: function(meal) {
            return $http.post('./index', meal);
        },
        pounds: function(restaurant) {
            const id = restaurant.id;
            return $http.put(`/index/${id}`, restaurant);
        }
    }
});
