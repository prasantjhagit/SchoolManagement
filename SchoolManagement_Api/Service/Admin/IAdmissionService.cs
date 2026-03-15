using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Service.Admin
{
    public interface IAdmissionService
    {
        // Save student admission data only (no files)
        Task<int> AddNewAdmission(AdmissionDto admissionDto);

        // Optional: separate method for updating photo path
        Task UpdatePhotoPathAsync(int studentId, string photoPath);

        // Optional: separate method for adding documents
        Task AddDocumentsAsync(int studentId, List<DocumentDto> documents);
        Task<List<StudentModel>> GetStudents();
        Task<List<TodayStudentStatusDto>> GetTodayStudentStatus();
    }
}
