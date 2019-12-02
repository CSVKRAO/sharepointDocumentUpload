using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
//https://github.com/SharePoint/PnP/tree/master/Samples/Core.LargeFileUpload
namespace SharePointDocUploadHelper
{
    public class SPDocUploadHelper
    {
        public string username;
        public string password;
        public string url;
        public string siteTitle { get; set; }
        private ClientContext clientContext;

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
        /// To read a file stream and convert it into bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
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
        public SPDocUploadHelper(String _url, String _username, String _password)
        {
            //this.Config = new DpToSharePointConfiguration();

            this.url = _url;
            this.username = _username;
            this.password = _password;

            this.clientContext = this.GetClientContext();
        }
        #endregion

        public bool CreateFolderInDocs(string foldername)
        {
            bool ret = false;

            List doclist = clientContext.Web.Lists.GetByTitle("Documents");                                                              // This creates a 
            var folders = doclist.RootFolder.Folders;
            clientContext.Load(folders);
            clientContext.ExecuteQuery();
            try
            {
                var newFolder = folders.Add(foldername);
                clientContext.Load(newFolder);
                clientContext.ExecuteQuery();
                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;

        }

        // public string UploadDocument(string libraryName, String fileName, byte[] file)
        public string UploadDocument(string url, String fileName, String inputFile)
        {
            SPSite sps = new SPSite(url);
            var file = clientContext.Web.GetFileByUrl(url);// sps.OpenWeb();
            //SPFile file = spwCurrent.GetFile(fileName);
            clientContext.Load(file);
            clientContext.ExecuteQuery();


            var responseUrl = "";
            //Web web = clientContext.Web;
            //clientContext.Load(web);
            //clientContext.ExecuteQuery();

            ///// teams / NAJDOCS / Shared Documents / FDD028

            //string folderPath = string.Format("{0}/{1}", web.ServerRelativeUrl,
            //   libraryName);

            //var folder = clientContext.Web.GetFolderByServerRelativeUrl(folderPath);
            //clientContext.Load(folder);
            //clientContext.ExecuteQuery();

            ////MemoryStream stream = new MemoryStream(file);
            //FileStream stream = new FileStream(inputFile, FileMode.Open);
            //FileCreationInformation info = new FileCreationInformation
            //{
            //    ContentStream = stream,
            //    Url = fileName,
            //    Overwrite = true
            //};


            ////List docs = web.Lists.GetByTitle(libraryName);
            //Microsoft.SharePoint.Client.File uploadFile = folder.Files.Add(info);

            //clientContext.Load(uploadFile);
            //clientContext.ExecuteQuery();

            //responseUrl = new Uri(clientContext.Url).GetLeftPart(UriPartial.Authority)
            //                  + uploadFile.ServerRelativeUrl;




            return responseUrl;
        }
    }
}
