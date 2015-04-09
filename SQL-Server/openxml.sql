***** HANDLE EMPTY TAGS in OPEN XML *****
<Data>
  <Table>
    <Category_ID>23</Category_ID>
    <App_ID>165858</App_ID>
    <DescriptionText1 />
    <DescriptionText2>Error 01: TRTC = NULL</DescriptionText2>
    <DescriptionText3 />
    <DescriptionText4 />
    <DescriptionText5 />
    <DueDate>2014-12-26</DueDate>
    <F_01 />
    <F_02 />
    <F_03 />
    <F_04 />
    <ActionURL>https://......</ActionURL>
    <ActionDescription>Test of Action Description</ActionDescription>
    <ActionHelp>Test of Action Help</ActionHelp>
  </Table>
  </Data>
  
  
  SP
  IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[usp_SP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[usp_SP]
GO

CREATE PROCEDURE usp_SP
@XML XML, @modified_by varchar(10),@file_Name varchar(100)
AS
BEGIN
	DECLARE @XmlDocumentHandle INT
	EXEC sp_xml_preparedocument @XmlDocumentHandle OUTPUT, @XML, '<ROOT xmlns:xyz="urn:MyNamespace"/>'  
	BEGIN TRY
		--BEGIN TRAN	
				
			IF OBJECT_ID('tempdb..#temp') IS NOT NULL
			  DROP TABLE #temp

			CREATE TABLE #temp
				(Category_ID VARCHAR(100),
				App_ID VARCHAR(100),
				DescriptionText1 varchar(Max),
				DescriptionText2 varchar(Max),
				DescriptionText3 varchar(Max),
				DescriptionText4 varchar(Max),
				DescriptionText5 varchar(Max),
				DueDate varchar(50),
				F_01 varchar(100),
				F_02 varchar(100),
				F_03 varchar(100),
				F_04 varchar(100),
				ActionURL varchar(Max),
				ActionDescription varchar(Max),
				ActionHelp varchar(Max),
				successflag BIT,
				Error_Details VARCHAR(500)
				)
			INSERT INTO #temp (Category_ID,App_ID,DescriptionText1,DescriptionText2,DescriptionText3,
			   DescriptionText4,DescriptionText5,DueDate,F_01,F_02,
				F_03,F_04 ,ActionURL,ActionDescription,ActionHelp)
			SELECT  ........
			  NULLIF(DescriptionText1,''), NULLIF(DescriptionText2,''), 
				NULLIF(DescriptionText3,''), NULLIF(DescriptionText4,''), 
				NULLIF(DescriptionText5,''), DueDate, 
				NULLIF(F_01,''), NULLIF(F_02,''), 
				NULLIF(F_03,''), NULLIF(F_04,''), 
				NULLIF(ActionURL,''), NULLIF(ActionDescription,''), 
				NULLIF(ActionHelp,'')
			FROM OPENXML (@XmlDocumentHandle,'/Data/Table', 2)   -- 2 for element based xml
				  WITH (Category_ID VARCHAR(100),   
					  App_ID VARCHAR(100),
					  DescriptionText1 VARCHAR(MAX),
					  DescriptionText2 VARCHAR(MAX),
					  DescriptionText3 VARCHAR(MAX),
					  DescriptionText4 VARCHAR(MAX),
					  DescriptionText5 VARCHAR(MAX),					  
					  DueDate VARCHAR(50),
					  F_01 VARCHAR,
					  F_02 VARCHAR,
					  F_03 VARCHAR,
					  F_04 VARCHAR,
					  ActionURL VARCHAR(MAX),
					  ActionDescription VARCHAR(MAX),
					  ActionHelp VARCHAR(MAX))
         
         ...........................
         ......business rules here.................
         ......
					

				COMMIT TRAN

			END

		IF OBJECT_ID('tempdb..#temp') IS NOT NULL
		  DROP TABLE #temp
	END TRY 

      BEGIN CATCH 
		IF @@TRANCOUNT > 0 
			ROLLBACK TRAN
		IF OBJECT_ID('tempdb..#temp') IS NOT NULL
		  DROP TABLE #temp

          SELECT  Error_message() 
          SET nocount OFF 

          RETURN -1 
      END CATCH 	

	
END

