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

}
