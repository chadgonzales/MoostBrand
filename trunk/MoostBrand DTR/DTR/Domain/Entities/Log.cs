using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class Log
    {
        public int Id { get; set; }
        public string EmpId { get; set; }
        public DateTime ScanDate { get; set; }
        public string Location { get; set; }
        public bool Sent { get; set; }
        public bool LogType { get; set; }
    }
}
