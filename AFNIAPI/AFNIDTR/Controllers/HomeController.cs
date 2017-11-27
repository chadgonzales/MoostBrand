using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AFNIDTR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
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
                       var test =  Newtonsoft.Json.JsonConvert.DeserializeObject(content);


                        //List<EmployeeInfo> info = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeInfo>>(content);

                        //foreach (var _employee in info)
                        //{
                        //    Employee _e = new Employee();
                        //    _e.No = _employee.Number;
                        //    _e.FirstName = _employee.FirstName;
                        //    _e.LastName = _employee.LastName;

                        //    _entity.Employees.Add(_e);
                        //    _entity.SaveChanges();
                        //    Console.WriteLine("Successfully added Employee " + _e.No);
                        //}

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

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}