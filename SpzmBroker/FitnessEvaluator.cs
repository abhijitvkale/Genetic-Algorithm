using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDM_GA_Program
{
    // This class evaluates the fitness of chromosomes(currently based on profit).
    // AmiBrokerAutomator object is created here to backtest chromosomes.
    public class FitnessEvaluator
    {
        GeneticSettings settings;
        private int generationNum;

        public int GenerationNum { set { this.generationNum = value; } }

        public FitnessEvaluator(GeneticSettings geneticSettings)
        {
            settings = geneticSettings;
        }

        // Runs chromosomes through AmiBrokerAutomator and sorts chromosomes based on profits.
        // Chromosome saving and fitness log creation is also called from here.
        public List<Chromosome> EvaluateProfits(List<Chromosome> chromosomes)
        { 
            AmiBrokerAutomator automator = new AmiBrokerAutomator(settings);
            for (int i = 0; i < chromosomes.Count; i++)
            {
                automator.BacktestChromosome(chromosomes[i], (i + 1));
                chromosomes[i].Profit = GetProfit(chromosomes[i]);
                if (chromosomes[i].Profit >= settings.ProfitFilterMin)
                    SaveChromosome(chromosomes[i]);
            }
            chromosomes.Sort((c1, c2) => c2.Profit.CompareTo(c1.Profit));
            int r =1;
            for (int i = 0; i < chromosomes.Count; i++)
            {

                chromosomes[i].Rank = r;
                r++;
            }
            MainWindow.display.Results2 = "best Rule:" + chromosomes[0].AflPath + "\n profit: " + chromosomes[0].Profit;
            PrintProfitFitnessLog(chromosomes);
            return chromosomes;
        }

        // Get profit value from the result of the analysis result.
        private double GetProfit(Chromosome chromosome)
        {
            string chromosome1String = System.IO.File.ReadAllText(chromosome.ResultPath);
            string[] manyTrades = chromosome1String.Split('\n');
            double profit = 0.0;
            
            if (manyTrades.Length > 2)
            {
                foreach (string x in manyTrades)
                {
                    try
                    {
                        string[] oneTrade = x.Split(',');
                        profit = profit + Double.Parse(oneTrade[7]);
                    }
                    catch  //Do nothing
                    {
                    }
                }
               
            }
            return profit;
        }

        // Save chromosome information to SavedChromosomes.txt.
        private void SaveChromosome(Chromosome chromosome)
        {
            string enterType;
            string exitType;
            if (settings.TradeType == 1)
            {
                enterType = "Buy Rule: ";
                exitType = "Sell Rule: ";
            }
            else
            {
                enterType = "Short Rule: ";
                exitType = "Cover Rule: ";
            }
            string chromosomeResult = "Generation: " + generationNum.ToString() + ", Profit: " + chromosome.Profit.ToString() + ", " + enterType + chromosome.EnterRule + ", " + exitType + chromosome.ExitRule + "\r\n";
            System.IO.File.AppendAllText(settings.ProgramFolderPath + @"\SavedChromosomes.txt", chromosomeResult);
        }

        // Print fitness values in terms of profit to FitnessLog.csv.
        private void PrintProfitFitnessLog(List<Chromosome> chromosomes)
        {
            string fitnessLogString = "\r\nGeneration " + (generationNum).ToString();
            if (generationNum > 0) // If not preliminary generation.
            {
                
                foreach (Chromosome c in chromosomes)
                {
                    fitnessLogString = fitnessLogString + "," + c.Profit.ToString();
                }
                System.IO.File.AppendAllText(settings.ProgramFolderPath + @"\FitnessLog.csv", fitnessLogString);
            }
        }
    }
}
