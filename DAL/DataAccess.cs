using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;


namespace <ns>
{
    public class DataAccess
    {
        private static string strConnection;

        /// <summary>
        /// 
        /// </summary>
        public DataAccess()
        {
            strConnection = Encryption.Decrypt(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        /// <summary>
        /// The method executes the stored procedure using (impersonating) the identity of the windows domain account.
        /// the account has access on the sql server database.
        /// </summary>
        
        /// <returns></returns>
        public int Add(string <param1>, string <param2>)
        {
            try
            {
                var DomainAccountUserNameToImpersonate = ConfigurationManager.AppSettings["DomainAccountUserNameToImpersonate"].ToString().Trim();
                var DomainAccountPasswordToImpersonate = Encryption.Decrypt(ConfigurationManager.AppSettings["DomainAccountPasswordToImpersonate"].ToString().Trim());
                var DomainName = ConfigurationManager.AppSettings["DomainName"].ToString().Trim();

                using (new Impersonation(DomainName, DomainAccountUserNameToImpersonate, DomainAccountPasswordToImpersonate))
                {
                    SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(strConnection, "<SP NAME>", true);

                    SqlHelper.AssignParameterValues(commandParameters, new object[] { 0, <param1>, <param2> });
                    SqlHelper.ExecuteNonQuery(strConnection, CommandType.StoredProcedure, "<SP NAME>", commandParameters);

                    return (int)commandParameters[0].Value;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    new Emailer().SendMail(ex.Message, "FAILURE");
                }
                catch (Exception){}
                
                return -1;
            }
            finally
            {
                //this.CloseConnection();
            }
        }
    }
}
