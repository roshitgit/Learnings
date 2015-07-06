(function () {
    require.config({

        baseUrl: URL.BaseURL,
        waitSeconds: 20, //wait for 20 seconds to load file. else quit (default is 7)
        // alias libraries paths
        paths: {
            domReady: 'vendor/requirejs-domready/domReady',
            jquery: 'vendor/jquery/jquery',
            angular: 'vendor/angular/angular',
            underscore: 'vendor/underscore/underscore',
            'ui.router': 'vendor/angular-ui-router/angular-ui-router',
            ngTable: 'vendor/ng-table/ng-table.min',
            gChart: 'vendor/ng-google-chart/ng-google-chart',
            'ui.bootstrap': 'vendor/angular-ui-bootstrap/ui-bootstrap-0.8.0',
            'ui.bootstrap.tpls': 'vendor/angular-ui-bootstrap/ui-bootstrap-tpls-0.8.0',
            jqueryScrollbar: 'vendor/jquery-scrollbar/jquery-scrollbar.min',
            moment: 'vendor/moment/moment.min',
            toastr: 'vendor/toastr/toastr.min',
            hopscotch: 'vendor/hopscotch/js/hopscotch.min',
            sitetour: 'components/SiteTour/SiteTour',
            alertify: 'vendor/alertify/alertify.min',
            bootstrapCollapse: 'vendor/bootstrap/js/bootstrap-collapse',
            amplify: 'vendor/amplify/amplify.min',
            jlinq: 'vendor/jlinq/jlinq',
            'underscore.string': 'vendor/underscore/underscore.string.min',
            'angular.filter': 'vendor/angular-filters/angular-filter.min',
            'ui.filters': 'vendor/angular-filters/unique',
            bindonce: 'vendor/bindonce/bindonce.min'
        },

        /*  angular, googlechart, ui router & other plugins does not support 
        AMD out of the box, put it in a shim
        */
        shim: {
            toastr: {
                exports: 'toastr',
                deps: ['jquery']
            },
            angular: {
                exports: 'angular',
                deps: ['jquery']
            },
            'ui.router': {
                deps: ['angular']
            },
            ngTable: {
                deps: ['angular']
            },
            jqueryScrollbar: {
                deps: ['angular']
            },
            gChart: {
                deps: ['angular']
            },
            'ui.bootstrap': {
                deps: ['angular']
            },
            'ui.bootstrap.tpls': {
                deps: ['ui.bootstrap']
            },
            hopscotch: {
                exports: 'hopscotch',
                deps: ['jquery']
            },
            sitetour: {
                exports: 'sitetour',
                deps: ['hopscotch'] //to ensure hopscotch plugin gets loaded first before getting the tour data
            },
            alertify: {
                exports: 'alertify',
                deps: ['jquery']
            },
            jlinq: {
                exports: 'jlinq'
            },
            amplify: {
                exports: 'amplify',
                deps: ['jquery']
            },
            'angular.filter': {
                exports: 'angular.filter',
                deps: ['angular']
            },
            'ui.filters': {
                exports: 'ui.filters',
                deps: ['angular']
            },
            bindonce: {
                deps: ['angular']
            }
        },

        // kick start application
        deps: ['bootstrap']
    });

})();

