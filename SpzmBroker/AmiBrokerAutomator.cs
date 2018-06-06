using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
//using NLog;
//using AmiBroker.Automation;

namespace FDM_GA_Program
{
    // This class handles the AmiBroker OLE automation for backtesting models.
    public class AmiBrokerAutomator
    {
        GeneticSettings settings;
        private string analysisFolderPath;
        private string chromosomesFolderPath;
        private string resultsFolderPath;
        private string xmlFormula;
        private dynamic AB;
        private dynamic NewA;
        private string currentAnalysis;

        public AmiBrokerAutomator(GeneticSettings geneticSettings)
        {
            settings = geneticSettings;
            InitializeProgramPaths();
            InitializeOLE();
        }

        private void InitializeProgramPaths()
        {
            analysisFolderPath = settings.ProgramFolderPath + @"\AnalysisDoc\";
            chromosomesFolderPath = settings.ProgramFolderPath + @"\Chromosomes\";
            resultsFolderPath = settings.ProgramFolderPath + @"\Results\";
            currentAnalysis = analysisFolderPath + "Analysis1.apx";
        }

        // Create AmiBroker Object and load data.
        private void InitializeOLE()
        {            
            System.Type objType = System.Type.GetTypeFromProgID("Broker.Application");
            AB = System.Activator.CreateInstance(objType);
            AB.LoadDatabase(settings.DatabasePath);
            AB.Import(0, settings.DatasetPath, settings.Format);
            AB.RefreshAll();
        }

        // Backtest chromosome's corresponding AFL file. Results path is saved to chromosome.
        public void BacktestChromosome(Chromosome chromosome, int chromosomeCount)
        {
            ConvertAFLToXML(chromosome.AflPath);
            InsertXMLToAPX(chromosome.AflPath);

            NewA = AB.AnalysisDocs.Open(currentAnalysis);
            if (NewA != null)
            {
                chromosome.ResultPath = resultsFolderPath + "Chromosome" + chromosomeCount.ToString() + ".csv";
                BackTestAnalysis(chromosome);
            }
        }

        // Start individual backtest and export result as CSV file.
        private void BackTestAnalysis(Chromosome chromosome)
        {
            NewA.Run(3);
            while (NewA.IsBusy) Thread.Sleep(500); // Check IsBusy every 0.5 seconds.
            NewA.Export(chromosome.ResultPath);
            NewA.Close(); // Analysis Completed.
        }

        // Convert AFL file to an XML format compatible with the APX file.
        private void ConvertAFLToXML(string aflFilePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(aflFilePath))
                {
                    string temp = sr.ReadToEnd();
                    xmlFormula = Regex.Replace(temp, @"\r\n?|\n", "\\r\\n");
                    xmlFormula = Regex.Replace(xmlFormula, @">", "&gt;");
                    xmlFormula = Regex.Replace(xmlFormula, @"<", "&lt;");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error converting AFL to XML:");
                Console.WriteLine(e.Message);
            }
        }

        // *************************
        // Insert XML into APX file.
        // *************************
        private void InsertXMLToAPX(string currentChromosomePath)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(currentAnalysis);

                currentChromosomePath = currentChromosomePath.Replace(@"\", @"\\");
                arrLine[5] = "<FormulaPath>" + currentChromosomePath + "</FormulaPath>";
                arrLine[6] = "<FormulaContent>" + xmlFormula + "\\r\\n" + "</FormulaContent>";
                arrLine[58] = "<TradeFlags>" + settings.TradeType.ToString() + "</TradeFlags>";


                File.WriteAllLines(currentAnalysis, arrLine);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error inserting XML into APX:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
