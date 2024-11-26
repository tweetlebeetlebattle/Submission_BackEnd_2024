namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class BroadSeaDataGif
    {
        public List<SpecificSeaDataGif> SeaDataList { get; set; }
    }

    public class SpecificSeaDataGif
    {
        public string Location { get; set; }
        public float? DailyWaveMax { get; set; }
        public float? DailyWaveMin { get; set; }
        public float? DailyWaveAvg { get; set; }
        public DateTime DateTime { get; set; }
    }
}
