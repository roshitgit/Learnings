
'use strict';
 
//path will be used as route configuration package
 
var path = require('path');
 
//Here application paths will be set
 
module.exports = function (app) {
 
//all the request/response will be handled using following code
 
// Angular Routes
 
app.get('/partials/*', function (req, res) {
 
var requestedView = path.join('./', req.url);
 
res.render(requestedView);
 
});
 
//application will load first on index.html
 
app.get('/*', function (req, res) {
 
res.render('index.html');
 
});
 
}
