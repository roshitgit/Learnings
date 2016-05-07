//var app = angular.module('app', ['ngAnimate', 'ngTouch', 'ui.grid']);
var app = angular.module('app', ['ui.grid']);

app.controller('MainCtrl', ['$scope', '$http', 'uiGridConstants', function ($scope, $http, uiGridConstants) {
    $scope.gridOptions = {
        enableFiltering: true,
        flatEntityAccess: true,
        showGridFooter: true,
        fastWatch: true
    };

    $scope.gridOptions.columnDefs = [
     { name: 'id' },
     { name: 'name' },
     { name: 'gender' },
     { field: 'age' }
    ];

    $http.get('/api/Data/GetRecordsForUiGrid')
    .success(function (data) {
      
        $scope.gridOptions.data = data;
    });

    $scope.toggleFlat = function () {
        $scope.gridOptions.flatEntityAccess = !$scope.gridOptions.flatEntityAccess;
    }
}]);
