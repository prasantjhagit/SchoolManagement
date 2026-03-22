using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Service.Admin
{
    public interface IAdmissionService
    {
        Task<int> AddNewAdmission(AdmissionDto admissionDto);
        Task<List<StudentModel>> GetStudents();
        Task<List<TodayStudentStatusDto>> GetTodayStudentStatus();
        Task<Student> GetStudentById(int id);
        Task AddDocumentAsync(DocumentUploadDto document);

    }
}
