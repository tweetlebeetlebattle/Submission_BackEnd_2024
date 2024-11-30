using Backend.Data.Models;

namespace Backend.DTO.RequestResponseDTOs.Weightlifter
{
    public class AllUniversalLogsAndTraining
    {
        public List<PackagedUniversalReading> PackagedReadings { get; set; }
    }
    public class PackagedUniversalReading
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsTraining { get; set; }
        public List<_UniversalReading> UniversalReadingsTrainings { get; set; }

    }
    public class _UniversalReading
    {
        public string Measurment { get; set; }
        public string UnitName { get; set; }
        public string Date { get; set; }
        public bool? IsSucessTraining { get; set; }

    }
}
