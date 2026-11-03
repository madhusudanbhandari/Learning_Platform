using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IEnrollmentService
{
    Task<ViewEnrollRequestDto> RequestEnrollment(int studentId, EnrollRequestDto dto);
    Task<List<ViewEnrollRequestDto>> ViewEnrollRequests(int techerId,int courseId);
    Task<ViewEnrollRequestDto> VerifyRequest(int teacherId,int enrollId,UpdateRequestDto dto);
    
}