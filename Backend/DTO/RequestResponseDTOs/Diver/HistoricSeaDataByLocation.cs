namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class HistoricSeaDataByLocation
    {
        public string Location { get; set; }
        public List<HistoricSeaDataByLocationReadings> Readings { get; set; }
    }

    public class HistoricSeaDataByLocationReadings
    {
        public WaveData? WaveData { get; set; }
        public TempData? TempData { get; set; }
        public WindData? WindData { get; set; }
        public DateTime? DateTime { get; set; } 
    }
    public class WaveData
    {
        public float? WaveMin { get; set; }
        public float? WaveMax { get; set; }
        public float? WaveAvg { get; set; }
        public string? WaveUnit { get; set; }
    }
    public class TempData
    {
        public float? TempMin { get; set; }
        public float? TempMax { get; set; }
        public float? TempAvg { get; set; }
        public string? TempUnit { get; set; }
    }
    public class WindData
    {
        public float? WindMin { get; set; }
        public float? WindMax { get; set; }
        public float? WindAvg { get; set; }
        public string? WindUnit { get; set; }
    }

}