
using lp.Domain.Entities.Enum;

namespace lp.Application.Dtos;

public class UpdateRequestDto
{   
    
    public EnrollmentStatus Status{get;set;}
    public string Remark{get;set;}=string.Empty;
}