/*  NOTE (important info):
    If ! is omitted from domready below, then it's just a normal module that happens to be a function, 
    which can take a callback that won't execute before the DOM is safe to interact with.
    so better to put ! to ensure dom is ready. also domReady! enjoys that same advantage over $(document).ready() 
            
    ng-app directive should not be used when manually bootstrapping the app.
    "angular.bootstrap" is an alternative approach to initializing application without using ng-app.
    better to bootstrap like this, as ng-app intialization way fails < IE 8.
    Also it fails if data is loaded asynchronously within the app.

    angular syntax:             angular.bootstrap(element, [modules], [config]);
    better to use if scripts are registered at the end of the page (instead of in the header).
    Otherwise, the DOM will not be loaded at the time of bootrsaping the app.
*/

(function () {
    /*
    * Application startup. Angular is bootstrapped once the DOM
    * has been loaded by the browser 
    */
    var dependencies = [
              //'require',  //not required here as already loaded
              'angular',
              './config'    //load all routes and dependencies before bootstrapping
    ];
    define(dependencies, function (angular, Config) {
        'use strict';

        //angular.element(document).ready(function () {         //angular way. in it "element" is dom element which is root of angular app.
        require(['domReady!'], function (document) {            //using requirejs domready plugin
            angular.bootstrap(document, ['DashboardModule']);   //manually bootstrap to compile the element into an executable, bi-directionally bound application.


            //count total number of watchers on page
            (function () {

                var root = angular.element(document.getElementsByTagName('body'));

                var countWatchers_ = function (element, scopes, count) {
                    var scope;
                    scope = element.data().$scope;
                    if (scope && !(scope.$id in scopes)) {
                        scopes[scope.$id] = true;
                        if (scope.$$watchers) {
                            count += scope.$$watchers.length;
                        }
                    }
                    scope = element.data().$isolateScope;
                    if (scope && !(scope.$id in scopes)) {
                        scopes[scope.$id] = true;
                        if (scope.$$watchers) {
                            count += scope.$$watchers.length;
                        }
                    }
                    angular.forEach(element.children(), function (child) {
                        count = countWatchers_(angular.element(child), scopes, count);
                    });
                    return count;
                };

                window.countWatchers = function () {
                    return countWatchers_(root, {}, 0);
                };

            })();


            setInterval(function () { console.log(countWatchers()) }, 5000);
        });
    });
})();

