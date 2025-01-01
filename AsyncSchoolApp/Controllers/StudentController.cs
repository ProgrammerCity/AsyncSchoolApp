using AsyncSchoolApp.Dtos;
using AsyncSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncSchoolApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly StudentService _studentService;
        public StudentController(ILogger<StudentController> logger, StudentService studentService)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllStudent")]
        public async Task<IActionResult> Get(int? age)
        {
            var student = await _studentService.GetStudentList(age);
            var fileId = Guid.NewGuid();
            var fileName = await  _studentService.SaveExcelFileAsync(student,fileId);
            var fileUrl = $"{Request.Scheme}://{Request.Host}/Exports/{fileName}";
            return Accepted(new StudentListDto() { Success =true , StudentCount = student.Count , Link = fileUrl});
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeDatabase()
        {
            await _studentService.InitializeDatabase();
            return Ok("Database initialized with 10000 fake students.");
        }
    }
}
