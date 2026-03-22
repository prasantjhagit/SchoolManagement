using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Reposirty.Admin
{
    public interface IAdmissionRepo
    {
        Task<int> AddNewAdmission(AdmissionDto admissionDto);
        Task<List<StudentModel>> GetStudents();
        Task<List<TodayStudentStatusDto>> GetTodayStudentStatus();
        Task<Student> GetStudentById(int studentId);
        Task AddDocumentAsync(DocumentUploadDto document);
    }
}
