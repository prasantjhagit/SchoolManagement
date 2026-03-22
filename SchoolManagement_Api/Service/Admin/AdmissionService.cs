using SchoolManagement_Api.DTO;
using SchoolManagement_Api.Reposirty.Admin;

namespace SchoolManagement_Api.Service.Admin
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionRepo _repo;
        public AdmissionService(IAdmissionRepo admissionRepo)
        {
            _repo = admissionRepo;
        }
        public async Task<int> AddNewAdmission(AdmissionDto admissionDto)
        {
           return await _repo.AddNewAdmission(admissionDto);
        }
        public async Task<List<StudentModel>> GetStudents()
        {
            return await _repo.GetStudents();
        }
        public async Task<List<TodayStudentStatusDto>> GetTodayStudentStatus()
        {
            return await _repo.GetTodayStudentStatus();
        }
        public async Task<Student> GetStudentById(int id)
        {
            return await _repo.GetStudentById(id);
        }

        public Task AddDocumentAsync(DocumentUploadDto document)
        {
           return _repo.AddDocumentAsync(document);
        }
    }
}
