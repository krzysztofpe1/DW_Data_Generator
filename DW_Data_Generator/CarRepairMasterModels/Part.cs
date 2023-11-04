using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Part
    {
        public int Id { get; set; }
        public string? Part_type { get; set; }
        public string? Producer { get; set; }
        public double? Price { get; set; }
        public DateTime? Date_order { get; set; }
        public DateTime? Date_in_stock { get; set; }
        public DateTime? Date_used { get; set; }
        public int FK_id_repair { get; set; }
    }
}
