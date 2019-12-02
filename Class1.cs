using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Net;

namespace SharePointDocReadHelper
{
    public class SPDocReadHelper
    {
        public string username;
        public string password;
        public string url;
        public string siteTitle { get; set; }
        private ClientContext clientContext;
        Microsoft.SharePoint.Client.File file;

        /// <summary>
        /// The get client context.
        /// </summary>
        /// <returns>
        /// The <see cref="ClientContext"/>.
        /// </returns>
        private ClientContext GetClientContext()
        {
            ClientContext clientContext = new ClientContext(this.url)
            {
                Credentials =
                                                      new SharePointOnlineCredentials(
                                                      this.username,
                                                      this.GetSecureString(this.password))
            };
            return clientContext;
        }

        /// <summary>
        /// The get secure string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="SecureString"/>.
        /// </returns>
        private SecureString GetSecureString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input string is empty and cannot be made into a SecureString", "input");
            }

            var secureString = new SecureString();
            foreach (char c in input)
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }

        #region Constructor
        public SPDocReadHelper(String _url, String _username, String _password)
        {
            //this.Config = new DpToSharePointConfiguration();

            this.url = _url;
            this.username = _username;
            this.password = _password;

            this.clientContext = this.GetClientContext();
        }
        #endregion

        public Stream ReadDocument(string url)
        {
            file = clientContext.Web.GetFileByGuestUrl(url);

            clientContext.Load(file);
            clientContext.ExecuteQuery();

            ClientResult<Stream> data = file.OpenBinaryStream();
            clientContext.ExecuteQuery();

            MemoryStream memoryStream = new MemoryStream();

            if (data.Value != null)
            {
                data.Value.CopyTo(memoryStream);
            }

            return memoryStream;

        }

        public string docuName()
        {
            return file.Name;
        }
    }
}
