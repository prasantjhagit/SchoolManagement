using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement_Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
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
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var data = await _admissionService.GetStudentById(id);

            if (data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Student not found"
                });
            }

            var result = new StudentModel
            {
                StudentId = data.StudentId,
                StudentName = data.StudentName,
                RollNumber = data.RollNumber,
                Class = data.Class,
                Section = data.Section,
                Session = data.Session,
                ParentPhone = data.ParentPhone,
                FatherName = data.FatherName,
                MotherName = data.MotherName,
                DateOfBirth = data.DateOfBirth,
                Gender = data.Gender,
                Email = data.StudentEmail
            };

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpPost("UploadDocumentEntry")]
        public async Task<IActionResult> UploadDocumentEntry([FromBody] DocumentUploadDto dto)
        {
            try
            {
                await _admissionService.AddDocumentAsync(dto);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
