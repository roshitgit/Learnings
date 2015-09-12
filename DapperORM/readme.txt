With Dapper Install other packages also
* install-package Dapper
* install-package Dapper.Contrib
* install-package Dapper.Extensions

***Why doesn't Dapper dot net open and close the connection itself?
http://stackoverflow.com/questions/12628983/why-doesnt-dapper-dot-net-open-and-close-the-connection-itself

****non-buffered-queries-in-dapper
http://stackoverflow.com/questions/23023534/managing-connection-with-non-buffered-queries-in-dapper

*** using output params and return values with dapper
http://stackoverflow.com/questions/22353881/passing-output-parameters-to-stored-procedure-using-dapper-in-c-sharp-code

** using dapper contrib
https://github.com/xliang/dapper-net-sample

http://devpacks.com/nuget/tag/dapper -- all package list

** dapper with transaction support
http://blog.akra-tech.com/post/C

http://coderissues.com/questions/31246977/dal-with-dapper-and-c-sharp

http://www.nudoq.org/#!/Packages/Dapper.Rainbow.PostgreSql

**** all tests
https://github.com/StackExchange/dapper-dot-net/blob/master/Tests/Tests.cs ------ vgood

*****DapperRainbow-VS-DapperContrib
http://en.efreedom.net/Question/1-10030285/DapperRainbow-VS-DapperContrib

* Dapper.net how to create a map
http://www.kacode.com/v294921-.html
https://github.com/randyburden/Slapper.AutoMapper

** handling nulls in dapper input params
http://programwith.com/question_4115354_how-can-i-send-a-string-as-null-to-sqlserver-using-dapper

* Set Column Attribues in Dapper
http://refactorthat.com/2012/03/13/adding-some-snaz-to-dapper/
http://weizhishi.com/questions/96287/dapper-with-attributes-mapping
http://stackoverflow.com/questions/8902674/manually-map-column-names-with-class-properties
http://stackoverflow.com/questions/8902674/manually-map-column-names-with-class-properties/30200384#30200384

* multiple resultsets
http://stackoverflow.com/questions/6317937/dapper-net-and-stored-proc-with-multiple-result-sets
http://www.ciiycode.com/0iyJHqqWPqPW/dapper-multi-mapping-with-querymultiple
http://tiku.io/questions/1079788/dapper-gridreader-already-disposed-error

*StackExchange - dapper-dot-net
https://github.com/StackExchange/dapper-dot-net ------- all about dapper

* net-microorms--dapper-petapoco-and-more
http://www.nullskull.com/a/1659/net-microorms--dapper-petapoco-and-more.aspx

* one-to-many relationship
http://www.programask.com/question_31578218_best-way-to-extract-a-one-to-many-relationship-with-dapper-dot-net-orm

* dapper-and-anonymous-types
http://stackoverflow.com/questions/6147121/dapper-and-anonymous-types

**using-dapper-with-mdx
http://www.dynamictyped.com/2011/08/20/using-dapper-with-mdx/

* creating-data-repository-using-dapper
http://www.bradoncode.com/blog/2012/12/creating-data-repository-using-dapper.html

* Creating type dynamically at runtime for dapper querymultiple Read method using Reflection
http://stackoverflow.com/questions/24592749/creating-type-dynamically-at-runtime-for-dapper-querymultiple-read-method-using

* dapper-reader-disposed-exception
http://www.4byte.cn/question/436557/dapper-reader-disposed-exception.html

* dapper-net-samples
https://liangwu.wordpress.com/2012/08/16/dapper-net-samples/


generic repository
* www.contentedcoder.com/2012/12/creating-data-repository-using-dapper.html

http://www.c-sharpcorner.com/UploadFile/e4e3f7/dapper-king-of-micro-orm-C-Sharp-net/

Dapper Extensions
* https://github.com/tmsmith/Dapper-Extensions

Handling out parameters & passing dynamic parameters
http://stackoverflow.com/questions/5962117/is-there-a-way-to-call-a-stored-procedure-with-dapper
http://stackoverflow.com/questions/8011225/can-i-use-dynamicparameters-with-template-and-have-a-return-parameter-in-dapper
http://code.google.com/p/dapper-dot-net/issues/detail?id=30 -- using anonymous object

Must haves for dapper
* http://nugetmusthaves.com/Tag/Dapper

* write the code below to enable ANSI input strings.
bydefault dapper sends input params as NVARCHAR. so is sp accepts varchar, then the sql query with dapper will result in a
index scan rather than a index seek. this might hamper query performance. So to enable input params to be of ANSI varchar, type code below.

Dapper.SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);
* http://stackoverflow.com/questions/6386069/can-ansistrings-be-used-by-default-with-dapper

Dapper Fluent API
* https://github.com/beardeddev/dapper-fluent/blob/master/Dapper.Fluent/DbManager.cs
