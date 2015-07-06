function () {

    var dependencies = [
          './grid-module',
          '../common/common',
          '../common/gridhelper',
          'bindonce'
    ];

    define(dependencies, function (GridModule, common, gridhelper, bo) {
        'use strict';

        GridModule.controller('GridController', GridViewModel);

        //make script "minsafe" using "$inject".
        GridViewModel.$inject = ['$scope', '$filter', 'GridService', 'ngTableParams',
                                '$rootScope', 'UserInfo', '$timeout', '$modal'];

        function GridViewModel($scope, $filter, GridService,
                                ngTableParams, $rootScope, UserInfo, $timeout, $modal) {

            var tableParamsInit = function () {
                $scope.loadingGrid = true;
                $scope.filteredData = [];
                $scope.finalGridData = [];
                $scope.tableParams = new ngTableParams({}, {});
            }
            ......
            ......
            ......
