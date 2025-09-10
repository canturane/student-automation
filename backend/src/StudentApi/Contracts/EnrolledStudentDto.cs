namespace StudentApi.Contracts;

public class EnrolledStudentDto
{
    public int EnrollmentId { get; set; }   
    public int StudentId { get; set; }      
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Number { get; set; } = null!;
     public decimal? Score { get; set; }  
}
