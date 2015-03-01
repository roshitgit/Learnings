angular.module('app.services', [])
    .service("$dataService", [
        "$http", function($http) {
            var baseLocal = "http://localhost/api";
            var baseRemote = "http://example.com/api";
            var baseURL = baseRemote;

            return {
                getAllRecords: function(datatype, onSuccess, onError) {
                    var url = baseURL + "/" + datatype;
                    console.log("GET: " + url);
                    $http.get(url).success(onSuccess).error(onError);
                },

                getRecord: function(datatype, id, onSuccess, onError) {
                    var url = baseURL + "/" + datatype + "/" + id;
                    console.log("GET: " + url);
                    $http.get(url).success(onSuccess).error(onError);
                },

                createRecord: function(datatype, data, onSuccess, onError) {
                    var url = baseURL + "/" + datatype;
                    console.log("POST: " + url);
                    $http.post(url, data).success(onSuccess).error(onError);
                },

                updateRecord: function(datatype, data, onSuccess, onError) {
                    var url = baseURL + "/" + datatype;
                    console.log("PUT: " + url);
                    $http.put(url, data).success(onSuccess).error(onError);
                },

                deleteRecord: function(datatype, id, onSuccess, onError) {
                    var url = baseURL + "/" + datatype + "/" + id;
                    console.log("DELETE: " + url);
                    $http.delete(url, id).success(onSuccess).error(onError);
                }
            }
        }
    ]);

In your AngularJS controller, you can get all the records of a particular type like this:


$dataService.getAllRecords("datatype", function(data, responseCode) {
            // This is the success function, called after the service returns a success response
            // Perform any logic that would happen after the request, such as updating the UI.
            $scope.formdata = data;
        }, function(data, responseCode) {
            console.log("Failed to load data");
            // Provide more details about the error here
        });

If you want to create a record, you would do it like this:


$dataService.createRecord("datatype", jsonData, function(data, responseCode) {
                // This is the success function, called after the service returns 
                // a HTTP response in the 200 range
                // Perform any logic that would happen after the request, such as updating the UI.
                console.log(responseCode);
            }, function(data) {
                console.log("Failed to add record to the database");
                // Provide more details about the error here
            });
