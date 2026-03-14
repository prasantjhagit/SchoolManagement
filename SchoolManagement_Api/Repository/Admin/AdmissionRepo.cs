
using Microsoft.EntityFrameworkCore;
using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Reposirty.Admin
{
    public class AdmissionRepo : IAdmissionRepo
    {
        private readonly ApplicationDbContext _db;

        public AdmissionRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<int> AddNewAdmission(AdmissionDto admissionDto)
        {
            if (admissionDto == null)
                throw new ArgumentNullException(nameof(admissionDto));

            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var student = new Student
                {
                    RegistrationNumber = admissionDto.RegistrationNumber,
                    SchoolCode = admissionDto.SchoolCode,
                    Class = admissionDto.Class,
                    Session = admissionDto.Session,
                    RollNumber = admissionDto.RollNumber,
                    StudentName = admissionDto.StudentName,
                    AdmissionDate = admissionDto.AdmissionDate,
                    Section = admissionDto.Section,
                    FatherName = admissionDto.FatherName,
                    MotherName = admissionDto.MotherName,
                    ParentPhone = admissionDto.ParentPhone,
                    DateOfBirth = admissionDto.DateOfBirth,
                    Gender = admissionDto.Gender,
                    StudentEmail = admissionDto.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _db.Students.AddAsync(student);
                await _db.SaveChangesAsync();

                // Save address if provided
                if (!string.IsNullOrEmpty(admissionDto.FullAddress))
                {
                    var address = new StudentAddress
                    {
                        StudentId = student.StudentId,
                        Pincode = admissionDto.Pincode,
                        Town = admissionDto.Town,
                        City = admissionDto.City,
                        State = admissionDto.State,
                        FullAddress = admissionDto.FullAddress,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    await _db.StudentAddresses.AddAsync(address);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return student.StudentId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdatePhotoPathAsync(int studentId, string photoPath)
        {
            var student = await _db.Students.FindAsync(studentId);
            if (student != null)
            {
              //  student.PhotoPath = photoPath;
                student.UpdatedAt = DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddDocumentsAsync(int studentId, List<DocumentDto> documents)
        {
            if (documents == null || documents.Count == 0) return;

            foreach (var doc in documents)
            {
                var document = new StudentDocument
                {
                    StudentId = studentId,
                    DocumentType = doc.DocumentType,
                    DocumentNumber = doc.DocumentNumber,
                    FilePath = doc.FilePath,
                    UploadDate = DateTime.Now
                };

                await _db.StudentDocuments.AddAsync(document);
            }

            await _db.SaveChangesAsync();
        }
        public async Task<List<StudentModel>> GetStudents()
        {
            var students = await _db.Students
                .Select(s => new StudentModel
                {
                    RegistrationNumber = s.RegistrationNumber,
                    SchoolCode = s.SchoolCode,
                    Class = s.Class,
                    Session = s.Session,
                    RollNumber = s.RollNumber,
                    StudentName = s.StudentName,
                    AdmissionDate = s.AdmissionDate,
                    Section = s.Section,
                    FatherName = s.FatherName,
                    MotherName = s.MotherName,
                    ParentPhone = s.ParentPhone,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
                    Email = s.StudentEmail
                })
                .ToListAsync();

            return students;
        }
    }


}