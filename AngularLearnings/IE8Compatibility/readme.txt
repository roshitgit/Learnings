** AngularJS 1.3 / 1.4 does not work with IE8. With these custom builds you get good IE8 support with "Pasvaz" BindOnce plugin
** Bindonce plugin then starts working on IE 8 browser after using this angular version.
** AngularJS 1.3 has one-time binding support
https://github.com/fergaldoyle/angular.js-ie8-builds
Note: after using this build (version 1.4) in angular app, ui datepicket formatting issue will come up while selecting date.
To fix use the directive as shown in link below
http://stackoverflow.com/questions/24198669/angular-bootsrap-datepicker-date-format-does-not-format-ng-model-value


http://stackoverflow.com/questions/27384301/angular-1-3-one-way-binding-ie8-support
Not that it would not be a solution to replace every :: binding, but I don't think it would be an efficient one or pretty for that matter. If IE8 support is crucial for you, I would suggest using this very popular bind once library: 
https://github.com/Pasvaz/bindonce. 
I've used it before 1.3 came along and it served me well.

https://github.com/angular-ui/bootstrap/issues/359
