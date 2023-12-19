using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DW_Data_Generator.DataGenerators
{
    public class CsvGenerator
    {
        private DataGenerator _dataGenerator;
        private const string _systemDirectory = "CarRepairMaster";
        private const string _excelDirectory = "CEOExcel";
        public CsvGenerator(DataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }
        public void ExportData()
        {
            var t1 = _dataGenerator.T1;
            CreateDirectories();
            SaveFile(_dataGenerator.Cars.Cast<ModelInterface>().ToList(), "T2\\" + _systemDirectory + "\\Cars.csv");
            SaveFile(_dataGenerator.Mechanics.Cast<ModelInterface>().ToList(), "T2\\" + _systemDirectory + "\\Mechanics.csv");
            SaveFile(_dataGenerator.Parts.Cast<ModelInterface>().ToList(), "T2\\" + _systemDirectory + "\\Parts.csv");
            SaveFile(_dataGenerator.Repairs.Cast<ModelInterface>().ToList(), "T2\\" + _systemDirectory + "\\Repairs.csv");


            SaveFile(_dataGenerator.Mechanics.Cast<ModelInterface>().ToList(), "T1\\" + _systemDirectory + "\\Mechanics.csv");

            var repairs = _dataGenerator.Repairs.Where(item => item.Repair_date_start.Date <= t1.Date).ToList();
            List<string> registrations = new();

            var parts = _dataGenerator.Parts.Where(item => item.Date_order.Value.Date <= t1).ToList();
            repairs.ForEach(item =>
            {
                registrations.Add(item.FK_registration);
            });
            SaveFile(repairs.Cast<ModelInterface>().ToList(), "T1\\" + _systemDirectory + "\\Repairs.csv");
            var cars = _dataGenerator.Cars.Where(item => registrations.Contains(item.Registration));
            SaveFile(cars.Cast<ModelInterface>().ToList(), "T1\\" + _systemDirectory + "\\Cars.csv");
            SaveFile(parts.Cast<ModelInterface>().ToList(), "T1\\" + _systemDirectory + "\\Parts.csv");

            var mechanics = _dataGenerator.Mechanics;
            var mechanicTAs = _dataGenerator.MechanicTAs;
            List<int> yearsT2 = new();
            mechanicTAs.ForEach(item =>
            {
                var tempYear = item.Date.Year;
                if (!yearsT2.Contains(tempYear))
                    yearsT2.Add(tempYear);
            });
            List<int> yearsT1 = yearsT2.Where(item => item <= t1.Year).ToList();


            yearsT1.ForEach(year =>
            {
                CreateDirectory("T1\\" + _excelDirectory);
                var temp = mechanicTAs.Where(item => item.Date <= t1 && item.Date.Year == year).Cast<ModelInterface>().ToList();
                SaveFile(temp, "T1\\" + _excelDirectory + '\\' + year + ".csv");
            });
            yearsT2.ForEach(year =>
            {
                CreateDirectory("T2\\" + _excelDirectory);
                var temp = mechanicTAs.Where(item => item.Date.Year == year).Cast<ModelInterface>().ToList();
                SaveFile(mechanicTAs.Cast<ModelInterface>().ToList(), "T2\\" + _excelDirectory + '\\' + year + ".csv");
            });

        }
        #region directories
        private void CreateDirectories()
        {
            CreateDirectory("T1\\" + _systemDirectory);
            CreateDirectory("T2\\" + _systemDirectory);
            CreateDirectory("T1\\" + _excelDirectory);
            CreateDirectory("T2\\" + _excelDirectory);
        }
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        #endregion
        private void SaveFile(List<ModelInterface> objects, string path)
        {
            if (objects.Count == 0)
                return;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(objects[0].GenerateCsvHeader());
                    foreach (var obj in objects)
                    {
                        sw.WriteLine(obj.ToCsv());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Wystąpił błąd podczas Zapisywania danych: {ex.Message}"); }
        }
    }
}
