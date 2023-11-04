using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public interface ModelInterface
    {
        public string GenerateCsvHeader();
        public string ToCsv();
    }
}
