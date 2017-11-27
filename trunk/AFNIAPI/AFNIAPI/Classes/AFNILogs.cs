using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFNIAPI.Classes
{
    public class AFNILogs
    {
        public int ID { get; set; }
        public string username { get; set; }
        public Nullable<DateTime> timeInOut { get; set; }
        public string logType { get; set; }
    }
}