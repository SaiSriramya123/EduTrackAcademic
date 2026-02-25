using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduTrackAcademics.Model;
using EduTrackAcademics.Data;

namespace EduTrackAcademics.AuthFolder
{
	public class Authorization : IAuthorization
	{
			private readonly EduTrackAcademicsContext  _context;
			private readonly string key = "This_is_my_first_Test_Key_for_jwt_token";

			public Authorization(EduTrackAcademicsContext context)
			{
				_context = context;
			}

			private string GenerateToken(string userId, string role)
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.ASCII.GetBytes(key);

				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(ClaimTypes.Role, role)
					}),
					Expires = DateTime.UtcNow.AddHours(1),
					SigningCredentials = new SigningCredentials(
						new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
				};

				var token = tokenHandler.CreateToken(tokenDescriptor);
				return tokenHandler.WriteToken(token);
			}

			public string AuthenticateStudent(Student student)
			{
				var studentInDb = _context.Student.FirstOrDefault(s => s.StudentEmail == student.StudentEmail);
				if (studentInDb == null) return null;

				var passwordHasher = new PasswordHasher<Student>();

				var result = passwordHasher.VerifyHashedPassword(studentInDb, studentInDb.StudentPassword, student.StudentPassword);
				if (result != PasswordVerificationResult.Success) return null;

				return GenerateToken(studentInDb.StudentId, studentInDb.Role);
			}

			public string AuthenticateInstructor(Instructor instructor)
			{
				var instructorInDb = _context.Instructor.FirstOrDefault(i => i.InstructorEmail == instructor.InstructorEmail);
				if (instructorInDb == null) return null;

				var passwordHasher = new PasswordHasher<Instructor>();

				var result = passwordHasher.VerifyHashedPassword(instructorInDb, instructorInDb.InstructorPassword, instructor.InstructorPassword);
				if (result != PasswordVerificationResult.Success) 
				   return null;

				return GenerateToken(instructorInDb.InstructorId, instructorInDb.Role);
			}

			public string AuthenticateCoordinator(Coordinator coordinator)
			{
				var coordinatorInDb = _context.Coordinator.FirstOrDefault(c => c.CoordinatorEmail == coordinator.CoordinatorEmail);
				if (coordinatorInDb == null) 
				   return null;

				var passwordHasher = new PasswordHasher<Coordinator>();

				var result = passwordHasher.VerifyHashedPassword(coordinatorInDb, coordinatorInDb.CoordinatorPassword, coordinator.CoordinatorPassword);
				if (result != PasswordVerificationResult.Success) 
				   return null;

				return GenerateToken(coordinatorInDb.CoordinatorId, coordinatorInDb.Role);
			}
		public bool Logout(string userId)
		{ 
		  return true; 
		}
	}
}


