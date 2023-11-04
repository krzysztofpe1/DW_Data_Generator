using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.CEOExcelModels
{
    public class MechanicTA
    {
        public Mechanic? Mechanic { get; set; }
        public DateTime Date { get; set; }
        public double HoursAmount { get; set; }
    }
}
