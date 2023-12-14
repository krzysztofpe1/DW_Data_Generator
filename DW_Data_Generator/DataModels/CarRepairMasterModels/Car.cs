using System;
using System.Text;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Car : ModelInterface
    {
        public string? Registration { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Type { get; set; }
        public int? YearOfProduction { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                "registration",
                "brand",
                "model",
                "type",
                "year_of_production",
                "name",
                "surname",
                "introduction_date",
                "expiration_date");
            return sb.ToString();
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';', Registration, Brand, Model, Type, YearOfProduction, Name, Surname, IntroductionDate.Value.ToString("dd-MM-yyyy"), (ExpirationDate != null)?ExpirationDate.Value.ToString("dd-MM-yyyy") : string.Empty);
            return sb.ToString();
        }
    }
}
