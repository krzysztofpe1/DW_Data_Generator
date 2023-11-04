using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator
{
    public class DataGenerator
    {
        public DateTime StartDate { get; set; }
        public int RepairsPerDay { get; set; }
        public int RepairsPerDayOscilation { get; set; }
        public DateTime T1 { get; set; }
        public DateTime T2 { get; set; }
        public int LimitRecords { get; set; }
        public int MechanicCount { get; set; }
        public int RegularClients { get; set; }
        public double ChanceForNewClient { get; set; }
        public DataGenerator() { }

        public void GenerateData()
        {

        }
    }
}
