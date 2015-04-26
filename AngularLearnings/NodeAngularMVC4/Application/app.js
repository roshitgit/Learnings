'use strict';
 
// this will load all the required dependencies for angular.js
 
angular.module('NodejsdemoApp', [
 
'ngRoute',
 
'ui.bootstrap'
 
])
 
.config(function ($routeProvider, $locationProvider) {
 
$routeProvider
 
//-- Public routes --//
 
.when('/home', {
 
templateUrl: 'partials/home.html',
 
controller: 'HomeCtrl'
 
})
 
$locationProvider.html5Mode(true);
 
})
 
.run(function ($rootScope, $location) {
 
$location.path('/home');&nbsp;
 
});
