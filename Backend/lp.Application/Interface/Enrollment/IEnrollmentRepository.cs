using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetEnrollmentRequestByCourse(int courseId);
    Task<Enrollment?> GetEnrollmentRequestById(int id);

    Task<List<Enrollment>> GetStudentRequests(int id);
    Task AddEnrollment(Enrollment enrollment);
    Task SaveChangesAsync();
}