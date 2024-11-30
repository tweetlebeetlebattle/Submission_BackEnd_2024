namespace Backend.DTO.RequestResponseDTOs.Weightlifter
{
    public class CreateNewTraining
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public double TargetWeight { get; set; }    
        public string UnitName { get; set; }
        public int TargetSets { get; set; }
        public int TargetReps { get; set; }
        public List<Set> Sets { get; set; }
    }
    public class Set
    {
        public int Reps { get; set; }
        public string? Text { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
