using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Repository;
using EduTrackAcademics.Service;
using EduTrackAcademics.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<EduTrackAcademicsContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("EduTrackAcademicsContext")
		?? throw new InvalidOperationException("Connection string not found")
	));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

//builder.Services.AddScoped<IStudentProgressesRepository, StudentProgressesRepository>();
//builder.Services.AddScoped<IStudentProgressesService, StudentProgressesService>();
//builder.Services.AddSingleton<DummyEnrollment>();

builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();

builder.Services.AddScoped<IAcademicReportRepository, AcademicReportRepository>();
builder.Services.AddScoped<IAcademicReportService, AcademicReportService>();



builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API v1");
	c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.MapControllers();


app.Run();
