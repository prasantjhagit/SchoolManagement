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

    // 1️⃣ Save basic admission data
    public async Task<int> AddNewAdmissionAsync(
      AdmissionDto admissionDto,
      IFormFile studentPhoto,
      List<IFormFile> documentFiles)
    {
        var json = JsonSerializer.Serialize(admissionDto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/AdmissionApi/CreateAdmission", content);

        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(result);
        int studentId = 0;
        //if (studentPhoto != null)
        //    await UploadPhotoAsync(studentId, studentPhoto);

        //if (documentFiles != null && documentFiles.Count > 0)
        //    await UploadDocumentsAsync(studentId, documentFiles);

        return studentId;
    }

    private async Task UploadPhotoAsync(int studentId, IFormFile photo)
    {
        var form = new MultipartFormDataContent();

        form.Add(new StreamContent(photo.OpenReadStream()), "studentPhoto", photo.FileName);
        form.Add(new StringContent(studentId.ToString()), "studentId");

        var response = await _httpClient.PostAsync("api/Admission/UploadPhoto", form);

        response.EnsureSuccessStatusCode();
    }
    private async Task UploadDocumentsAsync(int studentId, List<IFormFile> files)
    {
        var form = new MultipartFormDataContent();

        foreach (var file in files)
        {
            form.Add(new StreamContent(file.OpenReadStream()), "documentFiles", file.FileName);
        }

        form.Add(new StringContent(studentId.ToString()), "studentId");

        var response = await _httpClient.PostAsync("api/Admission/UploadDocuments", form);

        response.EnsureSuccessStatusCode();
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

}   