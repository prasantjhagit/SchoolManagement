
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Student> GetStudentBySearch(string searchText)
        {
            try
            {
                return await _db.Students
                    .FirstOrDefaultAsync(x =>
                        x.StudentId.ToString() == searchText ||
                        x.RollNumber == searchText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<TransferModelDto>> GetTransferHistoryByStudent()
        {
            try
            {
                return await _db.StudentTransfer
                    .OrderByDescending(t => t.TransferDate)
                    .Select(t => new TransferModelDto
                    {
                        TransferId = t.TransferId,
                        StudentId = t.StudentId,
                        StudentName = t.StudentName,
                        AdmissionDate = t.AdmissionDate,
                        TransferType = t.TransferType,
                        TransferDate = t.TransferDate,
                        FromClass = t.FromClass,
                        FromSection = t.FromSection,
                        ToClass = t.ToClass,
                        ToSection = t.ToSection,
                        Reason = t.TransferReason,
                        Status = t.Status,
                        CreatedAt = t.CreatedAt
                    }).ToListAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> SaveTransferAsync(TransferModelDto model)
        {
            try
            {
                var student = await _db.Students.FirstOrDefaultAsync(x => x.StudentId == model.StudentId);
                if (student == null)
                {
                    return false;
                }
                var transfer = new StudentTransfers
                {
                    StudentId = model.StudentId,
                    StudentName = student.StudentName,
                    AdmissionDate = student.AdmissionDate,
                    TransferType = model.TransferType,
                    TransferDate = model.TransferDate,
                    FromClass = student.Class,
                    FromSection = student.Section,
                    ToClass = model.ToClass,
                    ToSection = model.ToSection,
                    TransferReason = model.Reason,
                    Status = true,
                    CreatedAt = DateTime.Now
                };

                await _db.StudentTransfer.AddAsync(transfer);
                if (model.TransferType == 2)
                {
                    student.Class = model.ToClass;
                    student.Section = model.ToSection;
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


}