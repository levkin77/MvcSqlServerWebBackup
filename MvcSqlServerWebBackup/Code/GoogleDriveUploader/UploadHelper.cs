﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;

namespace MvcSqlServerWebBackup.GoogleDriveUploader
{
    public class UploadHelper : IUploadHelper
    {

        public String Password { set; get; }

        public DriveService Service { set; get; }

        //protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private String ClientId { set; get; }
        private String UserEmail { set; get; }
        private String FolderName { set; get; }
        private String ServiceAccountEmail { set; get; }
        private X509Certificate2 Certificate { set; get; }
        //  private const string SERVICE_ACCOUNT_EMAIL = "660481316212-aietulh54ei2eqsi1gdvl0g7s12ohf70@developer.gserviceaccount.com";
        // private const string SERVICE_ACCOUNT_PKCS12_FILE_PATH = @"C:\Users\Yuce\Documents\GitHub\StoreManagement\StoreManagement\StoreManagement.Admin\Content\Google Drive File Upload-1cecdf432860.p12";



        public void Connect(
          String clientId,
          String userEmail,
          String serviceAccountEmail,
         X509Certificate2 certificate,
          String folderName,
          String password)
        {
            ClientId = clientId;
            UserEmail = userEmail;
            FolderName = folderName;
            ServiceAccountEmail = serviceAccountEmail;
            Certificate = certificate;
            Password = password;
            ConnectToGoogleDriveService(UserEmail, FolderName);
        }


        /// <summary>
        /// Build a Drive service object authorized with the service account
        /// that acts on behalf of the given user.
        /// </summary>
        /// @param userEmail The email of the user.
        /// <returns>Drive service object.</returns>
        public DriveService BuildService(String userEmail)
        {

            //var scopes = new[]
            //    {
            //        DriveService.Scope.Drive,
            //        DriveService.Scope.DriveFile
            //    };
            ////X509Certificate2 certificate = new X509Certificate2(privateKeyRawData,
            ////    Password, X509KeyStorageFlags.Exportable);
            //ServiceAccountCredential credential = new ServiceAccountCredential(
            //    new ServiceAccountCredential.Initializer(ServiceAccountEmail)
            //    {
            //        Scopes = scopes,
            //        User = userEmail
            //    }.FromCertificate(Certificate));

            //// Create the service.
            //var service = new DriveService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            ////    ApplicationName = "Drive API Service Account Sample",
            //    ApplicationName = "StoreManagement"
            //});

            //return service;

            return Authentication.AuthenticateServiceAccount(ServiceAccountEmail, this.Certificate);
        }
        private void ConnectToGoogleDriveService(String userEmail, string folderName)
        {

            try
            {
                Service = BuildService(userEmail);

            }
            catch (Exception ex)
            {
                //Logger.Error(ex, String.Format("ConnectToGoogleDriveService error occurred: client id: {0} ", ClientId) + ex.StackTrace, userEmail, folderName);
            }


            try
            {
                String directoryId = GetParentId();
            }
            catch (Exception ex)
            {

                //Logger.Error(ex, String.Format("ConnectToGoogleDriveService creating Parent Id: {0} ", ClientId) + ex.StackTrace, userEmail, folderName);
            }

        }
        private string GetParentId()
        {
            var query = string.Format("title = '{0}' and mimeType = 'application/vnd.google-apps.folder'", FolderName);

            var files = FileHelper.GetFiles(Service, query);
            //Logger.Trace("Files Count:" + files);
            // If there isn't a directory with this name lets create one.
            if (files.Count == 0)
            {
                files.Add(this.CreateDirectory(FolderName));
                //Logger.Trace("If there isn't a directory with this name, lets create " + FolderName);
            }
            if (files.Count != 0)
            {
                string directoryId = files[0].Id;

                return directoryId;
            }
            else
            {
                //Logger.Error("CANNOT create " + FolderName);
                return "-1";
            }
        }

