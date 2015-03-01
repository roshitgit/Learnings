

http://sravi-kiran.blogspot.in/2013/03/MovingAjaxCallsToACustomServiceInAngularJS.html


/*
In my last post on Angular JS, we moved the data into an ASP.NET Web API controller and invoked the data using $http, 
a service in Angular JS that abstracts AJAX calls. But the code resided in the controller and that is not good. 
Responsibility of the controller should be to sit between data and view, but not to call services using AJAX. 
So, let’s move the AJAX calls to a separate component. 
The AJAX calls made using $http service are executed asynchronously. They return promise objects, using which their status 
can be tracked. As we will be moving them into a custom service, the controller depending on them should be notified with 
the status of completion. The notification can be sent using the promise object returned by the service functions or, 
we can create our own deferred object using $q service. 
I discussed about deferred and promise in jQuery earlier in a post. Basic usage of the pattern remains the same with 
$q as well, except a small difference syntax. Following snippet is shows the way of creating a deferred object, 
calling success and failure functions using $q: */

function myAsyncFunc(){
//Creating a deferred object
var deferred = $q.defer();
operation().success(function(){
    //Calling resolve of deferred object to execute the success callback
    deferred.resolve(data);
  }).error(function(){
    //Calling reject of deferred object to execute failure callback
    deferred.reject();
  });
  //Returning the corresponding promise object
  return deferred.promise;
}

/*While calling the above function, a callback to be called on success and a callback to be handled on failure should be 
attached. It might look as follows: */

myAsyncFunction().then(function(data){
    //Update UI using data or use the data to call another service
  },
  function(){
    //Display an error message
  });

/*The functions responsible for making AJAX calls should follow this pattern as the functions in $http are executed 
asynchronously. To notify the dependent logic about state of execution of the functionality, we have to use the 
promise pattern. 
As we will be enclosing this functionality in a custom service which requires $http for AJAX and $q for promise, 
these dependencies will be injected at the runtime when the module is loaded. Following snippet demonstrates the 
implementation of a function to retrieve items in shopping cart calling Web API service: */

angular.module('shopping', []).
  factory('shoppingData',function($http, $q){
    return{
      apiPath:'/api/shoppingCart/',
      getAllItems: function(){
        //Creating a deferred object
        var deferred = $q.defer();
 
        //Calling Web API to fetch shopping cart items
        $http.get(this.apiPath).success(function(data){
          //Passing data to deferred's resolve function on successful completion
          deferred.resolve(data);
      }).error(function(){
 
        //Sending a friendly error message in case of failure
        deferred.reject("An error occured while fetching items");
      });
 
      //Returning the promise object
      return deferred.promise;
    }
  }
}

function ShoppingCartCtrl($scope, shoppingData) {
  $scope.items = [];
 
  function refreshItems(){
    shoppingData.getAllItems().then(function(data){
      $scope.items = data;
    },
    function(errorMessage){
      $scope.error=errorMessage;
    });
  };
 
  refreshItems();
};

/*As you see parameters of the controller, the second parameter shoppingData will be injected by the dependency injector 
during runtime. The function refreshItems follows the same convention as the snippet we discussed earlier. It does the following: 
On successful completion of getAllItems() function, it assigns data to a property of $scope object which will be used to 
bind data
If the execution fails, it assigns the error message to another property of $scope object using which we can display 
the error message on the screen

The controller is free from any logic other than updating data that is displayed on the UI. */
