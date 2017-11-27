using AFNIAPI.Classes;
using AFNIAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AFNIAPI.Controllers
{
    [RoutePrefix("api/AFNI")]
    public class AFNIController : ApiController
    {
        [HttpGet]
        [Route("LogsList")]
        public List<AFNILogs> LogsList()
        {
            AfniDTREntities _entities = new AfniDTREntities();

            List<AFNILogs> _logsList = new List<AFNILogs>();

            var _logs = _entities.Logs.ToList();

            foreach (Log _log in _logs)
            {
                AFNILogs _logsClass = new AFNILogs();
                _logsClass.ID = _log.ID;
                _logsClass.username = _log.username;
                _logsClass.timeInOut = _log.timeInOut;
                _logsClass.logType = _log.logType;
                _logsList.Add(_logsClass);
            }

            return _logsList;
        }
    }
}
