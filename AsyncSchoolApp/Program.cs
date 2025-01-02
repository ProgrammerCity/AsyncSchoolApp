using System.Threading.Channels;
using AsyncBankApp.Dtos;
using AsyncBankApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TrcDbContext>(options =>
    options.UseSqlite("Data Source=Transactions.db"));
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddSingleton(c =>
{
    var channel = Channel.CreateBounded<TransactionExporterJob>(new BoundedChannelOptions(100)
    {
        FullMode = BoundedChannelFullMode.Wait
    });
    return channel;
});

builder.Services.AddHostedService<StatementBackgroundService>();
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
