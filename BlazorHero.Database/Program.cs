// See https://aka.ms/new-console-template for more information
using BlazorHero.Database;

var context = new DBContext();
context.Courses.Take(10)
    .ToList()
    .ForEach(x =>
    {
        global::System.Console.WriteLine(x.Title);
    });