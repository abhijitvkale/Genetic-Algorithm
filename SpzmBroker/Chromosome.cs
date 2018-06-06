using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CornyBroker;

namespace FDM_GA_Program
{
    public class Chromosome
    {
        private int generation;
        private double profit;
        private string enterRule;
        private string exitRule;
        private string afl;
        private string result;
        private int rank;

        public int Generation {get { return generation; } set {this.generation = value;} }
        public double Profit { get { return profit; } set { this.profit = value; } }
        public string EnterRule { get { return enterRule; } set { this.enterRule = value; } }
        public string ExitRule { get { return exitRule; } set { this.exitRule = value; } }
        public string AflPath { get { return afl; } set { this.afl = value; } }
        public string ResultPath { get { return result; } set { this.result = value; } }
        public int Rank { get { return rank; } set { this.rank = value; } }

        public Chromosome()
        {
            generation = -1;
            profit = -1; // Should be overwritten if evaluated by profit.
            enterRule = string.Empty;
            exitRule = string.Empty;
            afl = string.Empty;
            result = string.Empty;
            rank = 0;
            
        }
    }

    // Class to create chromosomes with random enter/exit rules.
    public static class ChromosomeFactory
    {
        public static Chromosome CreateChromosome(int maxRules)
        {
            Chromosome chromosome = new Chromosome();
            chromosome.EnterRule = CompoundAFLRule(RandomRule(), maxRules);
            chromosome.ExitRule = CompoundAFLRule(RandomRule(), maxRules);
            return chromosome;
        }

        // Turn single rules from Terminal Set into (potentially)compounded rule string.
        private static string CompoundAFLRule(Rule originalRule, int maxRules)
        {
            string newRuleString = "[" + originalRule.getABRule() + "]";

            for (int i = 0; i < (maxRules); i++) // Since first rule counts as one node, generate an additional (maxRules - 1) nodes.
            {
                int rndNum = RandomHolder.Instance.Next(2);
                if (rndNum == 0)
                    newRuleString = newRuleString.AppendRule();
                else
                    newRuleString = newRuleString.InsertRule();
            }
            return newRuleString;
        }

        // Generates a random rule to append to the rule string. (e.g. A or B or C)
        private static string AppendRule(this string str) 
        {
            Rule newRule = RandomRule();
            int rndNum = RandomHolder.Instance.Next(2);

            // TODO: can currently only append rules followed by an operator. In the future we should be able to create rules like "<rule1> AND (<rule2> OR <rule3>)".
            if (rndNum == 0)
            {
                return str + " AND " + "[" + newRule.getABRule() + "]";
            }
            else
            {
                return str + " OR " + "[" + newRule.getABRule() + "]";
            }
        }

        // Generates a random rule to insert into the rule string. (e.g. (A or B) and C)
        private static string InsertRule(this string str)
        {
            Rule newRule = RandomRule();
            List<string> ruleString = Breeder.TrimRuleList(str.Split('[', ']').ToList<string>());
            int rndNum = RandomHolder.Instance.Next(ruleString.Count);
            int rndOp = RandomHolder.Instance.Next(2);
            string newSection;
            if (rndOp == 0)
                newSection = "(" + ruleString[rndNum] + " AND " + newRule.getABRule() + ")";
            else
                newSection = "(" + ruleString[rndNum] + " OR " + newRule.getABRule() + ")";
            str = str.Replace(ruleString[rndNum], newSection);
            return str;
        }

        //TODO: Should generate one of 3 random values. It only generates one of two right now since divergence isn't ready.
        private static Rule RandomRule() // Generate a random Rule of either Cross, Threshold or Divergence.
        {
            int rndNum = RandomHolder.Instance.Next(2); // TODO: This value should be 3 when Divergence is completed, because there are 3 rule types(Cross, Threshold, Divergence)
            Rule rule;
            if (rndNum == 0)
            {
                //logger.Info("creating threshold rule");
                return CreateRandomThresholdRule();
            }
            else
            {
                //logger.Info("creating cross rule");
                return CreateRandomCrossRule();
            }
            /*
            else if (rndNum == 2)
            {
                return RuleFactory.createDivergenceRule();
            }*/
        }

        private static Rule CreateRandomCrossRule() // Generate a random Cross Rule.
        {
            int rndNum = RandomHolder.Instance.Next(3);
            if (rndNum == 0) // Create a Cross( Overlay, Overlay) rule.
            {
                return RuleFactory.createCrossRule(IndicatorFactory.getRandomPriceOverlayIndicator(), IndicatorFactory.getRandomPriceOverlayIndicator());
            }
            else if (rndNum == 1) // Create a Cross( Overlay, PriceArray) rule.
            {
                return RuleFactory.createCrossRule(IndicatorFactory.getRandomPriceOverlayIndicator(), IndicatorFactory.createRandomABPriceArray());
            }
            else // Create a Cross( PriceArray, Overlay) rule.
            {
                return RuleFactory.createCrossRule(IndicatorFactory.createRandomABPriceArray(), IndicatorFactory.getRandomPriceOverlayIndicator());
            }
        }

        private static Rule CreateRandomThresholdRule() // Generate a random Threshold Rule.
        {
            int rndNum = RandomHolder.Instance.Next(6);

            if (rndNum == 0) // Create a Threshold (Overlay, Overlay) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.getRandomPriceOverlayIndicator(), IndicatorFactory.getRandomPriceOverlayIndicator());
            }
            else if (rndNum == 1) // Create a Threshold (Value, Bounded) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.createRandomValue(), IndicatorFactory.getRandomBoundedIndicator());
            }
            else if (rndNum == 2) // Create a Threshold (Value, Unbounded) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.createRandomValue(), IndicatorFactory.getRandomUnboundedIndicator());
            }
            else if (rndNum == 3) // Create a Threshold (Bounded, Bounded) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.getRandomBoundedIndicator(), IndicatorFactory.getRandomBoundedIndicator());
            }
            else if (rndNum == 4) // Create a Threshold (Bounded, Value) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.getRandomBoundedIndicator(), IndicatorFactory.createRandomValue());
            }
            else // Create a Threshold (Unbounded, Value) rule.
            {
                return RuleFactory.createThresholdRule(IndicatorFactory.getRandomUnboundedIndicator(), IndicatorFactory.createRandomValue());
            }
        }
    }
}
