using System.Text;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Mechanic : ModelInterface
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                "id",
                "name",
                "surname");
            return sb.ToString();
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Id.ToString(), Name, Surname);
            return sb.ToString();
        }
    }
}
