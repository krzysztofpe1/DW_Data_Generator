namespace DW_Data_Generator.CarRepairMasterModels
{
    public interface ModelInterface
    {
        public string GenerateCsvHeader();
        public string ToCsv();
    }
}
