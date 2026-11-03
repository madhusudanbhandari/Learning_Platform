using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lp.Infrastructure.Repository;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly LpDbConext _context;

    public EnrollmentRepository(LpDbConext context)
    {
        _context=context;
    }

    public async Task<List<Enrollment>> GetEnrollmentRequestByCourse(int courseId)
    {
        return await _context.Enrollments
                                .Where(e=>e.Status==Domain.Entities.Enum.EnrollmentStatus.Pending && e.CourseId==courseId)
                                .ToListAsync();
    }
    public async Task<Enrollment?> GetEnrollmentRequestById(int id)
    {
        return await _context.Enrollments.FirstOrDefaultAsync(e=>e.Id==id);
    }
   
    public async Task AddEnrollment(Enrollment enrollment)
    {
         await _context.Enrollments.AddAsync(enrollment);
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }


}