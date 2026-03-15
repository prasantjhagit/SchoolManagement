using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement_Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolManagement_Api.DTO;
    using SchoolManagement_Api.Reposirty.Admin;
    using SchoolManagement_Api.Service.Admin;

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdmissionApiController : ControllerBase
    {
        private readonly IAdmissionService _admissionService;
       

        public AdmissionApiController(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
            
        }

        [HttpPost("CreateAdmission")]
        public async Task<IActionResult> CreateAdmission([FromBody] AdmissionDto admissionDto)
        {
            if (admissionDto == null)
                return BadRequest(new { Success = false, Message = "Admission data is required" });

            try
            {
                var studentId = await _admissionService.AddNewAdmission(admissionDto);

                if (studentId == 0)
                    return StatusCode(500, new { Success = false, Message = "Failed to save admission" });

                return Ok(new { Success = true, StudentId = studentId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(IFormFile studentPhoto, int studentId)
        {
            if (studentPhoto == null)
                return BadRequest("Photo required");

            var folder = Path.Combine("", "uploads/photos");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{studentId}_{studentPhoto.FileName}";
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await studentPhoto.CopyToAsync(stream);
            }

            return Ok(new { Success = true });
        }
        [HttpPost("UploadDocuments")]
        public async Task<IActionResult> UploadDocuments(int studentId, List<IFormFile> documentFiles, [FromForm] List<DocumentDto> documents)
        {
            if (documentFiles == null || documentFiles.Count == 0)
                return BadRequest("No documents uploaded");

            var folder = Path.Combine("", "uploads/documents");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            for (int i = 0; i < documentFiles.Count; i++)
            {
                var file = documentFiles[i];

                var fileName = $"{studentId}_{file.FileName}";
                var path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save document info to DB if needed
            }

            return Ok(new { Success = true });
        }
        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _admissionService.GetStudents();
            return Ok
                (
                new
                {
                    success = true,
                    data = students
                });
        }
        [HttpGet("GetTodayStudentStatus")]
        public async Task<IActionResult> GetTodayStudentStatus()
        {
            var result = await _admissionService.GetTodayStudentStatus();

            return Ok(new
            {
                success = true,
                data = result
            });
        }
    }
}