        /// <summary>
        /// Update an existing file's metadata and content.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to update.</param>
        /// <param name="newTitle">New title for the file.</param>
        /// <param name="newDescription">New description for the file.</param>
        /// <param name="newMimeType">New MIME type for the file.</param>
        /// <param name="newFilename">Filename of the new content to upload.</param>
        /// <param name="newRevision">Whether or not to create a new revision for this file.</param>
        /// <returns>Updated file metadata, null is returned if an API error occurred.</returns>
        public GoogleDriveFile updateFile(String fileId, String newTitle,
            String newDescription, String newMimeType, String newFilename, bool newRevision)
        {
            try
            {
                // First retrieve the file from the API.
                File file = Service.Files.Get(fileId).Execute();

                // File's new metadata.
                file.Name = newTitle;
                file.Description = newDescription;
                file.MimeType = newMimeType;

                // File's new content.
                byte[] byteArray = System.IO.File.ReadAllBytes(newFilename);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                // Send the request to the API.
                FilesResource.UpdateMediaUpload request = Service.Files.Update(file, fileId, stream, newMimeType);
                //request.NewRevision = newRevision;
                request.Upload();

                File updatedFile = request.ResponseBody;


                if (updatedFile == null)
                {
                    throw new Exception("InsertMediaUpload request.ResponseBody is null");
                }
                else
                {
                    return ConvertFileToGoogleDriveFile(file);
                }
            }
            catch (Exception e)
            {
                //Logger.Error(e, "An error occurred: " + e.StackTrace, fileId, newDescription, newFilename, newMimeType, newTitle);
                return null;
            }
        }

        private GoogleDriveFile ConvertFileToGoogleDriveFile(File file)
        {
            var gdf = new GoogleDriveFile();
            gdf.Id = file.Id;
            gdf.ThumbnailLink = file.ThumbnailLink;
            gdf.ModifiedDate = file.ModifiedTime;
            gdf.OriginalFilename = file.OriginalFilename;
            gdf.Name = file.Name;
            gdf.IconLink = file.IconLink;
            gdf.CreatedDate = file.CreatedTime;
            gdf.WebContentLink = file.WebContentLink;
            if (file.ImageMediaMetadata != null)
            {
                gdf.Height = file.ImageMediaMetadata.Height;
                gdf.Width = file.ImageMediaMetadata.Width;
            }
            


            return gdf;
        }

        /**
         * Permanently delete a file, skipping the trash.
         *
         * @param service Drive API service instance.
         * @param fileId ID of the file to delete.
         */
        public void deleteFile(String fileId)
        {
            try
            {
                Service.Files.Delete(fileId).Execute();
            }
            catch (System.IO.IOException e)
            {
                //Logger.Error(e, "Google Service.Files.Delete An error occurred: " + e.StackTrace, fileId);
            }
        }


        /// <summary>
        /// Retrieve a list of File resources.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <returns>List of File resources.</returns>
        public List<GoogleDriveFile> retrieveAllFiles()
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = Service.Files.List();

            do
            {
                try
                {
                    FileList files = request.Execute();

                    result.AddRange(files.Files);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    //Logger.Error(e,"Google Service RetrieveAllFiles An error occurred: " + e.StackTrace);
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            List<GoogleDriveFile> result2 = new List<GoogleDriveFile>();
            foreach (var file in result)
            {
                result2.Add(ConvertFileToGoogleDriveFile(file));
            }

            return result2;
        }

        /// <summary>
        /// Move a file to the trash.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to trash.</param>
        /// <returns>The updated file, null is returned if an API error occurred</returns>
        public GoogleDriveFile TrashFile(String fileId)
        {
            try
            {
                Service.Files.Delete(fileId).Execute();
                return ConvertFileToGoogleDriveFile(Service.Files.Get(fileId).Execute());
                //return ConvertFileToGoogleDriveFile(Service.Files..Execute() .Trash(fileId).Execute());
            }
            catch (Exception e)
            {
                //Logger.Error(e, "Google TrashFile An error occurred: " + e.StackTrace, fileId);
            }
            return null;
        }

        /// <summary>
        /// Insert new file.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="title">Name of the file to insert, including the extension.</param>
        /// <param name="description">Description of the file to insert.</param>
        /// <param name="parentId">Parent folder's ID.</param>
        /// <param name="mimeType">MIME type of the file to insert.</param>
        /// <param name="filename">Filename of the file to insert.</param><br>  /// <returns>Inserted file metadata, null is returned if an API error occurred.</returns>
        public GoogleDriveFile insertFile(String title, String description, String mimeType, String filename)
        {
            // File's metadata.
            File body = new File();
            body.Name = title;
            body.Description = description;
            body.MimeType = mimeType;

            // Set the parent folder.
            String directoryId = GetParentId();
            if (!String.IsNullOrEmpty(directoryId))
            {
                body.Parents = new List<string>(){ directoryId  };
                //body.Parents = new List<ParentReference>() { new ParentReference() { Id = directoryId } };
            }

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            var stream = new System.IO.MemoryStream(byteArray);
            try
            {
                FilesResource.CreateMediaUpload request = Service.Files.Create(body, stream, mimeType);
                request.Upload();

                File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);


                if (file == null)
                {
                    throw new Exception("InsertMediaUpload request.ResponseBody is null");
                }
                else
                {
                    return ConvertFileToGoogleDriveFile(file);
                }
            }
            catch (Exception e)
            {
                //Logger.Error(e, "GoogleDriveFile InsertFile An error occurred: " + e.StackTrace, title, description, mimeType, filename);
                return null;
            }
        }



