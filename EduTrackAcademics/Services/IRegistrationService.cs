using EduTrackAcademics.Model;


namespace EduTrackAcademics.Services
{
	public interface IRegistrationService
	{
		public List<Student> StuRegister(Student stu);
		public List<Instructor> InsRegister(Instructor ins);


	}
}
