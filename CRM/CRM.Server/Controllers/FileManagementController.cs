using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.Client.Models;
using CRM.Operation;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRM.Server.Controllers
{
    public class FileManagementController : BaseController
    {
        [ProducesResponseType(typeof(FileManagementGetIFrameLinkResponse),200)]
        [WebApiAuthenticate]
        [HttpPost("GetIFrameUrl")]
        public async Task<IActionResult> GetIFrameUrl(FileManagementGenerateTokenRequest request)
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                #region Generate token

                var generateTokenUrl = "file/GenerateToken";

                var generateTokenResponse = await httpClient.PostAsJsonAsync(generateTokenUrl, request);

                var generateTokenResponseModel =
                    JsonConvert.DeserializeObject<FileManagementGenerateTokenResponse>
                        (await generateTokenResponse.Content.ReadAsStringAsync());
                
                #endregion

                #region GetIFrameLink

                var getIFrameLinkUrl = "file/GetIframeLink";

                FileManagementGetIFrameLinkRequest getIFrameLinkRequest = new FileManagementGetIFrameLinkRequest
                {
                    Token = generateTokenResponseModel.Token
                };

                var getIframeLinkResponse = await httpClient.PostAsJsonAsync(getIFrameLinkUrl, getIFrameLinkRequest);

                FileManagementGetIFrameLinkResponse fileManagementGetIFrameLinkResponse =
                    JsonConvert.DeserializeObject<FileManagementGetIFrameLinkResponse>(
                        await getIframeLinkResponse.Content.ReadAsStringAsync());

                #endregion
                
                return Ok(fileManagementGetIFrameLinkResponse);
            }
        }

        [HttpPost("GenerateToken")]
        [WebApiAuthenticate]
        public async Task<IActionResult> GenerateToken(FileManagementGenerateTokenRequest request)
        {
            GenerateFileUploadTokenOperation operation = new GenerateFileUploadTokenOperation();
            operation.Execute(request);
            return Ok(operation.Response);
        }


        [HttpPost("CopyPathData")]
        [WebApiAuthenticate]
        public async Task<IActionResult> CopyPathData(CopyPathModel request)
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var generateTokenUrl = "file/CopyPathData";

                var generateTokenResponse = await httpClient.PostAsJsonAsync(generateTokenUrl, request);

                var generateTokenResponseModel =
                    JsonConvert.DeserializeObject<FileManagementGenerateTokenResponse>
                        (await generateTokenResponse.Content.ReadAsStringAsync());
            }

            return Ok();
        }

        [HttpGet("DownloadAttachmentViaToken")]
        [WebApiAuthenticate]
        public async Task<IActionResult> DownloadFilesViaToken(string token)
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var tokenModel = new FileManagementGenerateTokenResponse{Token = token};
                
                var fileDownloadUrl = "file/Download";

                var fileDownloadResponse = await httpClient.PostAsJsonAsync(fileDownloadUrl, tokenModel);

                var file = await fileDownloadResponse.Content.ReadAsStreamAsync();


                return File(file, fileDownloadResponse.Content.Headers.ContentType.MediaType);

            }
        }

        [HttpPost("CreateShortcutForFiles")]
        [WebApiAuthenticate]
        public IActionResult CreateShortcutForFiles(CreateShortcutForFilesRequest request)
        {
            CreateShortcutForFilesOperation createShortcutForFilesOperation = new CreateShortcutForFilesOperation();
            createShortcutForFilesOperation.Execute(request);

            return Ok();
        }

        [HttpPost("ArchiveFiles")]
        [WebApiAuthenticate]
        public IActionResult ArchiveFiles(string token)
        {
            ArchiveFilesOperation createShortcutForFilesOperation = new ArchiveFilesOperation();
            createShortcutForFilesOperation.Execute(token);

            return Ok();
        }
    }
}