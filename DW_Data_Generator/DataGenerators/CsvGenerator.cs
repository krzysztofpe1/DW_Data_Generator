using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DW_Data_Generator.DataGenerators
{
    public class CsvGenerator
    {
        private DataGenerator _dataGenerator;
        private string _systemDirectory = "CarRepairMaster";
        private string _excelDirectory = "CEOExcel";
        public CsvGenerator(DataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }
        public void ExportData()
        {
            CreateDirectories();
            SaveFile(_dataGenerator.Cars.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Cars.csv");
            SaveFile(_dataGenerator.Mechanics.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Mechanics.csv");
            SaveFile(_dataGenerator.Parts.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Parts.csv");
            SaveFile(_dataGenerator.Repairs.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Repairs.csv");
        }
        #region directories
        private void CreateDirectories()
        {
            CreateDirectory(_systemDirectory);
            CreateDirectory(_excelDirectory);
        }
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        #endregion
        private void SaveFile(List<ModelInterface> objects, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
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
