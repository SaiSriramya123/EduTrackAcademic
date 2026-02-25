using EduTrackAcademics.Model;

namespace EduTrackAcademics.AuthFolder
{
	public interface IAuthorization
	{

			string AuthenticateStudent(Student student);
			string AuthenticateInstructor(Instructor instructor);
			string AuthenticateCoordinator(Coordinator coordinator);
		    bool Logout(string userId);

	}


}

