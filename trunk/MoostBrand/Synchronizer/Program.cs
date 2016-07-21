using Synchronizer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Process();
            Console.ReadLine();
        }

        static async void Process()
        {
            Utils _utils = new Utils();
            await _utils.ProcessRequisition();
            await _utils.ProcessRequisitionDetails();
            await _utils.ProcessStockTransfer();
            await _utils.ProcessStockTransferDetails();
            await _utils.ProcessReceiving();
            await _utils.ProcessReceivingDetails();
            await _utils.ProcessStockAllocation();
            await _utils.ProcessStockAllocationDetails();
            await _utils.ProcessReturn();
            await _utils.ProcessReturnedItems();
            await _utils.ProcessStockAdjustment();
            await _utils.ProcessStockAdjustmentDetails();
        }
        
    }
}
