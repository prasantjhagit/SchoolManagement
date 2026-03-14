using SchoolManagement_Ui.DTO;
using System.Threading.Tasks;

namespace SchoolManagement_Ui.Service.Admin.Student
{
    // Interface
    public interface IAdmissionService
    {
        Task<int> AddNewAdmissionAsync(AdmissionDto admissionDto, IFormFile studentPhoto, List<IFormFile> documentFiles);
        Task<List<AdmissionDto>> GetStudents();
    }
}