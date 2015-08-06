** scheduling .bat file or "windows command script" .cmd file on windows 2008 r2
http://www.blogfodder.co.uk/2012/4/20/win-2008-task-scheduler-with-return-code-1-0x1

Note:
1. Provide folder path under "Start in (optional)" in Action/ Edit window.
2. Running the job manually should return the message shown below. return code should be 0 for a success. 
   If its 1, then job is failing.

The message shown below will be displayed in "History" tab. click on "Action Completed" to see the information on general tab for the selected category

Task Scheduler successfully completed task "\sendfile" , instance "{17819a24-e817-4653-af79-ac21003e1462}" , 
action "C:\Windows\SYSTEM32\cmd.exe" with return code 0.
