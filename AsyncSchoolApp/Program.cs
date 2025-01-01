using AsyncSchoolApp.Dtos;
using AsyncSchoolApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading.Channels;

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
builder.Services.AddSingleton( c =>
{
    var channel = Channel.CreateBounded<StudentExporterJob>(new BoundedChannelOptions(100)
    {
        FullMode = BoundedChannelFullMode.Wait
    });
    return channel;
});

builder.Services.AddSingleton<ConcurrentDictionary<string, JobStatus>>();
var app = builder.Build();

var exportsDir = Path.Combine(app.Environment.ContentRootPath, "Exports");
if (!Directory.Exists(exportsDir))
{
    Directory.CreateDirectory(exportsDir);
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


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
