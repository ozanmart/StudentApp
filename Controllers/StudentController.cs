using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Data;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Controllers;

[Route("students")]
[Authorize(Roles = "Admin")]
public class StudentController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IStudentService _studentService;
    public StudentController(AppDbContext context, IEmailService emailService, IStudentService studentService)
    {
        _context = context;
        _emailService = emailService;
        _studentService = studentService;
    }
    
    // GET: /students
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return View(students);
    }

    // GET: /students/{id:int} ie. /student/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        return student == null ? NotFound() : View("Index", new List<Student> {student});
    }
    
    // GET: /students/create
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /students/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        var successful = await _studentService.CreateStudentAsync(student);
        if (!successful) 
        { 
            ModelState.AddModelError("", "Failed to create student. Please try again."); 
            return View(student);
        }
        
        // Send welcome email
        try
        {
            await _emailService.SendEmailAsync(student.Email, "Welcome to Student App",
                $"Hello {student.Name}, welcome to our student app!\npeace\nOzan");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to send email: {e.Message}");
        }
        return RedirectToAction("Index");
    }
    
    // GET: /students/edit/{id:int}
    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        
        var student = await _studentService.GetStudentByIdAsync(id.Value);
        return student == null ? NotFound() : View(student);
    }
    
    // POST: /students/edit/{id:int}
    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id) return NotFound();
        
        if (!ModelState.IsValid) return View(student);
        
        if (!await _context.Students.AnyAsync(s => s.Id == id))
            return NotFound();
        
        var success = await _studentService.UpdateStudentAsync(student);
        
        if (!success)
        {
            ModelState.AddModelError("", "Failed to update student. Please try again.");
            return View(student);
        }
        
        return RedirectToAction("Index");
    }
    
    // GET: /students/delete/{id:int}
    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        
        var student = await _studentService.GetStudentByIdAsync(id.Value);
        return student == null ? NotFound() : View(student);
    }
    
    // POST: /students/delete/{id:int}
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();

        var success = await _studentService.DeleteStudentAsync(student);
       
        if (!success)
        {
            ModelState.AddModelError("", "Failed to delete student. Please try again.");
            return View(student);
        }
        
        return RedirectToAction("Index");
    }
}