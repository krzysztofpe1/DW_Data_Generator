using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Repair
    {
        public int Id { get; set; }
        public DateTime Repair_date_start { get; set; }
        public DateTime Repair_date_end { get; set; }
        public string? FK_registration { get; set; }
        public int FK_id_part { get; set; }
        public int FK_id_mechanic { get; set; }
        public double Pricing { get; set; }
        public bool Used_car_transporter { get; set; }
        public bool Is_complaint { get; set; }
    }
}
