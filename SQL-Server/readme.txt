** In OpenXML, always be aware that numeric fields cannot expect empty xml tags for doing table inserts
ex: <CAL_YR_LABOR_ESTIMATE />

** To fix issue use validation within the "WITH" clause of open xml.
ex: CAL_YR_LABOR_ESTIMATE [numeric](10, 0) '/dataset/work/CAL_YR_LABOR_ESTIMATE[.!=""]', 

** But this causes major performance issues while inserting records.
** ex, it will take approx 25 min to insert an excel file with 67 columns & 3000 rows.
** thereby better to modify all numeric fields in "OPEN XML" WITH CLAUSE to "varchar" & remove '/dataset/work/CAL_YR_LABOR_ESTIMATE[.!=""]'. this then takes few seconds to execute.

Link:
http://www.sqlservercentral.com/Forums/Topic13069-21-1.aspx


*** ALter existing column and add default value
ALter existing column and add default value

ALTER TABLE dbo.[<table>]
Alter Column [<col1>] [numeric](10, 0) NOT NULL
GO 

ALTER TABLE [<table>] 
ADD DEFAULT 0 FOR [<col1>];
GO
