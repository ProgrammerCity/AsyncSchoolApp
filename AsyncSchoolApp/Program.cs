using AsyncSchoolApp.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StuDbContext>(options =>
    options.UseSqlite("Data Source=Students.db"));
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentRepository>();
var app = builder.Build();

var exportsDir = Path.Combine(app.Environment.ContentRootPath, "Exports");
if (!Directory.Exists(exportsDir))
{
    Directory.CreateDirectory(exportsDir);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Exports")),
    RequestPath = "/Exports"
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
