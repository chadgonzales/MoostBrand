using AFNIAPI.Classes;
using AFNIAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                //_logsClass.ID = _log.ID;
                _logsClass.username = _log.username;
                _logsClass.timeInOut = _log.timeInOut;
                _logsClass.logType = _log.logType;
                _logsList.Add(_logsClass);
            }
            LogsService();
            return _logsList;
        }

        public void LogsService()
        {
            try
            {

                AfniDTREntities _entities = new AfniDTREntities();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:50197/api/AFNI/LogsList");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";
                httpWebRequest.Accept = "application/json; charset=utf-8";

                try
                {
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

                        WebHeaderCollection header = response.Headers;

                        string content;

                        var encoding = ASCIIEncoding.ASCII;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            content = reader.ReadToEnd();
                        }

                        List<AFNILogs> info = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AFNILogs>>(content);

                        foreach (var _logs in info)
                        {
                            Log _l = new Log();
                            //_l.ID = _logs.ID;
                            _l.username = _logs.username;
                            _l.timeInOut = _logs.timeInOut;
                            _l.logType = _logs.logType;

                            _entities.Logs.Add(_l);
                            _entities.SaveChanges();
                        }

                    }
                    catch (Exception err)
                    {
                        string message = err.Message;
                    }
                }
                catch (WebException e)
                {

                }
            }
            catch { }
        }

    }
}
