using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.ComponentModel;
using System.Text.RegularExpressions;
using CornyBroker;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Globalization;

namespace FDM_GA_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Restrictions for UI controls
        public const int restrictionRuleMin = 1; // A rule should have at least 1 rule (ie. Cross(MA(Close, 10), Close)).
        public const int restrictionRuleMax = 10; // Maximum rules that can be appended together (ie. <rule1> AND <rule2> OR <rule3> ....).
        public const int restrictionPopulationMin = 3; // 3 was chosen randomly as a hypothetical minimum.
        public const int restrictionPopulationMax = 2000;
        public const int restrictionGenerationMin = 1;
        public const int restrictionStopSizeMin = 0;
        public const string Version = "SpzmBroker v1.8.0";
        public const string Version2 = "Best chromosome will be shown here";

        public static TextBlockDisplay display = new TextBlockDisplay();

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
            txtDisplay.DataContext = display; // Textblock display.

        }

        // Initialize controls using user default settings.
        private void InitializeControls() 
        {
            txtDatasetFilePicker.Text = FDM_GA_Program.Properties.Settings.Default.DatasetFilePath;
            txtFormatFilePicker.Text = FDM_GA_Program.Properties.Settings.Default.FormatFile;
            txtDatabaseFolderPicker.Text = FDM_GA_Program.Properties.Settings.Default.DatabaseFolderPath;
            txtProgramFolderPicker.Text = FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath;
            txtMaxRules.Text = FDM_GA_Program.Properties.Settings.Default.MaxRules.ToString();
            txtPopulationSize.Text = FDM_GA_Program.Properties.Settings.Default.PopulationSize.ToString();
            txtGenerations.Text = FDM_GA_Program.Properties.Settings.Default.Generations.ToString();
            txtProfitFilterMinimum.Text = FDM_GA_Program.Properties.Settings.Default.ProfitFilterMin.ToString();
            txtStopSize.Text = FDM_GA_Program.Properties.Settings.Default.Stop.ToString();
            txtPosSize.Text = FDM_GA_Program.Properties.Settings.Default.PosSize.ToString();
            
            if (FDM_GA_Program.Properties.Settings.Default.TradeType == 1)
                rbtnLong.IsChecked = true; // Else it should equal 2 for short.
        }

        // Start the genetic program. GeneticProgram class is created and runs on another thread.

        Thread newThread;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (isValidateControls())
            {
                CreateDirectories();
                ValidateAPX();
                GeneticSettings settings = new GeneticSettings();
                InitializeGeneticSettings(settings);
                // Create a new thread to handle the automation.
                GeneticProgram geneticProgram = new GeneticProgram(settings);
                 newThread = new Thread(new ThreadStart(geneticProgram.DoAlgorithm));
                newThread.Start();
            }
        }

        // Initialize GeneticSettings object which will contain all user settings needed for the genetic program.
        private void InitializeGeneticSettings(GeneticSettings settings)
        {
           
            settings.DatabasePath = FDM_GA_Program.Properties.Settings.Default.DatabaseFolderPath;
            settings.DatasetPath = FDM_GA_Program.Properties.Settings.Default.DatasetFilePath;
            settings.Format = FDM_GA_Program.Properties.Settings.Default.FormatFile;
            settings.ProgramFolderPath = FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath;
            settings.MaxRules = FDM_GA_Program.Properties.Settings.Default.MaxRules;
            settings.Generations = FDM_GA_Program.Properties.Settings.Default.Generations;
            settings.PopulationSize = FDM_GA_Program.Properties.Settings.Default.PopulationSize;
            settings.TradeType = FDM_GA_Program.Properties.Settings.Default.TradeType;
            settings.ProfitFilterMin = FDM_GA_Program.Properties.Settings.Default.ProfitFilterMin;
            settings.Stop = FDM_GA_Program.Properties.Settings.Default.Stop;
            settings.PosSize = FDM_GA_Program.Properties.Settings.Default.PosSize;
        }

        // Check existence of an APX file. Program needs one to work from or else it will copy the sample one in the SpzmBroker folder.
        private void ValidateAPX() 
        {
            string analysisDocPath = FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath + @"\AnalysisDoc\Analysis1.apx";
            if (!File.Exists(analysisDocPath))
            {
                System.IO.File.WriteAllText(analysisDocPath, FDM_GA_Program.Properties.Resources.Analysis_Sample);
            }
        }

        // Directories required for the program. These are created where program folder is.
        private void CreateDirectories()
        {
            string ProgramFolderPath = FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath;
            Directory.CreateDirectory(ProgramFolderPath + @"\AnalysisDoc");
            Directory.CreateDirectory(ProgramFolderPath + @"\Chromosomes");
            Directory.CreateDirectory(ProgramFolderPath + @"\Results");
        }

        // Return true if user input is within bounds. Save those values as default.
        private bool isValidateControls()
        {
            int maxRulesCheck = 0;
            int.TryParse(txtMaxRules.Text, out maxRulesCheck);
            int populationSizeCheck = 0;
            int.TryParse(txtPopulationSize.Text, out populationSizeCheck);
            int generationsCheck = 0;
            int.TryParse(txtGenerations.Text, out generationsCheck);
            int stopSizeCheck = 0;
            int.TryParse(txtStopSize.Text, out stopSizeCheck);
            int posSizeCheck = 0;
            int.TryParse(txtPosSize.Text, out posSizeCheck);
            double profitFilterMinCheck = 0;
            double.TryParse(txtProfitFilterMinimum.Text, out profitFilterMinCheck);
            

            if (maxRulesCheck < restrictionRuleMin || maxRulesCheck > restrictionRuleMax)
            {
                MessageBox.Show(string.Format("\"Maximum Rules\" must be between {0} and {1}", restrictionRuleMin, restrictionRuleMax));
                return false;
            }
            else if (populationSizeCheck < restrictionPopulationMin || populationSizeCheck > restrictionPopulationMax)
            {
                MessageBox.Show(string.Format("\"Population Size\" must be between {0} and {1}", restrictionPopulationMin, restrictionPopulationMax));
                return false;
            }
            else if (generationsCheck < restrictionGenerationMin)
            {
                MessageBox.Show(string.Format("\"Generations\" must be greater than {0}", restrictionGenerationMin));
                return false;
            }
            else if (stopSizeCheck < restrictionStopSizeMin)
            {
                MessageBox.Show(string.Format("\"StopSize\" must be greater than {0}",restrictionStopSizeMin));
                return false;
            }
            else if (posSizeCheck < restrictionStopSizeMin)
            {
                MessageBox.Show(string.Format("\"Position Size\" must be greater than {0}", restrictionStopSizeMin));
                return false;
            }
            else if (DtStartDate == null || DtStartDate == null)
            {
                MessageBox.Show(string.Format("\"Dates\" must be specified correctly "));
                return false;
            }
            FDM_GA_Program.Properties.Settings.Default.MaxRules = maxRulesCheck;
            FDM_GA_Program.Properties.Settings.Default.PopulationSize = populationSizeCheck;
            FDM_GA_Program.Properties.Settings.Default.Generations = generationsCheck;
            FDM_GA_Program.Properties.Settings.Default.Stop = stopSizeCheck;
            FDM_GA_Program.Properties.Settings.Default.PosSize = posSizeCheck;
            FDM_GA_Program.Properties.Settings.Default.StartDate = DtStartDate.DisplayDate.ToString("yyyy-MM-dd");
            FDM_GA_Program.Properties.Settings.Default.EndDate = DtEndDate.DisplayDate.ToString("yyyy-MM-dd");
            FDM_GA_Program.Properties.Settings.Default.ProfitFilterMin = profitFilterMinCheck;
            FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath = txtProgramFolderPicker.Text;
            FDM_GA_Program.Properties.Settings.Default.Save();
            return true;
        }

        // Textboxes that expect numbers should only allow typing numbers.
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) 
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnDatasetFilePicker_Click(object sender, RoutedEventArgs e)
        {
            string originalPath = txtDatasetFilePicker.Text;
            string newPath = Navigation.PickFile(originalPath, ".csv", " Spreadsheets | *.csv*");
            if (!newPath.Equals(originalPath))
            {
                txtDatasetFilePicker.Text = newPath;
                FDM_GA_Program.Properties.Settings.Default.DatasetFilePath = newPath;
            }
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void btnFormatFilePicker_Click(object sender, RoutedEventArgs e)
        {
            string originalPath = txtFormatFilePicker.Text;
            string newPath = Navigation.PickFileNameOnly(originalPath);
            if (!newPath.Equals(originalPath))
            {
                txtFormatFilePicker.Text = newPath;
                FDM_GA_Program.Properties.Settings.Default.FormatFile = newPath;
            }
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void btnProgramFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            string originalPath = txtProgramFolderPicker.Text;
            string newPath = Navigation.PickFolder(originalPath);
            if (!newPath.Equals(originalPath))
            {
                txtProgramFolderPicker.Text = newPath;
                FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath = newPath;
            }
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void btnDatabaseFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            string originalPath = txtDatabaseFolderPicker.Text;
            string newPath = Navigation.PickFolder(originalPath);
            if (!newPath.Equals(originalPath))
            {
                txtDatabaseFolderPicker.Text = newPath;
                FDM_GA_Program.Properties.Settings.Default.DatabaseFolderPath = newPath;
            }
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void txtDatasetFilePicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.DatasetFilePath = txtDatasetFilePicker.Text;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void txtFormatFilePicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.FormatFile = txtFormatFilePicker.Text;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void txtDatabaseFolderPicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.DatabaseFolderPath = txtDatabaseFolderPicker.Text;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void txtProgramFolderPicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.ProgramFolderPath = txtProgramFolderPicker.Text;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void rbtnLong_Checked(object sender, RoutedEventArgs e)
        {
            int tradeType = 1;
            FDM_GA_Program.Properties.Settings.Default.TradeType = tradeType;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void rbtnShort_Checked(object sender, RoutedEventArgs e)
        {
            int tradeType = 2;
            FDM_GA_Program.Properties.Settings.Default.TradeType = tradeType;
            FDM_GA_Program.Properties.Settings.Default.Save();
        }

        private void txtProfitFilterMinimum_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       

        private void xbtnStop_Click(object sender, RoutedEventArgs e)
        {
            newThread.Abort();
            MainWindow.display.Results = "Spzm Stoped..";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.kairi = true;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.PR = true;

        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.ITrend = true;
        }

        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            FDM_GA_Program.Properties.Settings.Default.KF = true;
        }

    }


    // Display strings on the textblock.
    public class TextBlockDisplay : INotifyPropertyChanged
    {
        private string _results = MainWindow.Version;
        private string _results2 = MainWindow.Version2;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public string Results
        {
            get { return this._results;}
            set
            {
                if (value != this._results)
                {
                    this._results = value;
                    OnPropertyChanged("Results");
                }
            }
        }
        public string Results2
        {
            get { return this._results2; }
            set
            {
                if (value != this._results2)
                {
                    this._results = value;
                    OnPropertyChanged("Results");
                }
            }
        }
        
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                var visible = System.Convert.ToBoolean(value, culture);
                if (InvertVisibility)
                    visible = !visible;
                return visible ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new InvalidOperationException("Converter can only convert to value of type Visibility.");
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Converter cannot convert back.");
        }

        public Boolean InvertVisibility { get; set; }

    }

}
