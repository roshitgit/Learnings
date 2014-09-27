self.generateExcelReport = function (jsonData) {

            $.ajax({
                type: 'POST',
                url: "api/ExcelReport/GenerateExcelReport",
                cache: false,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonData),
                success: function (data) {
                    var element = angular.element('<a/>');
                    element.attr({
                        href: 'data:attachment/xls;charset=utf-8,' + encodeURI(data),
                        target: '_blank',
                        download: 'CitiApplicationManagerPortal.xls'
                    })[0].click();
                }
            });
            
            }
