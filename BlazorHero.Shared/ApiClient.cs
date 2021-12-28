using System.Net.Http.Json;

public class ApiClient
{
    public ApiClient(HttpClient http)
    {
        Http = http;
    }

    public HttpClient Http { get; }

    public async Task<List<CourseModel>> GetCoursesAsync()
    {
        var endpoint = $"/api/courses/";
        var courses = await Http.GetFromJsonAsync<List<CourseModel>>(endpoint);

        return courses ?? new List<CourseModel>();
    }

    public async Task<CourseModel> GetCourseByIdAsync(int id)
    {
        var endpoint = $"/api/courses/{id}";
        var courses = await Http.GetFromJsonAsync<CourseModel>(endpoint);

        return courses ?? new CourseModel();
    }
}