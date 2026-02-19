using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
    public class CoordinatorService:ICoordinatorService
    {
        private readonly ICoordinatorrepo _repo;
        public CoordinatorService(ICoordinatorrepo repo) { 
            _repo = repo;
        
        }
        public List<String> GetInstructorDetails()
        {
            return _repo.GetInstructorData();
        }
    }
}
