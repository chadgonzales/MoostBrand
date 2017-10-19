using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTR;
using System.Data.SqlClient;

using Newtonsoft.Json;

namespace Syncker
{
    class LogSynchronizer : SyncherDBInterface
    {
        LogRepo logRepo = new LogRepo();

        //Synchronize from local to main
        public void Synchronize()
        {
            //Get list of unsynchronized logs from local
            List<Log> lstUnsyncedLog = logRepo.ListUnsynched();

            //Initialize logs web service from main
            LogsService.LogsServiceSoapClient logsServiceMain = new LogsService.LogsServiceSoapClient();

            foreach (Log _log in lstUnsyncedLog) {
                int isSyncSuccessful;
                isSyncSuccessful = logsServiceMain.Synchronize(JsonConvert.SerializeObject(_log));
                if (isSyncSuccessful == 1) {
                    logRepo.MarkAsSent(_log.Id);
                }
            }
        }
    }
}