using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Text;

namespace DW_Data_Generator.CEOExcelModels
{
    public class MechanicTA : ModelInterface
    {
        public Mechanic? Mechanic { get; set; }
        public DateTime Date { get; set; }
        public double HoursAmount { get; set; } = 0;

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Mechanic.Id.ToString(), Mechanic.Name, Mechanic.Surname);
            return sb.ToString();
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Date.ToString(), HoursAmount.ToString());
            return sb.ToString();
        }
    }
}
