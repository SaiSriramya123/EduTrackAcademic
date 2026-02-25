using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Repository;
using EduTrackAcademics.Service;
using EduTrackAcademics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EduTrackAcademics.AuthFolder;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Database
// =======================
builder.Services.AddDbContext<EduTrackAcademicsContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("EduTrackAcademicsContext")
		?? throw new InvalidOperationException("Connection string not found")
	));

// =======================
// Controllers & Swagger
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// Dependency Injection
// =======================
builder.Services.AddScoped<ICoordinatorDashboardRepo, CoordinatorDashboardRepo>();
builder.Services.AddScoped<ICoordinatorDashboardService, CoordinatorDashboardService > ();


builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
builder.Services.AddSingleton<DummyInstructorData>();

builder.Services.AddSingleton<DummyInstructor>();
builder.Services.AddSingleton<DummyStudent>();
builder.Services.AddSingleton<DummyInstructorReg>();

builder.Services.AddScoped<IRegistrationRepo, RegistrationRepo>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IdService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<EmailService>();


builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<IPerformanceService, PerformanceService>();

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddScoped<IStudentProgressesRepository, StudentProgressesRepository>();
builder.Services.AddScoped<IStudentProgressesService, StudentProgressesService>();
builder.Services.AddSingleton<DummyEnrollment>();

//create builder classes for StudentProfile
builder.Services.AddScoped<IStudentDashboardRepo, StudentDashboardRepo>();
builder.Services.AddScoped<IStudentProfileRepo, StudentProfileRepo>();
builder.Services.AddScoped<IAuthorization, Authorization>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) .
	AddJwtBearer(options => { 
	options.TokenValidationParameters = new TokenValidationParameters 
	  { 
		ValidateIssuer = false, // set true if you want issuer
	    ValidateAudience = false, // set true if you want audience validation
		ValidateLifetime = true, 
		ValidateIssuerSigningKey = true, 
		IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes("This_is_my_first_Test_Key_for_jwt_token"))
	   }; 
	});

// =======================
// CORS
// =======================
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader());
});

var app = builder.Build();

// =======================
// Middleware
// =======================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API v1");
	c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
