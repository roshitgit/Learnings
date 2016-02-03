** In OpenXML, always be aware that numeric fields cannot expect empty xml tags
ex: <CAL_YR_LABOR_ESTIMATE />

** To fix issue use validation within the "WITH" clause of open xml.
ex: CAL_YR_LABOR_ESTIMATE [numeric](10, 0) '/dataset/work/CAL_YR_LABOR_ESTIMATE[.!=""]', 

Link:
http://www.sqlservercentral.com/Forums/Topic13069-21-1.aspx

ALter existing column and add default value

ALTER TABLE dbo.[<table>]
Alter Column [<col1>] [numeric](10, 0) NOT NULL
GO 

ALTER TABLE [<table>] 
ADD DEFAULT 0 FOR [<col1>];
GO
