Try out these 2 things: It's done on CAMP app
1. IIS app pool recycling.
2. Set web content expiration under "HTTP Response Headers" in features view of IIS of the concerned website.
        OR
3. use "Output Caching" feature as shown in link below. (put "Cache-Control" headers on advanced popup)
    http://madcomputerist.blogspot.com/2012/02/preventing-caching-of-javascript-files.html --- excellent. try and confirm
   This approach is recommended.

Note: 
* After every publish of the code in IIS, the IIS settings will be wiped out if the cache settings were not made in the web.config
* So set the cache headers in application web.config so after deployment the settings will stick in IIS as IIS will pick up from config file.
* If not sure how to setup in config file then do manual in IIS and save. The xml elements will be updated in web.config by IIS.

** all good ones here
http://blog.janjonas.net/2011-08-21/microsoft-iis-7-enable-client-side-browser-caching-static-content-web-config
http://forums.asp.net/t/1698433.aspx?How+to+Leverage+browser+caching+at+ASP+net+IIS+7+5
http://stackoverflow.com/questions/3929284/how-do-i-disable-caching-of-an-individual-file-in-iis-7-using-weserver-config-se
http://stackoverflow.com/questions/2195266/how-to-configure-static-content-cache-per-folder-and-extension-in-iis7/4821328#4821328


http://www.galcho.com/blog/post/2008/02/27/IIS7-How-to-set-cache-control-for-static-content.aspx --- vgood
http://serverfault.com/questions/178493/make-iis-7-5-cache-static-content-files-over-diferent-pages ---- vgood
https://gregsramblings.com/2012/05/28/html5-application-cache-how-to/
http://www.iis.net/configreference/system.webserver/caching
http://www.iis.net/learn/manage/managing-performance-settings/configure-iis-7-output-caching
http://serverfault.com/questions/126914/output-caching-with-iis7-how-to-for-an-dynamic-aspx-page  ---- vgood config setting.
http://stackoverflow.com/questions/32933449/how-to-enable-iis-level-output-caching-for-a-mvc-route --- vgood
http://stackoverflow.com/questions/2022960/iis7-set-no-cache-for-all-aspx-pages-but-not-images-css-js
http://superuser.com/questions/63014/how-do-i-turn-off-caching-in-iis7
http://marvelley.com/2011/04/07/preventing-aggressive-caching-of-javascript-files-with-iis7/
http://stackoverflow.com/questions/642954/iis7-cache-control
http://forums.iis.net/post/1973585.aspx
http://blogs.iis.net/ksingla/caching-in-iis7
http://serverfault.com/questions/176943/will-an-iis-reset-force-cached-items-to-be-resent

