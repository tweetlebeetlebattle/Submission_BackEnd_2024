using Backend.Data.Models;
using Backend.DTO.RequestResponseDTOs.Weightlifter;

namespace Backend.DTO.ModifiedDataDTO.Weightlifter
{
    public class CreateNewTrainingModified
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double TargetWeight { get; set; }
        public int UnitId { get; set; }
        public int TargetSets { get; set; }
        public int TargetReps { get; set; }
        public List<SetModified> Sets { get; set; }
    }
    public class SetModified
    {
        public int Reps { get; set; }
        public string? MediaId { get; set; }
    }
}
