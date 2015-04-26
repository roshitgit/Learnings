
***Integrate Node.js & Angular.js in Web based MVC Application
http://technet.weblineindia.com/web/integrate-node-js-angular-js-in-web-based-mvc-application/

/**
*Moduledependencies.
*/
 
//Below code will load requires packages for Application
 
var express = require('express')
 
, http = require('http')
 
, path = require('path');
 
//This will start the our application as express based web application
 
var app = express();
 
//Here we have defined our development configuration and we have defined directory structure of our application.
 
//Here views will set our HTML pages folder
 
app.configure('development', function(){
 
app.use(express.static(path.join(__dirname, '.tmp')));
 
app.use(express.static(path.join(__dirname, 'app')));
 
app.use(express.errorHandler());
 
app.set('views', __dirname + '/app/views');
 
});
 
//This will be used as our production environment
 
app.configure('production', function(){
 
app.use(express.favicon(path.join(__dirname, 'public', 'favicon.ico')));
 
app.use(express.static(path.join(__dirname, 'public')));
 
app.set('views', __dirname + '/views');
 
});
 
// all environments
 
//For using HTML as our view technology we need to load ejs package at here
 
app.engine('HTML', require('ejs').renderFile);
 
//Here we set our view engine as HTML
 
app.set('view engine', 'HTML');
 
//Here all the contents from HTML will be parsed in express.js and rendered on page
 
app.use(express.favicon());
 
app.use(express.logger('dev'));
 
app.use(express.bodyParser());
 
app.use(express.methodOverride());
 
//here we have defined our application routes configuration
 
app.use(app.router);
 
//from here our routes for angular.js will be used
 
require('./lib/config/routes')(app);
 
//Now our server will be hosted on particular port
 
var port = process.env.PORT || 8082;
 
app.listen(port, function () {
 
console.log('Express server listening on port %d in %s mode', port, app.get('env'));
 
});
