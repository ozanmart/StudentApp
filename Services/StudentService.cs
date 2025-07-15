using Microsoft.EntityFrameworkCore;
using StudentApp.Data;

namespace StudentApp.Services;
using StudentApp.Models;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;
    
    public StudentService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<bool> CreateStudentAsync(Student student)
    {
        await _context.Students.AddAsync(student); 
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        _context.Students.Update(student);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteStudentAsync(Student student)
    {
        _context.Students.Remove(student); 
        return await _context.SaveChangesAsync() > 0;
    }
}