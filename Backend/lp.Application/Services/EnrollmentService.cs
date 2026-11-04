using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
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

    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository,
                                ICourseRepository courseRepository,
                                IPaymentRepository paymentRepository,
                                IMapper mapper)
    {
        _enrollmentRepository=enrollmentRepository;
        _courseRepository=courseRepository;
        _paymentRepository=paymentRepository;
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
        var course=await _courseRepository.ViewCourseById(dto.CourseId);

        if (course == null)
        {
            throw new Exception("Cannot find the course");
        }

        var enroll=new Enrollment
        {
            StudentId=studentId,
            CourseId=course.Id,
            CreatedAt=DateTime.UtcNow,
            Status=Domain.Entities.Enum.EnrollmentStatus.Pending,
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

        var result=requests.Select(request=>
        {
            var payment=request.Payments.OrderByDescending(p=>p.CreatedAt).FirstOrDefault();

            if (payment == null)
            {
                throw new NotFoundException("Cannot find the payment");
            }

            return new  ViewEnrollRequestDto
                {
                    Id=request.Id,
                    StudentId=request.StudentId,
                    CourseId=request.CourseId,
                    Status=request.Status,
                    AmountPaid=payment.Amount,
                    CreatedAt=request.CreatedAt,    

                    Payment=new ViewPaymentDto
                    {
                    Id=payment.Id,
                    EnrollId=request.Id,
                    Amount=payment.Amount,
                    TransactionId=payment.TransactionId,
                    CreatedAt=payment.CreatedAt,
                    PaidAt=payment.PaidAt,
                    Status=payment.Status
                    }
                    

                };   
        }).ToList();

        return result;


        //n+1 query problem
        // foreach(var request in requests)
        // {
        //     var payment=await _paymentRepository.GetLatestPaymentByEnrollId(request.Id);

        //     if (payment == null)
        //     {
        //         throw new NotFoundException("cannot find the payment");
        //     }

            // var enrollRequest=new  ViewEnrollRequestDto
            //     {
            //         Id=request.Id,
            //         StudentId=request.StudentId,
            //         CourseId=request.CourseId,
            //         Status=request.Status,
            //         AmountPaid=payment.Amount,
            //         CreatedAt=request.CreatedAt,    

            //         Payment=new ViewPaymentDto
            //         {
            //         Id=payment.Id,
            //         EnrollId=request.Id,
            //         Amount=payment.Amount,
            //         TransactionId=payment.TransactionId,
            //         CreatedAt=payment.CreatedAt,
            //         PaidAt=payment.PaidAt,
            //         Status=payment.Status
            //         }
                    

            //     };   

        //         enrollRequests.Add(enrollRequest);

        // }

        // return enrollRequests;


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

        var payment=await _paymentRepository.GetLatestPaymentByEnrollId(request.Id);

        if (payment == null)
        {
            throw new NotFoundException("Cannot find the payment");
        }

        return new ViewEnrollRequestDto
        {
            Id=request.Id,
            StudentId=request.StudentId,
            CourseId=request.CourseId,
            Status=request.Status,
            AmountPaid=payment.Amount,
            CreatedAt=request.CreatedAt,    

            Payment=new ViewPaymentDto
            {
               Id=payment.Id,
               EnrollId=request.Id,
               Amount=payment.Amount,
               TransactionId=payment.TransactionId,
               CreatedAt=payment.CreatedAt,
               PaidAt=payment.PaidAt,
               Status=payment.Status
            }
            

        };

    }
    
}