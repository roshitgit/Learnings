Objective:
1. Create .msg file at runtime using ASP.NET
2. use Aspose.Email.dll for the same.
3. Write .msg files on another server preferablya File Server with sufficient disk space
   This server will be meant for uploading, writing and downloading files (viz: xls,xlsx,etc)
4. Provide to way to open the new created .msg files from IE browser
5. Thereby create a VD inside the application's  website where the code is hosted in IIS.
6. So the path will be http://<machinename.domain>/<VDNameForFileServerMapping>/<path-to-msg-file>

FileServer:
NAS FileServer FID:
Username: 	<domain>\<username>
Pass:		<password>

NAS Drives:
Dev NAS: \\<servername>\<folders>



To setup VD:

Steps to setup NAS VD mapping in IIS 7.5 (Win 2008 R2 server)

1.	Setup VD mapped to NAS drive under TAPortal website in IIS. (as shown below)
2.	On website specify mime type for .msg files as shown below.
Note:  The entry type will show up as “local” on website but will show up as “inherited” on VD mapped below the website
        Setup mime type only on the website and not on Virtual directory.
3.	While creating the VD, use the Connect As button to specify the FID credentials
4.	Do a test connection of the FID to verify the connection. 
Authentication & Authorization should pass
5.	After completing the setup, right click on VD and go to advanced settings. 
 
