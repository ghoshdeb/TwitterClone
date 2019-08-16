using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace TwitterClone.UI.CustomFilter
{
    public class MyErrorHandler:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext.Exception);
            base.OnException(filterContext);            
        }
        public void LogError(Exception obj)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\Documents\Visual Studio 2017\Projects\errorInfo.txt", true))
            {
                string errorContent = "Error: " + obj.GetBaseException().Message + "\n" + "Date:" + DateTime.Now;
                sw.WriteLine(errorContent);
            }
        }
    }
}