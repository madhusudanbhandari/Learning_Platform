using System.Diagnostics;
using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository,ICourseRepository courseRepository,IMapper mapper)
    {
        _enrollmentRepository=enrollmentRepository;
        _courseRepository=courseRepository;
        _mapper=mapper;
    }

    public async Task<ViewEnrollRequestDto> RequestEnrollment(int studentId, EnrollRequestDto dto)
    {
        var myRequests=await _enrollmentRepository.GetStudentRequests(studentId);

        foreach(var request in myRequests)
        {
            if (request.CourseId == dto.CourseId)
            {
                throw new Exception("You have already requested for this course");
            }
        }

        var enroll=new Enrollment
        {
            StudentId=studentId,
            CourseId=dto.CourseId,
            AmountPaid=dto.AmountPaid,
            CreatedAt=DateTime.UtcNow,
            Status=Domain.Entities.Enum.EnrollmentStatus.Pending
        };

        await _enrollmentRepository.AddEnrollment(enroll);
        await _enrollmentRepository.SaveChangesAsync();

        return _mapper.Map<ViewEnrollRequestDto>(enroll);
    }


    public async Task<List<ViewEnrollRequestDto>> ViewEnrollRequests(int teacherId,int courseId)
    {
        var course=await _courseRepository.ViewCourseById(courseId);

        if (course == null)
        {
            throw new NotFoundException("Cannot find the course");
        }

        if (course.CreatedBy != teacherId)
        {
            throw new InvalidOperationException("You donot own this course");
        }
        
        var requests=await _enrollmentRepository.GetEnrollmentRequestByCourse(courseId);

        return _mapper.Map<List<ViewEnrollRequestDto>>(requests);
    }

    public async Task<ViewEnrollRequestDto> VerifyRequest(int teacherId,int enrollId,UpdateRequestDto dto)
    {
        var request=await _enrollmentRepository.GetEnrollmentRequestById(enrollId);

        if (request == null)
        {
            throw new NotFoundException("Cannot find the request");
        }

        var course=await _courseRepository.ViewCourseById(request.CourseId);

        if (course == null)
        {
            throw new NotFoundException("Cannot find the course");
        }

        if (course.CreatedBy != teacherId)
        {
            throw new InvalidOperationException("You donot own this course");
        }        

        request.Status=dto.Status;
        request.Remark=dto.Remark;

        await _enrollmentRepository.SaveChangesAsync();

        return _mapper.Map<ViewEnrollRequestDto>(request);


    }


}