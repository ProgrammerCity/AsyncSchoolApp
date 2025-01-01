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
        public async Task<IActionResult> Get()
        {
            var student = await _studentService.GetStudentList();
            var fileName = await  _studentService.SaveExcelFileAsync(student);
            var fileUrl = $"{Request.Scheme}://{Request.Host}/Exports/{fileName}";
            return Ok(new StudentListDto() { Success =true , StudentCount = student.Count , Link = fileUrl});
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeDatabase()
        {
            await _studentService.InitializeDatabase();
            return Ok("Database initialized with 10000 fake students.");
        }
    }
}
