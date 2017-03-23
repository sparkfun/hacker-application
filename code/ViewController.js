// This is Angular code that controls the view.html document that is attached.

app.controller("ViewController", ['$http', '$state', '$rootScope', '$window',function($http, $state, $rootScope, $window) {
  var vm = this;

  // There must be a deck stored in $rootScope, or else the user will be rerouted to the previous page.

  if (!$rootScope.deck) {
    $state.go("public");
  }

  vm.stats = false;
  if (!$rootScope.deck.comments) {
    $rootScope.deck.comments = [];
  }

  // The upvote and downvote functions allow users to vote on the quality of any given deck in a style similar to Reddit.

  vm.upvote = function() {
    if ($rootScope.profile) {
      if (!$rootScope.deck.upvoters.includes($rootScope.profile.username)) {
        $rootScope.deck.votes++;
        $rootScope.deck.upvoters.push($rootScope.profile.username);
        if ($rootScope.deck.downvoters.includes($rootScope.profile.username)) {
          $rootScope.deck.votes++;
          $rootScope.deck.downvoters.splice($rootScope.deck.downvoters.indexOf($rootScope.profile.username), 1);
        }
      }
      vm.update();
    }
    else {
      $state.go("login");
    }
  }

  vm.downvote = function(index) {
    if ($rootScope.profile) {
      if (!$rootScope.deck.downvoters.includes($rootScope.profile.username)) {
        $rootScope.deck.votes--;
        $rootScope.deck.downvoters.push($rootScope.profile.username);
        if ($rootScope.deck.upvoters.includes($rootScope.profile.username)) {
          $rootScope.deck.votes--;
          $rootScope.deck.upvoters.splice($rootScope.deck.upvoters.indexOf($rootScope.profile.username), 1);
        }
      }
      vm.update();
    }
    else {
      $state.go("login");
    }
  }

  // The update function makes an HTTP request to the backend and updates the permanent information.

  vm.update = function() {
    $http.put("https://mastermage.herokuapp.com/deck", $rootScope.deck)
    .then(function(data) {
      console.log($rootScope.deck);
    })
    .catch(function(err) {
      console.log(err);
    });
  };

  // The showCard and hideCard functions allow the user to view the image of a card by putting the mouse over the card's name.

  vm.showCard = function(index) {
    $("#cardpic").attr("src", $rootScope.deck.deck.included[index].editions[0].image_url).css("display", "inline");
  }

  vm.hideCard = function() {
    $("#cardpic").css("display", "none");
  };

  // The smallCard function allows for responsiveness.  This function only works on screens where the width of the screen is less than the height.

  vm.smallCard = function(index) {
    if ($(window).width() < $(window).height()) {
      $("#responsive-cardpic").attr("src", $rootScope.deck.deck.included[index].editions[0].image_url).css("display", "inline");
    }
  }

  // The comment function will either allow a user to comment if logged in, or prompt the user to log in by rerouting them to the login page.

  vm.comment = function() {
    if (!$rootScope.profile) {
      $state.go("login");
    }
    else {
      vm.commentForm = true;
    }
  };

  // The addComment function takes the imput from the comment form, updates the deck data
  // (the comments are part of the deck data), and calls the update function to update the backend.

  vm.addComment = function() {
    console.log($rootScope.profile.username);
    $rootScope.deck.comments.push({username: $rootScope.profile.username, content: vm.newComment, cards: []});
    vm.commentForm = false;
    vm.newComment = "";
    vm.update();
  };

  // If the user started to comment then changed their mind, this function handles that.

  vm.cancel = function() {
    vm.commentForm = false;
    vm.newComment = "";
  };

  // The edit function allows the user to edit their own comments.

  vm.edit = function(index) {
    vm.editor = null;
    vm.update();
  };

  // The delete function allows the user to delete their own comments.

  vm.delete = function(index) {
    $rootScope.deck.comments.splice(index, 1);
    vm.editor = null;
    vm.update();
  }

}]);
