
Link: http://stackoverflow.com/questions/17913956/abstracting-http-calls-into-service

app.factory('myService', function($http, $q) {
 return {
   getFoo: function() {
     return $http.get('foo.json').then(function(result) {
       return result.data;
     }, function(result){
      //I'm doing something here to handle the error
      return $q.reject(result);
     });
   }
 }
});

app.controller('MainCtrl', function($scope, myService) {
  //the clean and simple way
  $scope.foo = myService.getFoo().then(function(){
    //Do something with successful response
  }, function(){
    //Do something with unsuccessful response
  });
}
