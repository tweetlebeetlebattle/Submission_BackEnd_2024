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

        public override string ToString()
        {
            var setsDetails = Sets != null && Sets.Count > 0
                ? string.Join(", ", Sets.Select(s => s.ToString()))
                : "No Sets";

            return $"CreateNewTraining [Name={Name}, Date={Date}, TargetWeight={TargetWeight}, UnitName={UnitName}, TargetSets={TargetSets}, TargetReps={TargetReps}, Sets=[{setsDetails}]]";
        }
    }

    public class Set
    {
        public int Reps { get; set; }
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }

        public override string ToString()
        {
            return $"Set [Reps={Reps}, Text={Text}, Image={(Image != null ? Image.FileName : "No Image")}]";
        }
    }
}
