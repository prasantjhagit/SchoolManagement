namespace SchoolManagement_Ui.DTO
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
        public string PhotoPath { get; set; }
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
    public class StudentModel
    {
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

    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
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

}
