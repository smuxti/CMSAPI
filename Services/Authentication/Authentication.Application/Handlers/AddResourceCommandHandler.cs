using Authentication.Application.Commands;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    public class AddResourceCommandHandler : IRequestHandler<AddResourceCommand, bool>
    {
        private readonly IUserResourceRepository _repository;
        private readonly ILogger<AddResourceCommandHandler> _logger;

        public AddResourceCommandHandler(IUserResourceRepository repository, ILogger<AddResourceCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddResourceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Get the file extension
                string fileExtension = Path.GetExtension(request.File.FileName);

                string directoryName = $"M-{request.MerchantId}";
                string fileName = $"M-{request.FileType}{fileExtension}";

                string ftpMerchantDirectoryUrl = $"ftp://203.170.76.126/Merchant/{directoryName}";
                EnsureFtpDirectoryExists(ftpMerchantDirectoryUrl);

                string ftpFileUrl = $"{ftpMerchantDirectoryUrl}/{fileName}";

                //condition to chk if this url exist in userresources
                var currentFile = await _repository.GetResourceByMerchIdAndFileType(request.MerchantId,request.FileType);
                if (currentFile != null) 
                {
                    ftpFileUrl = $"{ftpMerchantDirectoryUrl}/{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}-{fileName}"; 
                    currentFile.isDeleted = true;
                    await _repository.UpdateAsync(currentFile);
                }

                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                // Step 5: Use FTP credentials
                ftpRequest.Credentials = new NetworkCredential("raastftp", "raast@2024");
                ftpRequest.EnableSsl = false;  
                //ftpRequest.UsePassive = true;  

                using (Stream requestStream = ftpRequest.GetRequestStream())
                {
                    await request.File.CopyToAsync(requestStream);
                }

                using (FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync())
                {
                    Console.WriteLine($"Upload status: {ftpResponse.StatusDescription}");
                }

                string fileUrl = $"{ftpFileUrl}";
                var resource = new UserResource
                {
                    MerchantId = request.MerchantId,
                    FileType = request.FileType,
                    URL = fileUrl
                };

                await _repository.AddAsync(resource);

                return true;
                //string folderName = $"M-{request.MerchantId}";
                //string folderPath = Path.Combine("uploads", folderName);
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}


                //string fileExtension = Path.GetExtension(request.File.FileName);

                //string fileName = $"M-{request.MerchantId}-{request.FileType}{fileExtension}";
                //string filePath = Path.Combine(folderPath, fileName);


                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await request.File.CopyToAsync(stream);
                //}

                //string fileUrl = $"/uploads/{folderName}/{fileName}";

                //var resource = new UserResource
                //{
                //    MerchantId = request.MerchantId,
                //    FileType = request.FileType,
                //    URL = fileUrl
                //};

                //await _repository.AddAsync(resource);

                //return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        private void EnsureFtpDirectoryExists(string directoryUrl)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryUrl);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("raastftp", "raast@2024");

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Directory creation status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is FtpWebResponse response && response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.WriteLine("Directory already exists.");
                }
                else
                {
                    throw;  
                }
            }
        }
    }
}
