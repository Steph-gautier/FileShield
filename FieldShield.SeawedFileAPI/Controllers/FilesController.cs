using FieldShield.SeawedFileAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace FieldShield.SeawedFileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class FilesController : ControllerBase
    {

        private readonly ILogger<FilesController> _logger;
        private readonly HttpClient _httpClient;

        public FilesController(ILogger<FilesController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("SeawedFileAPI"); ;
        }

        [HttpPost(Name = "UploadFile")]
        [Tags("files")]
        public async Task<IActionResult> Post([Required] IFormFile file, string prefix)
        {
            string filePath = $"{prefix}/{file.FileName}";

            var response = await _httpClient.PostAsync(filePath, new MultipartFormDataContent
            {
                {new StreamContent(file.OpenReadStream()), "file", file.FileName }
            });

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Failed to create file: {file}", file.FileName);
                return StatusCode((int)response.StatusCode, "Failed to create file");
            }
        }

        [HttpDelete(Name = "DeleteFile")]
        [Tags("files")]
        public async Task<IActionResult> Delete([FromQuery] string filepath, bool recursive = true)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return BadRequest("filepath is required.");
            }

            //TODO : Verify file exists

            var response = await _httpClient.DeleteAsync($"{filepath}?recursive={recursive}");
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Failed to delete folder or file: {filepath}", filepath);
                return StatusCode((int)response.StatusCode, "Failed to delete file or folder");
            }
        }

        [HttpPut(Name ="MoveFileToFolder")]
        [Tags("files")]
        public async Task<IActionResult> MoveFileToFolder([FromQuery] string fromFilePath, [Required] string toFolderPath)
        {
            if (string.IsNullOrWhiteSpace(fromFilePath.Trim()) || string.IsNullOrWhiteSpace(toFolderPath.Trim()))
            {
                return BadRequest("Folder name and file ID are required.");
            }

            var response = await _httpClient.PutAsync($"{toFolderPath}?mv.from={fromFilePath}", null);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            _logger.LogError("Failed to move file: {fromFilePath} to folder: {toFolderPath}", fromFilePath, toFolderPath);
            return StatusCode((int)response.StatusCode, "Failed to move file");
        }

        [HttpHead(Name = "GetFileMetadata")]
        [Tags("files")]
        public async Task<IActionResult> GetMetaData([FromQuery] string filepath, bool metadata = true, long size = 250)
        {
            if (string.IsNullOrWhiteSpace(filepath.Trim()))
            {
                return BadRequest("File path is required");
            }

            var response = await _httpClient.GetAsync($"{filepath}?metadata={metadata}&limit={size}");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                return Ok(results);
            }

            _logger.LogError("Failed to download file {filepath}", filepath);
            return StatusCode((int)response.StatusCode, "Failed to download the file");
        }

        [HttpGet(Name ="DownloadFile")]
        [Tags("files")]
        public async Task<IActionResult> Download([FromQuery] string filepath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filepath.Trim()))
                {
                    return BadRequest("File path is required");
                }

                var response = await _httpClient.GetAsync(filepath);
                if (response.IsSuccessStatusCode)
                {
                    var fileContent = await response.Content.ReadAsByteArrayAsync();
                    return File(fileContent, "application/octet-stream", Path.GetFileName(filepath));
                }

                _logger.LogError("Failed to download file {filepath}", filepath);
                return StatusCode((int)response.StatusCode, "Failed to download the file");

            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong with the download {filepath} : {message}", filepath, e.Message);
                return StatusCode(500, "Internal server error while downloading file.");
            }
        }

    }
}
