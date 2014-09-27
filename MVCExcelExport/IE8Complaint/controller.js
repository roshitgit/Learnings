$scope.getDataToExport = function (event) {
            
            var url = URL.GenerateExcelReport + "RIOwner=" + geid;
            window.location = url;
            event.returnValue = false;
            return false;
        };
