namespace Backend.DTO
{
    public class HistoricSeaDataByLocation
    {
        public string Location { get; set; }
        public List<HistoricSeaDataByLocationReadings> Readings { get; set; }
    }

    public class HistoricSeaDataByLocationReadings
    {
        public float WaveMin { get; set; }
        public float WaveMax { get; set; }
        public float WaveAvg { get; set; }
        public DateTime DateTime { get; set; }
    }
}