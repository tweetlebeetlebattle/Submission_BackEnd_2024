namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class BroadSeaDataGSio
    {
        public List<SpecificSeaDataGSio> SeaDataList { get; set; }
    }

    public class SpecificSeaDataGSio
    {
        public string Location { get; set; }
        public float? DailyWaveMax { get; set; }
        public float? DailyWaveMin { get; set; }
        public float? DailyWaveAvg { get; set; }
        public float? DailyTempMax { get; set; }
        public float? DailyTempMin { get; set; }
        public float? DailyTempAvg { get; set; }
        public float? DailyWindMax { get; set; }
        public float? DailyWindMin { get; set; }
        public float? DailyWindAvg { get; set; }
        public DateTime DateTime { get; set; }
    }
}
