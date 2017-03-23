app.service('mainService', function($http) {
  return {
    meals: function() {
      return $http.get('./index')
    },
    update: function(meal) {
      return $http.put('./index', meal)
    },
    pounds: function(restaurant) {
      const id = restaurant.id
      return $http.put(`/index/${id}`, restaurant)
    }
  }
})
