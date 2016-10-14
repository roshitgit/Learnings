Both code files should be compiled into a .dll and then used in the application

* If class "CodeToImpersonateFullTrust-1.cs" is used, then the FID need not to be added as "Administrator" on server as it executes with full permissions.
( This class compiled as dll should be the ideal approach)

* If class "CodeToImpersonate-LimitedTrust-2.cs" is used, then the FID has to be added as "Administrator" on server as it does not execute with full permissions
( This approach is not recommended)
