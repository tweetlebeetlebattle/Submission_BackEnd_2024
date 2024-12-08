namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class SeaDataIndex
    {
        public List<DataIndex> DataIndices { get; set; }
    }
    public class DataIndex
    {
        public string Location { get; set; }
        public double Index { get; set; }
    }
}
