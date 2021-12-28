var http = new HttpClient() { BaseAddress = new Uri("https://localhost:6001/") };

var client = new ApiClient(http);

var course = await client.GetCourseByIdAsync(1);

Console.WriteLine(course.Title);