
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
        public async Task<List<StudentModel>> GetStudents()
        {
            var students = await _db.Students
                .Select(s => new StudentModel
                {
                    StudentId = s.StudentId,
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

        public async Task<List<TodayStudentStatusDto>> GetTodayStudentStatus()
        {
            try
            {
                var today = DateTime.Today;
                var monthStart = new DateTime(today.Year, today.Month, 1);

                var students = await _db.Students
                    .Select(s => new TodayStudentStatusDto
                    {
                        StudentId = s.StudentId,
                        StudentName = s.StudentName,
                        RollNumber = s.RollNumber,

                        PresentCount = _db.StudentAttendances
                            .Where(a => a.StudentId == s.StudentId
                                && a.Status == "Present"
                                && a.AttendanceDate >= monthStart)
                            .Count(),

                        AbsentCount = _db.StudentAttendances
                            .Where(a => a.StudentId == s.StudentId
                                && a.Status == "Absent"
                                && a.AttendanceDate >= monthStart)
                            .Count(),

                        LateCount = _db.StudentAttendances
                            .Where(a => a.StudentId == s.StudentId
                                && a.Status == "Late"
                                && a.AttendanceDate >= monthStart)
                            .Count(),

                        TodayStatus = _db.StudentAttendances
                            .Where(a => a.StudentId == s.StudentId
                                && a.AttendanceDate.Date == today)
                            .Select(a => a.Status)
                            .FirstOrDefault() ?? "NotMarked"
                    })
                    .ToListAsync();

                return students;
            }
            catch (Exception ex) 
            {
                return null;
            }
            finally
            {

            }
           
        }
        public async Task<Student> GetStudentById(int id)
        {
            try
            {
                return await _db.Students
                    .FirstOrDefaultAsync(x => x.StudentId == id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddDocumentAsync(DocumentUploadDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = new StudentDocument
            {
                StudentId = dto.StudentId,
                DocumentType = dto.DocumentType,
                DocumentNumber = dto.DocumentNumber,
                FilePath = dto.FilePath,
                UploadDate = DateTime.Now,
                Section=dto.Section,
                Class = dto.Class
            };

            await _db.StudentDocuments.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    }


}