using System;
using System.Linq;
using CG.Web.MegaApiClient;

namespace MvcSqlServerWebBackup
{
    public class MegaCloud
    {
        public static bool TestCloud(CloudDrive value, out string message )
        {
            MegaApiClient client = new MegaApiClient();
            try
            {
                client.Login(value.Uid, value.Password);
                client.Logout();
                message = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
            
            //var nodes = client.GetNodes();

            //INode root = nodes.Single(n => n.Type == NodeType.Root);
            //INode myFolder = client.CreateFolder("Upload", root);

            //INode myFile = client.UploadFile("MyFile.ext", myFolder);

            //Uri downloadUrl = client.GetDownloadLink(myFile);
            //Console.WriteLine(downloadUrl);
        }

        public static bool UploadToCloud(CloudDrive value, string file, out string message)
        {
            MegaApiClient client = new MegaApiClient();
            try
            {
                client.Login(value.Uid, value.Password);
                var nodes = client.GetNodes();
                INode root = nodes.Single(n => n.Type == NodeType.Root);
                INode myFile = client.UploadFile(file, root);

                Uri downloadUrl = client.GetDownloadLink(myFile);
                client.Logout();
                message = downloadUrl.ToString();
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

            //var nodes = client.GetNodes();

            //INode root = nodes.Single(n => n.Type == NodeType.Root);
            //INode myFolder = client.CreateFolder("Upload", root);

            //INode myFile = client.UploadFile("MyFile.ext", myFolder);

            //Uri downloadUrl = client.GetDownloadLink(myFile);
            //Console.WriteLine(downloadUrl);
        }
    }
}