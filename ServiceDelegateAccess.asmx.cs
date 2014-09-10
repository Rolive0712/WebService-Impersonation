using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DelegateService.DAL;

namespace <ns>
{
    /// <summary>
    
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class <classname> : System.Web.Services.WebService
    {
        private DataAccess da = new DataAccess();

        [WebMethod(Description = "<description here>", EnableSession=true)]
        
        public int Add(string <param1>, string <param2>)
        {
            return da.AddDelegate(<param1>, <param2>);
        }
    }
}
