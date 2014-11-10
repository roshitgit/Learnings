var controllers = {};
controllers.searchall = function ($scope, $http) {
    $scope.getall = function () {
        $http.get('api/contacts').success(function (result) {
            $scope.contactall = result;
        }).error(function (data) {
            alert(data);
        });
    }
};
controllers.searchone = function ($scope, $http) {
    $scope.getone = function () {
        $http.get('api/contacts/' + $scope.cid).success(
        function (result) {
            $scope.ContactId = result.ContactId;
            $scope.Name = result.Name;
            $scope.Address = result.Address;
            $scope.City = result.City;
        }).error(function (data) {
            alert(data);
        });
    }
};
controllers.addone = function ($scope, $http) {
    $scope.addone = function () {
        var ddata =new data();
        $http.post(
           '/api/contacts',
           JSON.stringify(ddata),
           {
               headers:
               {
                   'Content-Type': 'application/json'
               }
           }
       ).success(function (data) {
           alert("insert successfully");
       }).error(function () {
           alert("insert abort!");
       });
    }
};
controllers.updateone = function ($scope, $http) {
    $scope.updateone = function () {
        var ddata = new data();
        $http.put('api/contacts/id',ddata).success(function (data) {
              alert("Update Successfully");
          }).error(function (data) {
              alert(data);
         });
    }
};
controllers.deleteone = function ($scope, $http) {
    $scope.deleteone = function () {
        $http.delete('api/contacts/' + $scope.iContactId).success(
        function (data) {
                        alert("successfuly");
        }).error(function (data) { alert("abort"); });
    }
};
crudmodule.controller(controllers);