        public GoogleDriveFile UpdateFile(string uploadFile, string fileId)
        {

            if (System.IO.File.Exists(uploadFile))
            {

                String directoryId = GetParentId();
                var body = new File
                {
                    Name = System.IO.Path.GetFileName(uploadFile),
                    Description = "File updated by DriveUploader for Windows",
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<string>(){directoryId}
                };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(uploadFile);
                var stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.UpdateMediaUpload request = Service.Files.Update(body, fileId, stream, GetMimeType(uploadFile));
                    request.Upload();
                    var file = request.ResponseBody;
                    if (file == null)
                    {
                        throw new Exception("InsertMediaUpload request.ResponseBody is null");
                    }
                    else
                    {
                        return ConvertFileToGoogleDriveFile(file);
                    }
                }
                catch (Exception e)
                {
                    //Logger.Error(" GoogleDriveFile UpdateFile An error occurred: " + e.Message, e);
                    return null;
                }
            }
            else
            {
                //Logger.Error(" GoogleDriveFile UpdateFile File does not exist: " + uploadFile);
                return null;
            }

        }

        public GoogleDriveFile UpdateFile(
            string uploadFile,
            string fileId,
            String description = "File updated by DriveUploader for Windows",
            byte[] byteArray = null)
        {

            if (System.IO.File.Exists(uploadFile))
            {

                String directoryId = GetParentId();
                var body = new File
                {
                    Name = System.IO.Path.GetFileName(uploadFile),
                    Description = description,
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<string>(){directoryId}
                };

                // File's content.
                // byte[] byteArray = System.IO.File.ReadAllBytes(uploadFile);
                var stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.UpdateMediaUpload request = Service.Files.Update(body, fileId, stream, GetMimeType(uploadFile));
                    request.Upload();
                    var file = request.ResponseBody;
                    if (file == null)
                    {
                        throw new Exception("InsertMediaUpload request.ResponseBody is null");
                    }
                    else
                    {
                        return ConvertFileToGoogleDriveFile(file);
                    }
                }
                catch (Exception e)
                {
                    //Logger.Error("An error occurred: " + e.Message, e);
                    return null;
                }
            }
            else
            {
                //Logger.Error("File does not exist: " + uploadFile);
                return null;
            }

        }
        //Share(Service, file.Id, "me", "anyone", "reader");
        /// <summary>
        /// Share content. Doc link: https://developers.google.com/drive/v2/reference/permissions/insert
        /// </summary>  
        private static void Share(DriveService service, string fileId, string emailAddress, string type, string role, bool? isWithLink = null)
        {
            var permission = new Permission { EmailAddress = emailAddress, Type = type, Role = role };
            if (isWithLink.HasValue)
            {
                permission.AllowFileDiscovery = isWithLink;
            }
            service.Permissions.Create(permission, fileId).Execute();
        }
        //InsertPermission(service, fileId, "me@gmail.com", "user", "owner");
        //See more at: http://www.daimto.com/google-drive-permissions/
        private static void SetFileOwner(DriveService service, string fileId, string emailAddress, string type="user", string role="owner", bool? isWithLink = null)
        {
            var permission = new Permission { EmailAddress = emailAddress, Type = type, Role = role };
            if (isWithLink.HasValue)
            {
                permission.AllowFileDiscovery = isWithLink;
            }
            service.Permissions.Create(permission, fileId).Execute();
        }
        private File CreateDirectory(String directoryTitle, String directoryDescription = "Backup of files")
        {

            File newDirectory = null;

            var body = new File
                       {
                           Name = directoryTitle,
                           Description = directoryDescription,
                           MimeType = "application/vnd.google-apps.folder",
                           Parents = new List<string>(){"root"}
                       };
            try
            {


                var request = Service.Files.Create(body);
                newDirectory = request.Execute();


                Share(Service, newDirectory.Id, "", "anyone", "reader");


            }
            catch (Exception e)
            {
                //Logger.Error("An error occurred: " + e.Message, e);
            }
            return newDirectory;
        }

        public string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var extension = System.IO.Path.GetExtension(fileName);

            if (extension == null)
            {
                return mimeType;
            }

            var ext = extension.ToLower();
            var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }

            return mimeType;
        }

        //public GoogleDriveFile UploadFile(
        //    string uploadFile,
        //    String description = "File uploaded by DriveUploader For Windows")
        //{
        //    if (System.IO.File.Exists(uploadFile))
        //    {

        //        String directoryId = GetParentId();

        //        var body = new File
        //                   {
        //                       Name = System.IO.Path.GetFileName(uploadFile),
        //                       Description = description,
        //                       MimeType = GetMimeType(uploadFile),
        //                       Parents = new List<ParentReference>()
        //                                 {
        //                                     new ParentReference()
        //                                     {
        //                                         Id = directoryId
        //                                     }
        //                                 },
        //                   };

        //        byte[] byteArray = System.IO.File.ReadAllBytes(uploadFile);
        //        var stream = new System.IO.MemoryStream(byteArray);
        //        try
        //        {
        //            //body.UserPermission.WithLink = true;
        //            //body.UserPermission.Type = "anyone";

        //            FilesResource. .InsertMediaUpload request = Service.Files.Insert(body, stream, GetMimeType(uploadFile));
        //            request.Upload();
        //            var file = request.ResponseBody;
        //            if (file == null)
        //            {
        //                throw new Exception("InsertMediaUpload request.ResponseBody is null");
        //            }
        //            else
        //            {
        //                SetFileOwner(Service, file.Id, this.UserEmail, "user", "owner");
        //                return ConvertFileToGoogleDriveFile(file);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.Error("An error occurred: " + e.Message, e);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        Logger.Error("File does not exist: " + uploadFile);
        //        return null;
        //    }

        //}
        public GoogleDriveFile InsertFile(
            string uploadFile,
            String description = "Google Drive File for anyone",
            byte[] byteArray = null, bool shared=true)
        {

            String directoryId = GetParentId();
            var body = new File
            {
                Name = System.IO.Path.GetFileName(uploadFile),
                Description = description,
                MimeType = GetMimeType(uploadFile),
                Parents = new List<string>(){directoryId}
                
            };

            var stream = new System.IO.MemoryStream(byteArray);

            try
            {

                FilesResource.CreateMediaUpload request = Service.Files.Create(body, stream, GetMimeType(uploadFile));
                request.Upload();
                var file = request.ResponseBody;


                if (file == null)
                {
                    throw new Exception("InsertMediaUpload request.ResponseBody is null");
                }
                else
                {
                    if (shared)
                    {
                        Share(Service, file.Id, "me", "anyone", "reader");
                    }
                    else
                    {
                        SetFileOwner(Service, file.Id, this.UserEmail, "user", "writer");
                        SetFileOwner(Service, file.Id, this.UserEmail, "user", "reader");
                        SetFileOwner(Service, file.Id, this.UserEmail, "user", "owner");
                    }
                    return ConvertFileToGoogleDriveFile(file);
                }
            }
            catch (Exception e)
            {
                //Logger.Error("Service.Files.Insert error occurred: " + e.StackTrace, e);
                return null;
            }






        }
    }
}
