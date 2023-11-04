using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DW_Data_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Ctor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion
        #region Private Methods

        #endregion
        #region GUI Interactions
        private async void PercentageTextBoxControl(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            // Combine the current text in the TextBox with the incoming input
            string currentText = box.Text;
            string newText = currentText.Substring(0, box.SelectionStart) + e.Text + currentText.Substring(box.SelectionStart + box.SelectionLength);

            // Try to parse the combined text as an integer
            if (int.TryParse(newText, out int inputValue))
            {
                // Check if the value is within the range [0, 100]
                if (inputValue < 0 || inputValue > 100)
                {
                    e.Handled = true; // Prevent the character from being entered
                }
            }
            else
            {
                e.Handled = true; // If parsing fails, prevent the character from being entered
            }
            await EstimateRecordsCount();
        }
        private async void NumericTextBoxControl(object sender, TextCompositionEventArgs e)
        {
            if (!isNumeric(e.Text))
                e.Handled = true;
            await EstimateRecordsCount();
        }
        private async void UpdateEstimatedCounts(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
                await EstimateRecordsCount();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            generate_data_button.IsEnabled = false;
            await Task.Delay(1000);
            generate_data_button.IsEnabled = true;
        }
        private bool isNumeric(string text)
        {
            if (!int.TryParse(text, out _))
                return false;
            return true;
        }
        private async Task EstimateRecordsCount()
        {
            try
            {
                await Task.Delay(500);
                var repairsPerDay = int.Parse(repairs_per_day.Text);
                var repairsPerDayOscilation = int.Parse(repairs_per_day_oscilation.Text);
                var minimumRepairsPerDay = repairsPerDay - repairsPerDayOscilation;
                var maximumRepairsPerDay = repairsPerDay + repairsPerDayOscilation;
                var startDate = DateTime.Parse(start_date.Text);
                var currentDate = DateTime.Today;

                var differenceInDays = (currentDate - startDate).TotalDays;

                var recordsLimit = int.Parse(limit_records.Text);
                if (recordsLimit == 0)
                    recordsLimit = int.MaxValue;

                var minimumRecordsCount = minimumRepairsPerDay * differenceInDays;
                var maximumRecordsCount = maximumRepairsPerDay * differenceInDays;

                if (minimumRecordsCount > recordsLimit)
                    estimated_records.Text = recordsLimit.ToString();
                else if (maximumRecordsCount > recordsLimit)
                    estimated_records.Text = minimumRecordsCount.ToString() + " - " + recordsLimit.ToString();
                else
                    estimated_records.Text = minimumRecordsCount.ToString() + " - " + maximumRecordsCount.ToString();
            }
            catch (Exception) { }
        }
        #endregion
    }
}
