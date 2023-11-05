using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.Utils
{
    public class DWException : Exception
    {
        public DWException() { }
        public DWException(string message) : base(message) { }
    }
}
