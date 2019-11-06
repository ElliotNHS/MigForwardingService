using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace MigForwardingLibrary
{
    public class MigForwardingService
    {
        private SemaphoreSlim _semaphoreSlim;
     
        public MigForwardingService() {
        }

        public void Start() {

            var config = new MigForwardingConfiguration();

            _semaphoreSlim = new SemaphoreSlim(0);
            while(true)
            {
                Console.WriteLine("It is {0} and all is well.", DateTime.Now);
                Thread.Sleep(5000);

                if (_semaphoreSlim.Wait(500))
                {
                    OnStopping();
                    break;
                }
           }
                }
        public void Stop() {
            _semaphoreSlim.Release();

        }
        public void OnStopping()
        {
            Console.WriteLine("MIG forwarding service is stopping.");
            Console.WriteLine("Press any key to exit the program.");
            Console.ReadKey();
            // Here any connections would be closed, log files would be finished, 
            //release any references to unmanaged code, any clean up activity
        }
    }
}
