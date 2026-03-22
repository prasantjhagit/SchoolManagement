using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement_Api.DTO
{
    public class AdmissionDto
    {
        public string SchoolCode { get; set; }
        public string Class { get; set; }
        public string Session { get; set; }
        public string RegistrationNumber { get; set; }
        public string RollNumber { get; set; }
        public string StudentName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Section { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string ParentPhone { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        //public string PhotoPath { get; set; }
        public string Pincode { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FullAddress { get; set; }
    }
    public class DocumentDto
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FilePath { get; set; }
    }

    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, MaxLength(20)]
        public string RegistrationNumber { get; set; }

        [Required, MaxLength(10)]
        public string SchoolCode { get; set; }

        [Required, MaxLength(5)]
        public string Class { get; set; }

        [Required, MaxLength(4)]
        public string Session { get; set; }

        [MaxLength(10)]
        public string RollNumber { get; set; }

        [Required, MaxLength(100)]
        public string StudentName { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        [MaxLength(5)]
        public string Section { get; set; }

        [MaxLength(100)]
        public string FatherName { get; set; }

        [MaxLength(100)]
        public string MotherName { get; set; }

        [MaxLength(15)]
        public string ParentPhone { get; set; }

        [MaxLength(100)]
        public string ParentsEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(100)]
        public string StudentEmail { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public StudentAddress Address { get; set; }
        public ICollection<StudentDocument> Documents { get; set; }
        public ICollection<StudentAttendance> StudentAttendances { get; set; }

    }

    public class StudentAddress
    {
        [Key]
        public int AddressId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [MaxLength(10)]
        public string Pincode { get; set; }

        [MaxLength(50)]
        public string Town { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(255)]
        public string FullAddress { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Student Student { get; set; }
    }

    public class StudentDocument
    {
        [Key]
        public int DocumentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [MaxLength(50)]
        public string DocumentType { get; set; }

        [MaxLength(50)]
        public string DocumentNumber { get; set; }

        [MaxLength(255)]
        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }

        public Student Student { get; set; }
    }
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }
        public string RegistrationNumber { get; set; }

        public string SchoolCode { get; set; }

        public string Class { get; set; }

        public string Section { get; set; }

        public string Session { get; set; }

        public string? RollNumber { get; set; }

        public string StudentName { get; set; }

        public DateTime? AdmissionDate { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public string ParentPhone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

    }

    public class TodayStudentStatusDto
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string RollNumber { get; set; }

        public int PresentCount { get; set; }

        public int AbsentCount { get; set; }

        public int LateCount { get; set; }

        public string TodayStatus { get; set; }
    }

    public class StudentAttendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Status { get; set; }

        public string MarkedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public Student Student { get; set; }
    }
    [Table("StudentTransfers")]
    public class StudentTransfers
    {
        [Key]
        public int TransferId { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int TransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public string FromClass { get; set; }
        public string FromSection { get; set; }
        public string ToClass { get; set; }
        public string ToSection { get; set; }
        public string TransferReason { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class TransferModelDto
    {
        public int TransferId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int TransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public string FromClass { get; set; }
        public string FromSection { get; set; }
        public string ToClass { get; set; }
        public string ToSection { get; set; }
        public string Reason { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
