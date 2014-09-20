 * Write code below in the "Main" method of program class.
 static void Main()
        {
         
#if (!DEBUG)
    System.ServiceProcess.ServiceBase[] ServicesToRun;
    ServicesToRun = new System.ServiceProcess.ServiceBase[] { new Service1() };
    System.ServiceProcess.ServiceBase.Run(ServicesToRun);
#else
            // Debug code: this allows the process to run as a non-service.
            // It will kick off the service start point, but never kill it.
            // Shut down the debugger to exit
            Service1 service = new Service1();
            service.Start();
            // Put a breakpoint on the following line to always catch
            // your service when it has finished its work
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
        
/// <summary>
/// Call this method directly while debugging from program class.
/// </summary>      
public void Start()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
        }
