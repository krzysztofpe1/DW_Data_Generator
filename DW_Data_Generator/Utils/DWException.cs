using System;

namespace DW_Data_Generator.Utils
{
    public class DWException : Exception
    {
        public DWException() { }
        public DWException(string message) : base(message) { }
    }
}
