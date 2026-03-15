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

        public Task AddDocumentsAsync(int studentId, List<DocumentDto> documents)
        {
            throw new NotImplementedException();
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

        public Task UpdatePhotoPathAsync(int studentId, string photoPath)
        {
            throw new NotImplementedException();
        }
    }
}
