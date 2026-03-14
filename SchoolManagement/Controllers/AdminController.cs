using Microsoft.AspNetCore.Mvc;
using SchoolManagement_Ui.DTO;
using SchoolManagement_Ui.Service.Admin.Student;

namespace SchoolManagement_Ui.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmissionService _admissionService;

        private readonly IWebHostEnvironment _env;

        public AdminController(IAdmissionService admissionService, IWebHostEnvironment env)
        {
            _admissionService = admissionService;
            _env = env;
        }
        public IActionResult DashboardPartial()
        {
            return PartialView("_DashBoard");
        }
        public IActionResult StudentListPartial()
        {

            return PartialView("_StudentList");
        }
        public IActionResult Admission()
        {
            return PartialView("_StudentAdmission");
        }
        public IActionResult StudentAttendance()
        {

            return PartialView("_StudentAttendance");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdmission([FromForm] AdmissionDto admissionDto,IFormFile studentPhoto,List<IFormFile> documentFiles)
        {
            try
            {
                var studentId = await _admissionService.AddNewAdmissionAsync(admissionDto, studentPhoto, documentFiles);

                return Json(new { Success = true, StudentId = studentId });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }
    }

}

