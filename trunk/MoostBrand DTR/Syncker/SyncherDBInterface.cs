using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Syncker
{
    class SyncherDBInterface : DBInterface
    {
        public override string GetConnString()
        {
            //var userPath = Environment.GetFolderPath(Environment
            //                                 .SpecialFolder.ApplicationData);
            //var filename = Path.Combine(userPath, "serversettings.txt");

            //var connectionString = File.ReadAllText(filename);

            //return connectionString;

            return ConfigurationManager.ConnectionStrings["DBConnSynch"].ConnectionString;
        }
    }
}