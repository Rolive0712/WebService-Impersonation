1. Add service reference (using ?wsdl path) to the web service on the UI web application
2. This is the same web service which impersonates the windows domain account.

public partial class _Default : System.Web.UI.Page
    {
        DelegateAccessProxy.ServiceDelegateAccessSoapClient client = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                client = new DelegateAccessProxy.ServiceDelegateAccessSoapClient();

                //To impersonate from the consumer (client), provide credentials as shown below.
                //client.ClientCredentials.UserName.UserName = "<name here>";
                //client.ClientCredentials.UserName.Password = "<password here>";
                
                int retval = Convert.ToInt32(client.AddDelegate(<ID1>, <ID2>));
                if (retval == 1)
                {
                    lblResponse.Text = "Success: ";
                }
                else
                {
                    lblResponse.Text = "Internal Server Error!!!.";
                    lblResponse.Style.Value = "color: Red; font-style: italic";
                }
            }
            catch (Exception ex)
            {
                new Emailer().SendMail(ex.Message, "Failure");
            }
            
        }

    }
