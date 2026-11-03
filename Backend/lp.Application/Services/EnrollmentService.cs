using System.Diagnostics;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository=enrollmentRepository;
    }

    public async Task<ViewEnrollRequestDto> RequestEnrollment(int studentId, EnrollRequestDto dto)
    {
        var enroll=new Enrollment
        {
            StudentId=studentId,
            CourseId=dto.CourseId,
            AmountPaid=dto.AmountPaid,
            CreatedAt=DateTime.Now,
            Status=Domain.Entities.Enum.EnrollmentStatus.Pending
        };

        await _enrollmentRepository.AddEnrollment(enroll);
        await _enrollmentRepository.SaveChangesAsync();

        return new ViewEnrollRequestDto
        {
            Id=enroll.Id,
            StudentId=enroll.StudentId,
            CourseId=enroll.CourseId,
            AmountPaid=enroll.AmountPaid,
            CreatedAt=enroll.CreatedAt,
            Status=enroll.Status,
            Remark=enroll.Remark
        };
    }


    public async Task<List<ViewEnrollRequestDto>> ViewEnrollRequests(int courseId)
    {
        var requests=await _enrollmentRepository.GetEnrollmentRequestByCourse(courseId);

        return requests.Select(enroll=>new ViewEnrollRequestDto
        {
           Id=enroll.Id,
            StudentId=enroll.StudentId,
            CourseId=enroll.CourseId,
            AmountPaid=enroll.AmountPaid,
            CreatedAt=enroll.CreatedAt,
            Status=enroll.Status,
            Remark=enroll.Remark
            
        }).ToList();
    }

    public async Task<ViewEnrollRequestDto> VerifyRequest(int enrollId,UpdateRequestDto dto)
    {
        var request=await _enrollmentRepository.GetEnrollmentRequestById(enrollId);

        if (request == null)
        {
            throw new NotFoundException("Cannot find the request");
        }

        request.Status=dto.Status;
        request.Remark=dto.Remark;

        await _enrollmentRepository.SaveChangesAsync();

        return new ViewEnrollRequestDto
        {
            Id=request.Id,
            StudentId=request.StudentId,
            CourseId=request.CourseId,
            AmountPaid=request.AmountPaid,
            CreatedAt=request.CreatedAt,
            Status=request.Status,  
            Remark=request.Remark
        };


    }


}