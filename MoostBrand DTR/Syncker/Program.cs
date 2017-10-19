using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTR;
using System.Threading;

namespace Syncker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialized Jentec DTR Synchronizer");
            Console.WriteLine("...");
            Console.WriteLine("");
            Console.WriteLine("Synchronizing employees...");
            try
            {
                EmployeeSynchronizer empSyncher = new EmployeeSynchronizer();
                empSyncher.Synchronize();

                Console.WriteLine("Synchronizing logs...");
                LogSynchronizer logSyncher = new LogSynchronizer();
                logSyncher.Synchronize();

                Console.WriteLine("Synchronizing employee registration...");
                EmployeeRegistrationSynchronizer regSyncher = new EmployeeRegistrationSynchronizer();
                regSyncher.Synchronize();

                Console.WriteLine("Process complete.");

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                Thread.Sleep(5000);
            }   
        }
    }
}