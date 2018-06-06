using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornyBroker
{
    /*
     * Class RuleFactory is used to create Amibroker buy and sell rules out of  two Indicator objects.
     * It is currently capable of making Cross rules, like Cross(80, RSI(7))
     * Threshold rules, like RSI(7) > 80
     * but Dirvergence Rules are currently not implements since there are not Divergence Indicator classes yet defined.
     * 
     * FIX? Currently, each method of the class only returns the appropriate rule given two Indicators.  Because it
     * is so simple, it is possible that this factory can be deleted and the Rule objects called directly.
     */
    public static class RuleFactory
    {
        public static Rule createCrossRule(Indicator first, Indicator second)
        {
            return new CrossRule(first, second);

        }

        public static Rule createThresholdRule(Indicator first, Indicator second)
        {
            return new ThresholdRule(first, second);
        }

        public static Rule createDivergenceRule(Indicator first, Indicator second)
        {
            throw new NotImplementedException();
        }
    }
    /*
     * An Amibroker Rule is made up of two Indicator objects that are compared some how.
     * Each rule always contains two Indicator objects.  So, they are implmented in this
     * abstract class.
     * 
     * How the rules are compared depends on if a Cross, Threshold, or Divergence is desired.
     * Those specifics are defined in the subclasses.
     */
    public abstract class Rule
    {
        protected Indicator firstIndicator;
        protected Indicator secondIndicator;

        public Rule(Indicator first, Indicator second)
        {
            firstIndicator = first;
            secondIndicator = second;
        }

        public virtual String getABRule()
        {
            return "Abstract rule";
        }
    }
    /*
     * Cross rules are of the form Cross(firstRule, secondRule) for Amibroker.
     * If a CrossRule is created out of Indicators that are not compatable, then an exception is thrown.
     */
    class CrossRule : Rule
    {
        public CrossRule(Indicator first, Indicator second) : base(first, second)
        {
            if (!first.isComparable(second))
            {
                throw new ArgumentException("Uncomparable indicators for cross rule: " + first.getABCode() + " and " + second.getABCode());
            }
        }

        public override string getABRule()
        {
            return "Cross(" + firstIndicator.getABCode() + "," + secondIndicator.getABCode() + ")";
        }
    }
    /*
     * Threshold rules are of the form firstRule > secondRule for Amibroker.
     * If a ThresholdRule is created out of Indicators that are not compatable, then an exception is thrown.
     */
    class ThresholdRule : Rule
    {
        public ThresholdRule(Indicator first, Indicator second) : base(first, second)
        {
           
                if (!first.isComparable(second))
                {
                    throw new ArgumentException("Uncomparable indicators for threshold rule: " + first.getABCode() + " and " + second.getABCode());
                }
          

            
        }

        public override string getABRule()
        {
            return firstIndicator.getABCode() + " > " + secondIndicator.getABCode();
        }
    }
    /*
     * Divergence rules are currenlty not implemented because no Divergence Indicator classes are currently defined.
     */
    class Divergence : Rule
    {
        public Divergence(Indicator first, Indicator second) : base(first, second)
        {
            throw new NotImplementedException("Divergence rule not implemented.");
        }
    }
}
