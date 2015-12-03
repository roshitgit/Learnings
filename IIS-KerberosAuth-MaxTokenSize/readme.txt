The MaxTokenSize by default is 12,000 bytes. This has been the default value since Windows 2000 SP2 and still 
remains in Windows 7 and Windows 2008 R2. As company’s grow so do the groups within your organization.  
If your Kerberos token becomes too big your users will receive error messages 
during login and applications that use Kerberos authentication will potentially fail as well.

CAMP application failed and the user could not login to CAMP because his AD account had 175 group memberships.
To fix, change registry settings on web server. check links below.

Note: CAMP app used "Kerberos" authentication. Website was configured to use Windows Authentication with Custom Identity (SQL server Domain account - FID).

http://blogs.technet.com/b/shanecothran/archive/2010/07/16/maxtokensize-and-kerberos-token-bloat.aspx
http://blogs.technet.com/b/surama/archive/2009/04/06/kerberos-authentication-problem-with-active-directory.aspx
http://www.grouppolicy.biz/2013/06/how-to-configure-iis-to-support-large-ad-token-with-group-policy/
