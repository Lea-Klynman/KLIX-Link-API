using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IRepositories;
using KLIX_Link_Core.Services;
using KLIX_Link_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KLIX_Link.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFileController : ControllerBase
    {
        private readonly IUserFileService _fileService;
        public UserFileController(IUserFileService fileService)
        {
            _fileService = fileService;
        }

        // GET
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUserFilesAsync()
        {
            var files = await _fileService.GetAllUserFilesAsync();
            return Ok(files);
        }

        [HttpGet("user/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<UserFileDto[]>> GetUserFilesByUserIdAsync(int id)
        {

            if (id < 0)
                return BadRequest();
            var userFiles = await _fileService.GetUserFilesByUserIdAsync(id);
            if (userFiles == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(userFiles);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetFileByIdAsync(int id)
        {
            var file = await _fileService.GetUserFileByIdAsync(id);
            if (file == null)
                return NotFound("File not found.");

            return Ok(file);
        }

        [HttpGet("filesShared/{email}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult> GetFileshareByEmailAsync(string email)
        {
            var file = await _fileService.GetFileshareByEmail(email);
            return Ok(file);
        }

        //Post
        [HttpPost("IsFile/{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult> IsFileExistAsync(int id, [FromBody] string name)
        {
            var result = await _fileService.IsFileNameExist(id, name);
            return Ok(result);
        }

        [HttpPost("CheckingIsAllowedView/{email}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult> CheckingIsAllowedViewAsync(string email, [FromBody] SharingFileDTO sharingFileDto)
        {
            var file = await _fileService.GetUserFileByIdAsync(sharingFileDto.Id);
            if (file == null)
                return NotFound("File not found.");

            sharingFileDto.Password = file.FilePassword;
            var result = await _fileService.GetDecryptFileAsync(sharingFileDto);
            if (result == null)
                return NotFound("File not found.");

            return File(result.FileContents, result.ContentType, result.FileDownloadName);

        }


        [HttpPost("upload/{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UploadFileAsync(int id, [FromForm] UploadFileRequestDTO request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var userId = id; // לממש בהתאם
            var result = await _fileService.UploadFileAsync(request.File, request.FileName, request.Password, userId,request.FileType);
            return Ok(new { encryptedLink = result });
        }


        [HttpPost("decrypt-file")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> DecryptFileAsync([FromBody] SharingFileDTO request)
        {
            var result = await _fileService.GetDecryptFileAsync(request);
            if (result == null)
            {
                return Unauthorized("Invalid password or file not found.");
            }

            return result;

        }

        [HttpPost("encrypted/file")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> EncryptFileAsync([FromBody] SharingFileDTO request)
        {
            var result = await _fileService.GetEncryptFileAsync(request);
            if (result == null)
            {
                return Unauthorized("Invalid password or file not found.");
            }

            return result;

        }


        // PUT
        [HttpPut("Sharing/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult> SharingFileAsync(int id, [FromBody] string email)
        {
            var result = await _fileService.SharingFileAsync(id, email);
            if (result == null)
                return NotFound("File not found.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UpdateFileNameAsync(int id, [FromBody] string newFileName)
        {
            var result = await _fileService.UpdateFileNameAsync(id, newFileName);
            if (!result)
                return BadRequest("Failed to update file name.");

            return Ok(result);
        }


        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> DeleteFileAsync(int id)
        {
            var result = await _fileService.DeleteUserFileAsync(id);
            if (!result)
                return NotFound("File not found.");

            return Ok(result);
        }
    }
}
