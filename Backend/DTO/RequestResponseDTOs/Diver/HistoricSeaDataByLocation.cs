namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class HistoricSeaDataByLocation
    {
        public string Location { get; set; }
        public List<HistoricSeaDataByLocationReadings> Readings { get; set; }
    }

    public class HistoricSeaDataByLocationReadings
    {
        public float? WaveMin { get; set; }
        public float? WaveMax { get; set; }
        public float? WaveAvg { get; set; }
        public int? WaveUnitId { get; set; } 

        public float? TempMin { get; set; }
        public float? TempMax { get; set; }
        public float? TempAvg { get; set; }
        public int? TempUnitId { get; set; } 

        public float? WindMin { get; set; }
        public float? WindMax { get; set; }
        public float? WindAvg { get; set; }
        public int? WindUnitId { get; set; } 

        public DateTime? DateTime { get; set; } 
    }

}