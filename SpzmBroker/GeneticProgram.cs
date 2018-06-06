using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FDM_GA_Program
{
    // Main class of the genetic program.
    class GeneticProgram
    {
        GeneticSettings settings;
        FitnessEvaluator evaluator;

        public GeneticProgram(GeneticSettings geneticSettings)
        {
            settings = geneticSettings;
            evaluator = new FitnessEvaluator(settings);
        }
        
        // Main genetic algorithm.
        public void DoAlgorithm()
        {
            MainWindow.display.Results = "Starting Automation.";
            // Boetticher: The following line generates chromosomes that will be used in the first generation
            List<Chromosome> chromosomes = PrelimChromosomes(evaluator); 

            for (int i = 0; i < settings.Generations; i++)
            {
                // Boetticher: Right now the program makes a call to AflMaker.
                // Boetticher: Instead the should be a call to some procedure that breeds the chromosomes (Cross over and mutate)
                AflMaker.MakeAflFiles(chromosomes, settings.ProgramFolderPath, settings.TradeType);
                MainWindow.display.Results = "Backtesting Generation " + (i + 1).ToString() + ".";
                evaluator.GenerationNum = (i + 1);
                chromosomes = evaluator.EvaluateProfits(chromosomes);
                

                MainWindow.display.Results = "Completed.";
            }
        }
        
        // Continually create new random chromosomes and evaluate until all chromosomes are profitable for generation 1.
        private List<Chromosome> PrelimChromosomes(FitnessEvaluator evaluator)
        {
            List<Chromosome> finalChromosomes = new List<Chromosome>(); // New empty list, keep adding chromosomes rule by rule into list (if they are above the profit threshold).
            while (finalChromosomes.Count < settings.PopulationSize)
            {
                List<Chromosome> preChromosomes = CreateRandomChromosomes(); // Preliminary chromosomes generated randomly.
                AflMaker.MakeAflFiles(preChromosomes, settings.ProgramFolderPath, settings.TradeType);
                preChromosomes = evaluator.EvaluateProfits(preChromosomes); // Evaluator returns preChromosomes sorted by fitness.
                finalChromosomes = TestPrelimChromosomes(finalChromosomes, preChromosomes);
                
            }
            // Boetticher: At this point all preliminary processing is done
            return finalChromosomes;
        }

        // If preliminary chromosome is above profit minimum, add it to the final list of chromosomes. Break if final chromosome list is the size of the population.
        // Since chromosomes are ranked descending order prior to this, break when a chromosome is encounted with less than profit minimum.
        private List<Chromosome> TestPrelimChromosomes(List<Chromosome> finalChromosomes, List<Chromosome> preChromosomes)
        {
            foreach (Chromosome chromosome in preChromosomes)
            {
                if (finalChromosomes.Count == settings.PopulationSize || chromosome.Profit < settings.ProfitFilterMin)
                    break;
                finalChromosomes.Add(chromosome);
            }
            float perc = (finalChromosomes.Count / ((float)settings.PopulationSize)) * 100;
            MainWindow.display.Results = "Creating Initial work " + "(" + perc.ToString() + "%)";
            return finalChromosomes;
        }

        private List<Chromosome> CreateRandomChromosomes()
        {
            List<Chromosome> chromosomes = new List<Chromosome>();
            for (int i = 0; i < settings.PopulationSize; i++)
            {
                chromosomes.Add(ChromosomeFactory.CreateChromosome(settings.MaxRules));
            }
            return chromosomes;
        }
    }
}
