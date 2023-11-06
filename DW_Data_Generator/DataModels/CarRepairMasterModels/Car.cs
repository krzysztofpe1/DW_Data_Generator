using System.Text;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Car : ModelInterface
    {
        public string? Registration { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                "registration",
                "brand",
                "model",
                "name",
                "surname");
            return sb.ToString();
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Registration, Brand, Model, Name, Surname);
            return sb.ToString();
        }
    }
}
