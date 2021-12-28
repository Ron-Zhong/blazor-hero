namespace BlazorHero.Frontend.Pages;

public partial class Courses
{
    private List<CourseModel> courses { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        courses = await Client.GetCoursesAsync();
    }
}
