using System;
using System.Text;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Part : ModelInterface
    {
        public int Id { get; set; }
        public string? Part_type { get; set; }
        public string? Producer { get; set; }
        public double? Price { get; set; }
        public double? LabourCost { get; set; }
        public double? LabourTime { get; set; }
        public DateTime? Date_order { get; set; }
        public DateTime? Date_in_stock { get; set; }
        public DateTime? Date_used { get; set; }
        public int FK_id_repair { get; set; }

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                "id",
                "part_type",
                "producer",
                "price",
                "date_order",
                "date_in_stock",
                "date_used",
                "FK_id_repair");
            return sb.ToString();
        }
        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Id.ToString(), Part_type, Producer, Price, Date_order.ToString(), Date_in_stock.ToString(), Date_used.ToString(), FK_id_repair.ToString());
            return sb.ToString();
        }
    }
}
