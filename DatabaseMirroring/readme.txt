* This is Failsafe. If the production database server goes down, the contingency db server (COB Server) should take over.
* But for the seamless transition to happen from the UI web app, the UI connection string should have "Failover Partner" mentioned on the conenction string.

* In this case first step will be to setup "Mirroring" on the contingency (COB) server.
* Next step would be to setup "failover partner" on the UI connection string.

** sample conn string with failover
Data Source=<server>\<instance>; Failover Partner=<server>\<instance>; Trusted_Connection=True; User Id=<domain>\<uid>;Password=<password>; Database=<database>;Connect Timeout=500;pooling='true'; Max Pool Size=100;


** failover partner
https://msdn.microsoft.com/en-us/library/5h52hef8(v=vs.110).aspx
https://msdn.microsoft.com/en-us/library/ms175484.aspx
http://blogs.msdn.com/b/spike/archive/2010/12/08/clarification-on-the-failover-partner-in-the-connectionstring-in-database-mirror-setup.aspx
