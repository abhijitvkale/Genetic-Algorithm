using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDM_GA_Program
{
    public class GeneticSettings
    {
        private string databasePath;
        private string datasetPath;
        private string format;
        private string programFolderPath;
        private int maxRules;
        private int populationSize;
        private int generations;
        private int tradeType;
        private double profitFilterMin;
        private int stop;
        private int posSize;

        public string DatabasePath { get { return databasePath; } set { this.databasePath = value; } }
        public string DatasetPath { get { return datasetPath; } set { this.datasetPath = value; } }
        public string Format { get { return format; } set { this.format = value; } }
        public string ProgramFolderPath { get { return programFolderPath; } set { this.programFolderPath = value; } }
        public int MaxRules { get { return maxRules; } set { this.maxRules = value; } }
        public int PopulationSize { get { return populationSize; } set { this.populationSize = value; } }
        public int Generations { get { return generations; } set { this.generations = value; } }
        public int TradeType { get { return tradeType; } set { this.tradeType = value; } }
        public double ProfitFilterMin { get { return profitFilterMin; } set { this.profitFilterMin = value; } }
        public int Stop { get { return stop; } set { this.stop = value; } }
        public int PosSize { get { return posSize; } set { this.posSize = value; } }
    }
}
