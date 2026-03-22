using SchoolManagement_Ui.DTO;
using System.Threading.Tasks;

namespace SchoolManagement_Ui.Service.Admin.Student
{
    // Interface
    public interface IAdmissionService
    {
        Task<int> AddNewAdmissionAsync(AdmissionDto admissionDto, IFormFile studentPhoto, List<IFormFile> documentFiles);
        
        Task<List<StudentModel>> GetStudents();

        Task<List<TodayStudentStatusDto>> GetTodayStudentStatus();
        Task<StudentModel> GetStudentById(int id);
        Task<List<TransferModel>> GetTransferHistoryByStudentId();
        Task<StudentModel> GetStudentBySearch(string searchKey);
        Task<bool> SaveTransferAsync(TransferModel transferModel);
    }
}