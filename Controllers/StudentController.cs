using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Data;
using StudentApp.Models;

namespace StudentApp.Controllers;

[Route("students")]
[Authorize(Roles = "Admin")]
public class StudentController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEmailSender _emailSender;
    public StudentController(AppDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }
    
    // GET: /students
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.ToListAsync();
        return View(students);
    }

    // GET: /students/{id:int} ie. /student/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int? id)
    {
        var student = await _context.Students.FindAsync(id);
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
        
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        try
        {
            await _emailSender.SendEmailAsync(student.Email, "Welcome to Student App",
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
        
        var student = await _context.Students.FindAsync(id);
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
        
        _context.Update(student);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    // GET: /students/delete/{id:int}
    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        
        var student = await _context.Students.FindAsync(id);
        return student == null ? NotFound() : View(student);
    }
    
    // POST: /students/delete/{id:int}
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}