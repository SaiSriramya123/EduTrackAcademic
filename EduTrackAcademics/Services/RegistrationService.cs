using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
	public class RegistrationService : IRegistrationService
	{
		private readonly IRegistrationRepo _repo;

		public RegistrationService(IRegistrationRepo repo)
		{
			_repo = repo;
		}

		public List<Student> StuRegister(Student student)
		{
			return _repo.StudentRegistration(student);
		}

		public List<Instructor> InsRegister(Instructor ins)
		{
			return _repo.InstructorRegistration(ins);
		}
	}
}
