using AsyncSchoolApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsyncSchoolApp.Services
{
    public class StudentRepository
    {
        private readonly StuDbContext _context;

        public StudentRepository(StuDbContext context)
        {
            _context = context;
        }
        public async Task InitializeDatabase()
        {
            _context.Database.EnsureCreated();

            if (!await _context.Students.AnyAsync())
            {
                for (int i = 1; i <= 10000; i++)
                {
                    _context.Students.Add(new Student
                    {
                        FirstName = $"FirstName{i}",
                        LastName = $"LastName{i}",
                        Age = new Random().Next(8, 14),
                        StudentCode = new Random().Next(2010, 3030)
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetAllStudents()
        {
            //await Task.Delay(10_000);
            return await _context.Students.ToListAsync();
        }
    }
}
