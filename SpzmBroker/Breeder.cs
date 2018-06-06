using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDM_GA_Program
{
    // This class breeds chromosomes currently with elitism and Stochastic universal sampling selection for crossover.
    // More methods for selection could be added later.
    class Breeder
    {
        // Apply elitism selection and Stochastic universal sampling selection for crossover.
        public static List<Chromosome> BreedChromosomes(List<Chromosome> chromosomes)
        {
            List<Chromosome> newChromosomes = new List<Chromosome>();
            int elites = (chromosomes.Count + 30) / 30;
            for (int i = 0; i < elites; i++)
            {
                newChromosomes.Add(chromosomes[i]);
            }

            for (int j = elites; j < chromosomes.Count; j++)
            {
                CrossoverChromosomesSUS(j, ref chromosomes, ref newChromosomes);
            }
            return newChromosomes;
        }

        // Crossover Stochastic.Universal.Sampling. A chromosome will swap rules with any chromosome in the original list at an equally random chance.
        private static void CrossoverChromosomesSUS(int chromosomeIndex, ref List<Chromosome> chromosomes, ref List<Chromosome> newChromosomes)
        {
            Chromosome chromosome = chromosomes[chromosomeIndex];

            int rndChromosome = RandomHolder.Instance.Next(chromosomes.Count);
            int rndNumRuleFirst = RandomHolder.Instance.Next(2); // Choose either enter or exit rule of first chromosome.
            int rndNumRuleSecond = RandomHolder.Instance.Next(2); // Choose either enter or exit rule of second chromosome.

            List<string> ruleFirstSplitList = new List<string>();
            List<string> ruleSecondSplitList = new List<string>();

            if (rndNumRuleFirst == 0)
                ruleFirstSplitList = chromosome.EnterRule.Split('[', ']').ToList<string>();
            else
                ruleFirstSplitList = chromosome.ExitRule.Split('[', ']').ToList<string>();

            if (rndNumRuleSecond == 0)
                ruleSecondSplitList = chromosomes[rndChromosome].EnterRule.Split('[', ']').ToList<string>();
            else
                ruleSecondSplitList = chromosomes[rndChromosome].ExitRule.Split('[', ']').ToList<string>();

            ruleFirstSplitList = TrimRuleList(ruleFirstSplitList);
            ruleSecondSplitList = TrimRuleList(ruleSecondSplitList);

            int rndFirstSplit = RandomHolder.Instance.Next(ruleFirstSplitList.Count);
            int rndSecondSplit = RandomHolder.Instance.Next(ruleSecondSplitList.Count);

            if (rndNumRuleFirst == 0)
                chromosome.EnterRule = chromosome.EnterRule.Replace(ruleFirstSplitList[rndFirstSplit], ruleSecondSplitList[rndSecondSplit]);
            else
                chromosome.ExitRule = chromosome.ExitRule.Replace(ruleFirstSplitList[rndFirstSplit], ruleSecondSplitList[rndSecondSplit]);

            newChromosomes.Add(chromosome);
        }

        // Remove ANDS, ORS and empty strings from rules list. Only swap rules right now.
        public static List<string> TrimRuleList(List<string> ruleList)
        {
            ruleList.RemoveAll(s => s.Equals(" AND "));
            ruleList.RemoveAll(s => s.Equals(" OR "));
            ruleList.RemoveAll(s => string.IsNullOrEmpty(s));
            return ruleList;
        }
    }
}
