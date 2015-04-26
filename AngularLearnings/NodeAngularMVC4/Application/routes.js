

1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
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
