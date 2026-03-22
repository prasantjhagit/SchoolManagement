using SchoolManagement_Ui.DTO;
using SchoolManagement_Ui.Service.Admin.Student;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AdmissionService : IAdmissionService
{
    private readonly HttpClient _httpClient;

    public AdmissionService(HttpClient httpClient)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost:5239/")
        };

        var byteArray = Encoding.ASCII.GetBytes("admin:1234");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public async Task<int> AddNewAdmissionAsync(AdmissionDto admissionDto,IFormFile studentPhoto,List<IFormFile> documentFiles)
    {
        var json = JsonSerializer.Serialize(admissionDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/AdmissionApi/CreateAdmission", content);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(result);

        var apiResponse = JsonSerializer.Deserialize<AdmissionResponseDto>(
            result,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (apiResponse == null || !apiResponse.Success)
            throw new Exception(apiResponse?.Message ?? "API failed");

        int studentId = apiResponse.StudentId;
        if (studentPhoto != null && studentPhoto.Length > 0)
        {
            var extension = Path.GetExtension(studentPhoto.FileName).ToLower();

            if (extension != ".png")
                throw new Exception("Only PNG image allowed");

            var photoFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "students",
                "photos"
            );

            if (!Directory.Exists(photoFolder))
                Directory.CreateDirectory(photoFolder);

            var fileName = $"student_{studentId}.png";
            var filePath = Path.Combine(photoFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await studentPhoto.CopyToAsync(stream);
            }

            await UploadDocumentEntryAsync(
                studentId,
                "Student Photo",
                "",
                $"/uploads/students/photos/{fileName}",
                admissionDto
            );
        }
        if (documentFiles != null && documentFiles.Count > 0)
        {
            var docFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "students",
                "documents"
            );

            if (!Directory.Exists(docFolder))
                Directory.CreateDirectory(docFolder);

            int index = 1;

            foreach (var file in documentFiles)
            {
                if (file == null || file.Length == 0)
                    continue;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".pdf" && extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                    throw new Exception("Only PDF/Image files allowed");

                var fileName = $"doc_{studentId}_{index}{extension}";
                var filePath = Path.Combine(docFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await UploadDocumentEntryAsync(
                    studentId,
                    "Aadhar Card", 
                    "",         
                    $"/uploads/students/documents/{fileName}",
                    admissionDto
                );

                index++;
            }
        }

        return studentId;
    }
    public async Task<List<StudentModel>> GetStudents()
    {
        var response = await _httpClient.GetAsync("api/AdmissionApi/GetStudents");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ApiResponse<List<StudentModel>>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.Data ?? new List<StudentModel>();
    }
    public async Task<List<TodayStudentStatusDto>> GetTodayStudentStatus()
    {
        var response = await _httpClient.GetAsync("api/AdmissionApi/GetTodayStudentStatus");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ApiResponse<List<TodayStudentStatusDto>>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.Data ?? new List<TodayStudentStatusDto>();
    }
    public async Task<StudentModel> GetStudentById(int id)
    {
        var response = await _httpClient.GetAsync($"api/AdmissionApi/GetStudentById?id={id}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ApiResponse<StudentModel>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.Data;
    }
    private async Task UploadDocumentEntryAsync(int studentId,string documentType,string documentNumber,string filePath,AdmissionDto admissionDto)
    {
        var docDto = new DocumentUploadDto
        {
            StudentId = studentId,
            DocumentType = documentType,
            DocumentNumber = documentNumber,
            FilePath = filePath,
            Class = admissionDto.Class,
            Section = admissionDto.Section
        };

        var json = JsonSerializer.Serialize(docDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/AdmissionApi/UploadDocumentEntry", content);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(result);
    }

}   