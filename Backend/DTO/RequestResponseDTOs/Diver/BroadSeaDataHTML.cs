namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class BroadSeaDataHTML
    {
        public List<SpecificSeaDataHTML> SeaDataList { get; set; }
    }
    public class SpecificSeaDataHTML
    {
        public string Location { get; set; }
        public float? DailyWaveMax { get; set; }
        public float? DailyWaveMin { get; set; }
        public float? DailyWaveAvg { get; set; }
        public float? DailyTempMax { get; set; }
        public float? DailyTempMin { get; set; }
        public float? DailyTempAvg { get; set; }
        public DateTime DateTime { get; set; }
    }
}