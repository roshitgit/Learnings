http://www.peterbe.com/plog/promises-with-$http

/*By writing this I'm taking a risk of looking like an idiot who has failed to read the docs. So please be gentle.
AngularJS uses a promise module called $q. It originates from this beast of a project.
You use it like this for example:*/

angular.module('myapp')
.controller('MainCtrl', function($scope, $q) {
  $scope.name = 'Hello ';
  var wait = function() {
    var deferred = $q.defer();
    setTimeout(function() {
      // Reject 3 out of 10 times to simulate 
      // some business logic.
      if (Math.random() > 0.7) deferred.reject('hell');
      else deferred.resolve('world');
    }, 1000);
    return deferred.promise;
  };

  wait()
  .then(function(rest) {
    $scope.name += rest;
  })
  .catch(function(fallback) {
    $scope.name += fallback.toUpperCase() + '!!';
  });
});
/*
Basically you construct a deferred object and return its promise. Then you can expect the .then and .catch to be called back if all goes well (or not).
There are other ways you can use it too but let's stick to the basics to drive home this point to come.
Then there's the $http module. It's where you do all your AJAX stuff and it's really powerful. However, 
it uses an abstraction of $q and because it is an abstraction it renames what it calls back. Instead of .then and 
.catch it's .success and .error and the arguments you get are different. Both expose a catch-all function called .finally. 
You can, if you want to, bypass this abstraction and do what the abstraction does yourself. So instead of:*/

$http.get('https://api.github.com/users/peterbe/gists')
.success(function(data) {
  $scope.gists = data;
})
.error(function(data, status) {
  console.error('Repos error', status, data);
})
.finally(function() {
  console.log("finally finished repos");
});
...you can do this yourself...:

$http.get('https://api.github.com/users/peterbe/gists')
.then(function(response) {
  $scope.gists = response.data;
})
.catch(function(response) {
  console.error('Gists error', response.status, response.data);
})
.finally(function() {
  console.log("finally finished gists");
});

/*It's like it's built specifically for doing HTTP stuff. The $q modules doesn't know that the response body, the HTTP status 
code and the HTTP headers are important.
However, there's a big caveat. You might not always know you're doing AJAX stuff. 
You might be using a service from somewhere and you don't care how it gets its data. You just want it to deliver some data. 
For example, suppose you have an AJAX request cached so that only the first time it needs to do an HTTP GET but all 
consecutive times you can use the stuff already in memory. E.g. Something like this:*/

angular.module('myapp')
.controller('MainCtrl', function($scope, $q, $http, $timeout) {

  $scope.name = 'Hello ';
  var getName = function() {
    var name = null;
    var deferred = $q.defer();
    if (name !== null) deferred.resolve(name);
    $http.get('https://api.github.com/users/peterbe')
    .success(function(data) {
      deferred.resolve(data.name);
    }).error(deferred.reject);
    return deferred.promise;
  };

  // Even though we're calling this 3 different times
  // you'll notice it only starts one AJAX request.
  $timeout(function() {
    getName().then(function(name) {
      $scope.name = "Hello " + name;
    });    
  }, 1000);

  $timeout(function() {
    getName().then(function(name) {
      $scope.name = "Hello " + name;
    });    
  }, 2000);

  $timeout(function() {
    getName().then(function(name) {
      $scope.name = "Hello " + name;
    });    
  }, 3000);
});

/*And with all the other promise frameworks laying around like jQuery's you will sooner or later forget if it's success() or 
then() or done() and your goldfish memory (like mine) will cause confusion and bugs.
So is there a way to make $http.<somemethod> return a $q like promise but with the benefit of the abstractions that the 
$http layer adds?
Here's one such possible solution maybe:*/

var app = angular.module('myapp');

app.factory('httpq', function($http, $q) {
  return {
    get: function() {
      var deferred = $q.defer();
      $http.get.apply(null, arguments)
      .success(deferred.resolve)
      .error(deferred.resolve);
      return deferred.promise;
    }
  }
});

app.controller('MainCtrl', function($scope, httpq) {

  httpq.get('https://api.github.com/users/peterbe/gists')
  .then(function(data) {
    $scope.gists = data;
  })
  .catch(function(data, status) {
    console.error('Gists error', response.status, response.data);
  })
  .finally(function() {
    console.log("finally finished gists");
  });
});
