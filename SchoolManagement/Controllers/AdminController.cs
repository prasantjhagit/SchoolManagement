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

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _admissionService.GetStudents();
            return Json(students);
        }

        public IActionResult StudentListPartial()
        {

            return PartialView("_StudentList");
        }

        public IActionResult Admission()
        {
            return PartialView("_StudentAdmission");
        }

        public async Task<IActionResult> StudentAttendance()
        {
            var students = await _admissionService.GetTodayStudentStatus();

            return PartialView("_StudentAttendance", students);
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

        public async Task<IActionResult> FilterStudentList(string? search = "", string? classFilter = "", string? sectionFilter = "", string? sessionFilter = "")
        {
            try
            {
                var students = await _admissionService.GetStudents();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    students = students.Where(s =>
                        (!string.IsNullOrEmpty(s.StudentName) && s.StudentName.ToLower().Contains(search)) ||
                        (!string.IsNullOrEmpty(s.RollNumber) && s.RollNumber.ToLower().Contains(search)) ||
                        (!string.IsNullOrEmpty(s.RegistrationNumber) && s.RegistrationNumber.ToLower().Contains(search)) ||
                        (!string.IsNullOrEmpty(s.ParentPhone) && s.ParentPhone.ToLower().Contains(search))
                    ).ToList();
                }

                if (!string.IsNullOrWhiteSpace(classFilter))
                {
                    // use the actual property name from your Student model
                    students = students.Where(s => s.Class == classFilter).ToList();
                }

                if (!string.IsNullOrWhiteSpace(sectionFilter))
                {
                    students = students.Where(s => s.Section == sectionFilter).ToList();
                }

                if (!string.IsNullOrWhiteSpace(sessionFilter))
                {
                    students = students.Where(s => s.Session == sessionFilter).ToList();
                }

                return Json(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

}

