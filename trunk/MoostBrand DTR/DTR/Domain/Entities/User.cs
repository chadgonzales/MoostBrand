using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class User
    {
        public int Id { get; set; }
        public string EmpId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public int Active { get; set; }
    }
}