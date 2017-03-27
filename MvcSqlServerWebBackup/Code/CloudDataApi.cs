using System;
using System.ComponentModel;
using System.Linq;
using CG.Web.MegaApiClient;
using System.IO;
using System.Threading;


namespace MvcSqlServerWebBackup
{
    public class CloudDataApi
    {
        // MegaRestApiClient
        // https://github.com/gpailler/MegaApiClient
        public static bool TestMegaCloud(CloudDrive value, out string message)
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

        public static bool UploadToMegaCloud(CloudDrive value, string file, out string message)
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

        //http://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c
        //http://stackoverflow.com/questions/28679082/null-exception-running-async-method-synchronously-with-ms-asynchelper
        public static bool TestMailRuCloud(CloudDrive value, out string message)
        {
            bool result = false;

            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    MailRuCloudApi.Account account = new MailRuCloudApi.Account(value.Uid, value.Password);
                    result = account.Login();

                }).GetAwaiter().GetResult();
                //var api = new MailRuCloudApi.MailRuCloud();
                //api.Account = new MailRuCloudApi.Account(value.Uid, value.Password);
                //api.Account.Login();
                //MailRuCloudApi.Account account = new MailRuCloudApi.Account(value.Uid, value.Password);
                //result = account.Login();
                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        
        public static bool UploadToMailRuCloud(CloudDrive value, string file, out string message)
        {
            bool result = false;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    var api = new MailRuCloudApi.MailRuCloud(value.Uid, value.Password);
                    result = api.Account.Login();
                    if (result)
                    {
                        result = api.UploadFile(new FileInfo(file), "/").Result;
                        Console.WriteLine(result);
                    }

                }).GetAwaiter().GetResult();
                //var api = new MailRuCloudApi.MailRuCloud();
                //api.Account = new MailRuCloudApi.Account(value.Uid, value.Password);

                //var percent = 0;
                //api.ChangingProgressEvent += delegate (object sender, ProgressChangedEventArgs e)
                //{
                //    percent = e.ProgressPercentage;
                //};
                //var task = api.UploadFile(new FileInfo(file), "/");
                //if (task.Result)
                //{
                //    if (percent == 100)
                //    {
                //        message = string.Empty;
                //        return true;
                //        //Thread.Sleep(5000);
                //    }
                //    else
                //    {
                //        message = string.Empty;
                //        return false;
                //    }
                //}
                //else
                //{
                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        //https://github.com/nirinchev/pCloud.NET
        public static bool TestPCloud(CloudDrive value, out string message)
        {
            bool result = false;
            string errMessage = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        var client = pCloud.NET.pCloudClient.CreateClientAsync(value.Uid, value.Password).Result;
                        result = true;
                        errMessage = string.Empty;
                    }
                    catch (Exception e)
                    {
                        result = false;
                        errMessage = e.Message;
                    }
                }).GetAwaiter().GetResult();

                message = errMessage;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        public static bool UploadToPCloud(CloudDrive value, string file, out string message)
        {
            bool result = false;
            string uploadLink = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    var client = pCloud.NET.pCloudClient.CreateClientAsync(value.Uid, value.Password).Result;
                    CancellationToken cancellationToken = new CancellationToken();
                    var fileStream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);
                    string uploadFileName = System.IO.Path.GetFileName(file);
                    var folder = client.ListFolderAsync(0).Result;
                    var res = client.UploadFileAsync(fileStream, 0, uploadFileName, cancellationToken).Result;
                    result = true;

                }).GetAwaiter().GetResult();

                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        //https://github.com/MediaFire/mediafire-csharp-open-sdk
        public static bool TestMediaFireCloud(CloudDrive value, out string message)
        {
            bool result = false;
            string errMessage = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        var client = pCloud.NET.pCloudClient.CreateClientAsync(value.Uid, value.Password).Result;
                        result = true;
                        errMessage = string.Empty;
                    }
                    catch (Exception e)
                    {
                        result = false;
                        errMessage = e.Message;
                    }
                }).GetAwaiter().GetResult();

                message = errMessage;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        public static bool UploadToMediaFireCloud(CloudDrive value, string file, out string message)
        {
            bool result = false;
            string uploadLink = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    var client = pCloud.NET.pCloudClient.CreateClientAsync(value.Uid, value.Password).Result;
                    CancellationToken cancellationToken = new CancellationToken();
                    var fileStream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);
                    string uploadFileName = System.IO.Path.GetFileName(file);
                    var folder = client.ListFolderAsync(0).Result;
                    var res = client.UploadFileAsync(fileStream, 0, uploadFileName, cancellationToken).Result;
                    result = true;

                }).GetAwaiter().GetResult();

                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        //https://github.com/coryrwest/B2.NET
        public static bool TestB2Cloud(CloudDrive value, out string message)
        {
            bool result = false;
            string errMessage = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        // the B2Client will default to the bucketId provided here
                        // for all subsequent calls if you set PersistBucket to true.
                        var options = new B2Net.Models.B2Options()
                        {
                            AccountId = value.Uid,
                            ApplicationKey = value.Password
                            //,BucketId = "OPTIONAL BUCKET ID",
                            //PersistBucket = true / false
                        };
                        
                        var client = new B2Net.B2Client(options);
                        // the returned options object will contain the authorizationToken
                        // necessary for subsequent calls to the B2 API.
                        var canConnect = client.Authorize().Result;

                        //canConnect.
                        //List Buckets
                        //var client = new B2Client(options);
                        //options = client.Authorize().Result;
                        var bucketList = client.Buckets.GetList().Result;
                        var uploadBasked = bucketList.FirstOrDefault(s => s.BucketName.ToUpper() == value.Location.ToUpper());
                        if (uploadBasked == null)
                        {
                            result = false;
                            errMessage = "Не найдена корзина на обласном сервисе!";
                        }
                        else
                        {
                            result = true;
                            errMessage = string.Empty;
                        }
                        
                    }
                    catch (Exception e)
                    {
                        result = false;
                        errMessage = e.Message;
                    }
                }).GetAwaiter().GetResult();

                message = errMessage;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        public static bool UploadToB2Cloud(CloudDrive value, string file, out string message)
        {
            bool result = false;
            string uploadLink = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    // the B2Client will default to the bucketId provided here
                    // for all subsequent calls if you set PersistBucket to true.
                    var options = new B2Net.Models.B2Options()
                    {
                        AccountId = value.Uid,
                        ApplicationKey = value.Password
                        //,BucketId = "OPTIONAL BUCKET ID",
                        //PersistBucket = true / false
                    };
                    var client = new B2Net.B2Client(options);
                    options = client.Authorize().Result;
                    string uploadFileName = System.IO.Path.GetFileName(file);
                    var bFileData =  File.ReadAllBytes(file);
                    var bucketList = client.Buckets.GetList().Result;
                    var uploadBasked = bucketList.FirstOrDefault(s => s.BucketName.ToUpper() == value.Location.ToUpper());
                    try
                    {
                        var fileResult = client.Files.Upload(bFileData, uploadFileName, uploadBasked.BucketId).Result;
                        Console.WriteLine(fileResult.FileId);
                    }
                    catch (Exception uploadErr)
                    {
                        Console.WriteLine(uploadErr);
                        throw;
                    }
                    
                    
                    // { FileId: "",
                    //   FileName: "",
                    //   ContentLength: "", 
                    //   ContentSHA1: "", 
                    //   ContentType: "",
                    //   FileInfo: Dictionary<string,string> }

                    //var client = pCloud.NET.pCloudClient.CreateClientAsync(value.Uid, value.Password).Result;
                    //CancellationToken cancellationToken = new CancellationToken();
                    //var fileStream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);
                    //string uploadFileName = System.IO.Path.GetFileName(file);
                    //var folder = client.ListFolderAsync(0).Result;
                    //var res = client.UploadFileAsync(fileStream, 0, uploadFileName, cancellationToken).Result;
                    result = true;

                }).GetAwaiter().GetResult();

                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        public static bool TestYandexCloud(CloudDrive value, out string message)
        {
            bool result = false;
            string errMessage = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        Yawful.Authentication.DiskAuthentication auth = new Yawful.Authentication.DiskAuthentication { ClientId = value.Uid, ClientSecret = value.Password };
                        try
                        {
                            string tokenUrl = auth.BuildTokenRequestUrl();
                            Yawful.Authentication.AccessToken token = new Yawful.Authentication.AccessToken() { TokenString = value.Token };
                            //Yawful.Authentication.AccessToken token = auth.ExchangeCode(value.Token, CancellationToken.None).Result;
                            Yawful.Client.DiskClient client = new Yawful.Client.DiskClient(token);
                            client.GetDiskMeta(CancellationToken.None);
                        }
                        catch (Exception ex)
                        {
                            result = false;
                            errMessage = ex.Message;
                        }
                        result = true;
                        errMessage = string.Empty;
                        
                    }
                    catch (Exception e)
                    {
                        result = false;
                        errMessage = e.Message;
                    }
                }).GetAwaiter().GetResult();

                message = errMessage;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        public static bool UploadToYandexCloud(CloudDrive value, string file, out string message)
        {
            bool result = false;
            string uploadLink = string.Empty;
            string taskMessage = string.Empty;
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    Yawful.Authentication.DiskAuthentication auth = new Yawful.Authentication.DiskAuthentication { ClientId = value.Uid, ClientSecret = value.Password };
                    string tokenUrl = auth.BuildTokenRequestUrl();

                    Yawful.Authentication.AccessToken token = new Yawful.Authentication.AccessToken() { TokenString = value.Token };
                    //Yawful.Authentication.AccessToken token = auth.ExchangeCode(value.Token, CancellationToken.None).Result;
                    Yawful.Client.DiskClient client = new Yawful.Client.DiskClient(token);

                    System.IO.FileInfo fInf = new FileInfo(file);
                    using (var bFileData = System.IO.File.Open(file, FileMode.Open))
                    {
                        client.UploadResource(bFileData, "/" + fInf.Name, true, null, CancellationToken.None).GetAwaiter().GetResult();
                    }
                    
                    result = true;
                    taskMessage = string.Empty;

                }).GetAwaiter().GetResult();

                message = string.Empty;
                return result;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }
    }
}