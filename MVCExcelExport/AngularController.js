$scope.getDataToExport = function (e) {
           

            var url = '@Url.Content("~/ExcelReport/GenerateExcelReport?")' + "<param1>=" + Trim(id);
            window.location = url;
        };
