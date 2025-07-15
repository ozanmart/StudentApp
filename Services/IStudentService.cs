using StudentApp.Models;

namespace StudentApp.Services;

public interface IStudentService
{
    public Task<List<Student>> GetAllStudentsAsync();
    public Task<Student?> GetStudentByIdAsync(int id);
    public Task<bool> CreateStudentAsync(Student student);
    public Task<bool> UpdateStudentAsync(Student student);
    public Task<bool> DeleteStudentAsync(Student student);
}