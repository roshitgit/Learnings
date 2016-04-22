1. Onboard web application into EB.NET
   Project Collection Name    ------- <TFS CollectionName> 
   Specify the Environment you are on-boarding onto (TFS PROD or TFS UAT)   ------ TFS PROD
   Name of the Team Project within the project collection  -------- < TFS repository Name >
   App Id for the team project   ---- <App ID>
   Your business sector --- <Sector>
   Primary contact person's email  ----- <contact id>

2. After onboard, create test EB Build package from VS 2013.
3. After successful test build, onboard to automated build tool (RLM) specifying the test package build id.
