(function () {


    var columnDefs = [
        // this row shows the row index, doesn't use any data from the row
        {
            headerName: "#", width: 50, cellRenderer: function (params) {
                return params.node.id + 1;
            }
        },
        { headerName: "ID", field: "id", width: 250 },
        { headerName: "LastName", field: "name", width: 250 },
        { headerName: "Gender", field: "gender", width: 120 },
        { headerName: "Age", field: "age", width: 250 },
        { headerName: "Date", field: "date", width: 200 },
        { headerName: "URL", field: "Url", width: 200 },
        { headerName: "Priority", field: "Priority", width: 300 }
    ];

    var gridOptions = {
        enableColResize: true,
        //debug: true,
        //rowSelection: 'multiple',
        //rowDeselection: true,
        columnDefs: columnDefs,
        enableFilter: true
        //rowModelType: 'virtual'
    };

    function onFilterChanged(value) {
        gridOptions.api.setQuickFilter(value);
    }


    function setRowData(allOfTheData) {
        //var dataSource = {
        //    rowCount: null, // behave as infinite scroll
        //    pageSize: 100,
        //    overflowSize: 100,
        //    maxConcurrentRequests: 2,
        //    maxPagesInCache: 2,
        //    getRows: function (params) {
        //        console.log('asking for ' + params.startRow + ' to ' + params.endRow);
        //        // At this point in your code, you would call the server, using $http if in AngularJS.
        //        // To make the demo look real, wait for 500ms before returning
        //        setTimeout(function () {
        //            // take a slice of the total rows
        //            var rowsThisPage = allOfTheData.slice(params.startRow, params.endRow);
        //            // if on or after the last page, work out the last row.
        //            var lastRow = -1;
        //            if (allOfTheData.length <= params.endRow) {
        //                lastRow = allOfTheData.length;
        //            }
        //            // call the success callback
        //            params.successCallback(rowsThisPage, lastRow);
        //        }, 100);
        //    }
        //};

        //gridOptions.api.setDatasource(dataSource);
        gridOptions.api.setRowData(allOfTheData);
    }

    $(document).ready(function () {
        var gridDiv = document.querySelector('#myGrid');
        new agGrid.Grid(gridDiv, gridOptions);

        $.ajax({
            url: '/api/Data/GetRecordsForUiGrid',
            type: "GET",
            cache: false,
            success: function (response) {
                setRowData(response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    });


})();


