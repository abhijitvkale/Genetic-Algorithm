using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CornyBroker
{
    /* Class IndicatorFactory contains methods for returning Indicator objects.  Its primary purpose it to handle the grunt work
     * of randomizing the Indicator object parameters so that the main method of the program remains clean.  It also contains
     * some useful methods for returning Indicator objects of specific subclasses (PriceOverlay, BoundedIndicator, and
     * UnboundedIndicator) for convenience.
     * 
     * NOTE: THIS CLASS USES REFLECTION to create lists of the various Indicator subclasses automatically.  This is done so that
     * as new methods for creating random Indicator objects are added, they will automatically be added to the correct list based
     * on subclass of type Indicator.  In order for new indicators to work with this factory you must:
     *  1. Create a new Indicator class that inherits from the appropriate superclass (PriceOverlay, BoundedIndicator, or
     *  UnboundedIndicator).  See the comments further down about creating new Indicator classes.
     *  2. Create a new method in this class called "createRandom" + (Name of new Indicator class).  It is necessary that the method
     *  start with "createRandom" in order for the relfection process to add the method to the correct list.
     *  
     * FIX? The "create" methods simply return an object of the requested indicator type.  Since these do not actually perform any additional
     * processing, it could be appropriate to remove these methods from this factory.  Then the Indicator classes could be called 
     * directly to simple create a new Indicator object.
     * 
     * FIX? Currently the means for randomly generating parameter values is a realy simple "one size fits all" approach.  It might 
     * be better if more customization could take place.  MAXPERIOD, MINPERIOD, MAXVALUE, and MINVALUE are used by all the
     * indicators as ranges of random values.
     * 
     */
    public static class IndicatorFactory
    {
        static bool isInitialized = false;  //Keeps track if the factory has been initalized.  "Get" methods will initalize class if needed.
        static Random Rnd = new Random();   //Used by several of the "createRandom" methods to random generate parameter values for indicators.
        const int MAXPERIOD = 252;          //Specify the highest value for indicator periods. This was 252
        const int MINPERIOD = 2;            //Do not set below 2.  Specify the lowest value for indicator periods.
        const double MAXVALUE = 100;       //Specify the maximum value for objects of type Value.
        const double MINVALUE = 0;       //Specify the minimum value for objects of type Value. This was -100
        static String[] ABPriceArrays = { "CLOSE" }; //All possible Amibroker price array names.
        static MethodInfo[] crMethodsArr;   //Stores an array of all methods in this class that start with "createRandom".
        static MethodInfo[] poMethodsArr;   //Stores an array of all "createRandom" methods that return PriceOverlay object.
        static MethodInfo[] bMethodsArr;    //Stores an array of all "createRandom" methods that return "BoundedIndicator" objects.
        static MethodInfo[] uMethodsArr;    //Stores an array of all "createRandom" methods that return "UnboundedIndicator" objects.
                                            //static MethodInfo[] dMethodsArr;  //TODO Stores all "CreateRandom" methods that return "Divergence" object.  Not currently
                                            //used, because no such object have been made yet.

        /*
         * Method initialize is called by the "get" methods to populate crMethodsArr, poMethodsArr, and uMethodsArr.  Currently,
         * the dMethodsArr is not populated, because no Divergent Indicator classes have been created yet.
         * 
         * Reflection is used to identify all methods of this class that start with "createRandom".  Those methods are then 
         * stored in the MethodInfo arrays. All createRandom methods are stored in crMethods.  CreateRandom methods which
         * return a PriceOverlay object are stored in poMethodsArr.  Those that return a BoundedIndicator object are
         * stored in the bMethodsArr, and those that return an UnboundedIndicator object are stored in the uMethodsArr.
         * 
         * As new createRandom methods are added to the class (one for each Indicator class), the Initalize method will
         * automatically place the method in the correct MethodInfo arrays.
         */
        private static void initialize()
        {
            MethodInfo[] allMethods = typeof(IndicatorFactory).GetMethods();
            LinkedList<MethodInfo> crMethodsList = new LinkedList<MethodInfo>();
            foreach (MethodInfo m in allMethods)
            {
                if (m.Name.Contains("createRandom"))    //Of all the methods of this class, only find those that
                {                                       //start with "createRandom".
                    crMethodsList.AddLast(m);
                }
            }
            //getRandomBoundedIndicator custom indicators
            if(FDM_GA_Program.Properties.Settings.Default.kairi)
                foreach (MethodInfo m in allMethods)
                {
                    if (m.Name.Contains("kairi"))    //Of all the methods of this class, only find those that
                    {                                       
                        crMethodsList.AddLast(m);
                    }
                }

            if (FDM_GA_Program.Properties.Settings.Default.PR)
                foreach (MethodInfo m in allMethods)
                {
                    if (m.Name.Contains("PercentRank"))    //Of all the methods of this class, only find those that
                    {
                        crMethodsList.AddLast(m);
                    }
                }

            if (FDM_GA_Program.Properties.Settings.Default.KF)
                foreach (MethodInfo m in allMethods)
                {
                    if (m.Name.Contains("KalmanFilter"))    //Of all the methods of this class, only find those that
                    {
                        crMethodsList.AddLast(m);
                    }
                }
            if (FDM_GA_Program.Properties.Settings.Default.ITrend)
                foreach (MethodInfo m in allMethods)
                {
                    if (m.Name.Contains("ITrend"))    //Of all the methods of this class, only find those that
                    {
                        crMethodsList.AddLast(m);
                    }
                }

            //end
            crMethodsArr = crMethodsList.ToArray<MethodInfo>();

            LinkedList<MethodInfo> poMethodsList = new LinkedList<MethodInfo>();
            LinkedList<MethodInfo> bMethodsList = new LinkedList<MethodInfo>();
            LinkedList<MethodInfo> uMethodsList = new LinkedList<MethodInfo>();
            //LinkedList<MethodInfo> dMethodsList = new LinkedList<MethodInfo>();   //Not currently used
            foreach (MethodInfo cr in crMethodsArr)
            {
                Indicator temp = cr.Invoke(null, null) as Indicator;    //Use reflection to call the method and return Indicator object.
                if (temp.GetType().BaseType.Name.Equals("PriceOverlayIndicator"))
                {
                    poMethodsList.AddLast(cr);
                }
                else if (temp.GetType().BaseType.Name.Equals("BoundedIndicator"))
                {
                    bMethodsList.AddLast(cr);
                }
                else if (temp.GetType().BaseType.Name.Equals("UnboundedIndicator"))
                {
                    uMethodsList.AddLast(cr);
                }
                /*
                else if (temp.GetType().BaseType.Name.Equals("Divergece"))
                {
                    dMethodList.AddLast(cr);
                }
                */
            }
            poMethodsArr = poMethodsList.ToArray<MethodInfo>();
            bMethodsArr = bMethodsList.ToArray<MethodInfo>();
            uMethodsArr = uMethodsList.ToArray<MethodInfo>();
            //dMethodsArr = dMethodsList.ToArray<MethodIndo>();
            isInitialized = true;   //Once initalized, no need to initialize again.
        }
        /*
         * Returns any random Indicator object, regardless of its subclass type.  Uses the methods stored
         * in crMethodsArr to randomly call a "createRandom" method from this class.
         */
        public static Indicator getRandomIndicator()
        {
            if (!isInitialized)
            {
                initialize();
            }
            int rndNum = Rnd.Next(crMethodsArr.Length);
            return crMethodsArr[rndNum].Invoke(null, null) as Indicator; //Using reflection to call method.
        }
        /*
         * Returns any random PriceOverlay object.  Uses the methods stored in poMethodsArr to randomly call
         * a "createRandom" method from this class.
         */
        public static PriceOverlayIndicator getRandomPriceOverlayIndicator()
        {
            if (!isInitialized)
            {
                initialize();
            }
            int rndNum = Rnd.Next(poMethodsArr.Length);
            return poMethodsArr[rndNum].Invoke(null, null) as PriceOverlayIndicator;    //using relfection to call method.
        }
        /*
         * Returns any random BoundedIndicator object.  Uses the methods stored in bMethodsArr to randomly
         * call a "createRandom" method from this class.
         */
        public static BoundedIndicator getRandomBoundedIndicator()
        {
            if (!isInitialized)
            {
                initialize();
            }
            int rndNum = Rnd.Next(bMethodsArr.Length);
            return bMethodsArr[rndNum].Invoke(null, null) as BoundedIndicator;  //Using reflection to call method.
        }
        /*
         * Returns any random UnboundedIndicator object.  Uses the methods stored in uMethodsArr to randomly
         * call a "createRandom" method from this class.
         */
        public static UnboundedIndicator getRandomUnboundedIndicator()
        {
            if (!isInitialized)
            {
                initialize();
            }
            int rndNum = Rnd.Next(uMethodsArr.Length);
            return uMethodsArr[rndNum].Invoke(null, null) as UnboundedIndicator;    //Using relfection to call method.
        }

        /*
         * Returns a ValueIndicator object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createValue(String value)
        {
            return new ValueIndicator(value);
        }

        public static Indicator createRandomValue()
        {
            //FIX!
            //In this context we really don't know what an appropirate value will be to compare against another indicator.
            //It would be better to generate random values based on the indicator that this value will be compared to.
            double rndNum = Rnd.NextDouble() * (MAXVALUE - MINVALUE) + MINVALUE;
            return new ValueIndicator(rndNum.ToString());
        }
        /*
         * Returns a ABPriceArray object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createABPriceArray(String arrayName)
        {
            return new ABPriceArrayIndicator(arrayName);
        }

        public static Indicator createRandomABPriceArray()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            return new ABPriceArrayIndicator(rndArray);
        }
        /*
         * Returns an AccumualtionDistribution object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createAccumulationDistribution()
        {
            return new AccumulationDistribution();
        }
        //TODO: Missing createRandom method for AccumulationDistribution!

        /*
         * Returns a AverageDirectionalIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createAverageDirectionalIndex(String period)
        {
            return new AverageDirectionalIndex(period);
        }

        public static Indicator createRandomAverageDirectionalIndex()
        {
            int rndPeriod = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new AverageDirectionalIndex(rndPeriod.ToString());
        }
        /*
         * Returns a AverageTrueRange object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createAverageTrueRange(String period)
        {
            return new AverageTrueRange(period);
        }

        public static Indicator createRandomAverageTrueRange()
        {
            int rndPeriod = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new AverageTrueRange(rndPeriod.ToString());
        }

        /*
         * Returns a CommodityChannelIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createCommodityChannelIndex(String period)
        {
            return new CommodityChannelIndex(period);
        }
        public static Indicator createRandomCommodityChannelIndex()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new CommodityChannelIndex(rndNum.ToString());
        }
        
        /*
         * Returns a BOllingerBandTop object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createBollingerBandTop(String array, String period, String width)
        {
            return new BollingerBandTop(array, period, width);
        }

        public static Indicator createRandomBollingerBandTop()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            double rndWidth = Rnd.NextDouble() * 5;
            return new BollingerBandTop(rndArray, rndNum.ToString(), rndWidth.ToString());
        }

        /*
         * Returns a BollingerBandBottom object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createBollingerBandBottom(String array, String period, String width)
        {
            return new BollingerBandBottom(array, period, width);
        }

        public static Indicator createRandomBollingerBandBottom()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            double rndWidth = Rnd.NextDouble() * 5;
            return new BollingerBandTop(rndArray, rndNum.ToString(), rndWidth.ToString());
        }

        /*
         * Returns a Chaikin object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createChaikin(String fastPeriod, String slowPeriod)
        {
            return new Chaikin(fastPeriod, slowPeriod);
        }

        public static Indicator createRandomChaikin()
        {
            int rndSlowNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndFastNum = Rnd.Next(MINPERIOD, rndSlowNum);
            if (rndSlowNum == rndFastNum) //Can happen when rndSlowNum == MINPERIOD.
            {
                rndFastNum--;   //Fast should be less than slow
            }
            return new Chaikin(rndFastNum.ToString(), rndSlowNum.ToString());
        }

        /*
         * Returns a DoubleExponentialMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createDoubleExponentialMovingAverage(String array, String period)
        {
            return new DoubleExponentialMovingAverage(array, period);
        }

        public static Indicator createRandomDoubleExponentialMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new DoubleExponentialMovingAverage(rndArray, rndNum.ToString());
        }

        /*
         * Returns a ExponentialMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createExpoentialMovingAverage(String array, String period)
        {
            return new ExponentialMovingAverage(array, period);
        }

        public static Indicator createRandomExponentialMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new ExponentialMovingAverage(rndArray, rndNum.ToString());
        }

        /*
         * Returns a LinearRegression object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createLinearRegression(String array, String period)
        {
            return new LinearRegression(array, period);
        }

        public static Indicator createRandomLinearRegression()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new LinearRegression(rndArray, rndNum.ToString());
        }

        /*
         * Returns a MACD object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createMACD(String fastPeriod, String slowPeriod)
        {
            return new MACD(fastPeriod, slowPeriod);
        }

        public static Indicator createRandomMACD()
        {
            int rndSlowNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndFastNum = Rnd.Next(MINPERIOD, rndSlowNum);
            if (rndSlowNum == rndFastNum) //Can happen when rndSlowNum == MINPERIOD.
            {
                rndFastNum--;   //Fast should be less than slow
            }
            return new MACD(rndFastNum.ToString(), rndSlowNum.ToString());
        }

        /*
         * Returns a MinusDirectionalIndicator object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createMinusDirectionalIndicator(String period)
        {
            return new MinusDirectionalIndicator(period);
        }

        public static Indicator createRandomMinusDirectionalIndicator()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new MinusDirectionalIndicator(rndNum.ToString());
        }

        /*
         * Returns a MoneyFlowIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createMoneyFlowIndex(String period)
        {
            return new MoneyFlowIndex(period);
        }

        public static Indicator createRandomMoneyFlowIndex()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new MoneyFlowIndex(rndNum.ToString());
        }

        /*
         * Returns a OnBalanceVolume object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createOnBalanceVolume()
        {
            return new OnBalanceVolume();
        }

        public static Indicator createRandomOnBalanceVolume()
        {
            //There are no parameters to randomize for OBV.
            return new OnBalanceVolume();
        }


        public static Indicator createkairi(String array, String period)
        {

            return new kairi(array, period);

        }

        public static Indicator customkairi()
        {

            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];

            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);

            return new kairi(rndArray, rndNum.ToString());

        }

        public static Indicator createPercentRank(String array, String period)
        {

            return new PercentRank(array, period);

        }

        public static Indicator customPercentRank()
        {

            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];

            int rndperiod = Rnd.Next(MINPERIOD, MAXPERIOD);



            return new PercentRank(rndArray, rndperiod.ToString());

        }

        public static Indicator createFrama(String array,int period)
        {

            return new Frama(array , period);

        }

        public static Indicator customFrama()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            return new Frama(rndArray,rndNum);

        }

        public static Indicator createKalmanFilter(String array, String period)
        {

            return new KalmanFilter(array, period);

        }

        public static Indicator customKalmanFilter()
        {

            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];

            int rndPeriod = Rnd.Next(MINPERIOD, MAXPERIOD);

            return new KalmanFilter(rndArray, rndPeriod.ToString());

        }

        //public static Indicator createKAMA(String array, String Volume, String Price)
        //{

        //    return new KAMA(array, Volume, Price);

        //}

        //public static Indicator createRandomKAMA()
        //{

        //    String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];

        //    int rndPrice = Rnd.Next(Price);

        //    int rndVolume = Rnd.Next(Volume);

        //    return new KAMA(rndArray, rndPrice.ToString(), rndVolume.ToString());

        //}

        public static Indicator createITrend(String array, String alpha)
        {

            return new ITrend(array, alpha);

        }

        public static Indicator customITrend()
        {

            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];

            int rndalpha = Rnd.Next(MINPERIOD, MAXPERIOD);



            return new ITrend(rndArray, rndalpha.ToString());

        }

        /*
         * Returns a ParabolicStopAndReverse object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
      //  public static Indicator createParabolicStopAndReverse(String acceleration, String maxAcceleration)
      //  {
      //      return new ParabolicStopAndReverse(acceleration, maxAcceleration);
      //  }

      //  public static Indicator createRandomParabolicStopAndReverse()
      //  {
      //      double rndAccel = Rnd.NextDouble() / 10;
      //      double rndMax = Rnd.NextDouble() / 5 + rndAccel;
      //      return new ParabolicStopAndReverse(rndAccel.ToString(), rndMax.ToString());
      //  }
        /*
         * Returns a PlusDirectionalIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createPlusDirectionalIndicator(String period)
        {
            return new PlusDirectionalIndicator(period);
        }

        public static Indicator createRandomPlusDirectionalIndicator()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new PlusDirectionalIndicator(rndNum.ToString());
        }

        /*
         * Returns a PositiveVolumeIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createPositiveVolumeIndex()
        {
            return new PositiveVolumeIndex();
        }

        public static Indicator createRandomPositiveVolumeIndex()
        {
            //No parameters to randomize.
            return new PositiveVolumeIndex();
        }

        /*
         * Returns a PirceOscillarot object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createPriceOscillator(String fastPeriod, String slowPeriod)
        {
            return new PriceOscillator(fastPeriod, slowPeriod);
        }

        public static Indicator createRandomPriceOscillator()
        {
            int rndSlowNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndFastNum = Rnd.Next(MINPERIOD, rndSlowNum);
            if (rndSlowNum == rndFastNum) //Can happen when rndSlowNum == MINPERIOD.
            {
                rndFastNum--;   //Fast should be less than slow
            }
            return new PriceOscillator(rndFastNum.ToString(), rndSlowNum.ToString());
        }

        /*
         * Returns a RelativeMomentum object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createRelativeMomentumIndex(String period, String momentum)
        {
            return new RelativeMomentumIndex(period, momentum);
        }

        public static Indicator createRandomRelativeMomentumIndex()
        {
            int rndPeriod = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndMomentum = rndPeriod * 5 / 20;   //Momentum based on ratio of AB default momentum/period.
            return new RelativeMomentumIndex(rndPeriod.ToString(), rndMomentum.ToString());
        }

        /*
         * Returns a RandWalkIndex object.
         * NOTE: This method name had to be shorted to "Rand" from "Random" so the initialize method wouldn't
         * add it to the "createRandom" methods array.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createRandWalkIndex(String minPeriod, String maxPeriod)
        {
            return new RandomWalkIndex(minPeriod, maxPeriod);
        }

        public static Indicator createRandomRandWalkIndex()
        {
            int rndSlowNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndFastNum = Rnd.Next(MINPERIOD, rndSlowNum);
            if (rndSlowNum == rndFastNum) //Can happen when rndSlowNum == MINPERIOD.
            {
                rndFastNum--;   //Fast should be less than slow
            }
            return new RandomWalkIndex(rndFastNum.ToString(), rndSlowNum.ToString());
        }

        /*
         * Returns a RateOfChange object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createRateOfChange(String array, String period)
        {
            return new RateOfChange(array, period);
        }

        public static Indicator createRandomRateOfChange()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new RateOfChange(rndArray, rndNum.ToString());
        }

        /*
         * Returns a RelativeStrengthIndex object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createRelativeStrengthIndex(String array, String period)
        {
            return new RelativeStrengthIndex( period);
        }

        public static Indicator createRandomRelativeStrength()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new RelativeStrengthIndex( rndNum.ToString());
        }

        /*
         * Returns a SimpleMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createSimpleMovingAverage(String array, String period)
        {
            return new SimpleMovingAverage(array, period);
        }

        public static Indicator createRandomSimpleMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new SimpleMovingAverage(rndArray, rndNum.ToString());
        }

        /*
         * Returns a StochasticPercentD object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createStochasticPercentD(String period, String kSmooth, String dSmooth)
        {
            return new StochasticPercentD(period, kSmooth, dSmooth);
        }

        public static Indicator createRandomStochasticPercentD()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndKD = Rnd.Next(rndNum * 3 / 14);
            return new StochasticPercentD(rndNum.ToString(), rndKD.ToString(), rndKD.ToString());
        }

        /*
         * Returns a StochasticPercentK object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createStochasticPercentK(String period, String kSmooth)
        {
            return new StochasticPercentK(period, kSmooth);
        }

        public static Indicator createRandomStochasticPercentK()
        {
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndK = Rnd.Next(rndNum * 3 / 14);
            return new StochasticPercentK(rndNum.ToString(), rndK.ToString());
        }

        /*
         * Returns a TripleExpontialMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createTripleExponentialMovingAverage(String array, String period)
        {
            return new TripleExponentialMovingAverage(array, period);
        }

        public static Indicator createRandomTripleExponentialMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new TripleExponentialMovingAverage(rndArray, rndNum.ToString());
        }

        /*
         * Returns a TimeSeriesForcast object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createTimeSeriesForcast(String array, String period)
        {
            return new TimeSeriesForecast(array, period);
        }

        public static Indicator createRandomTimeSeriesForecast()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new TimeSeriesForecast(rndArray, rndNum.ToString());
        }

        /*
         * Returns a VolumeOscillator object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createVolumeOscillator(String fastPeriod, String slowPeriod)
        {
            return new VolumeOscillator(fastPeriod, slowPeriod);
        }

        public static Indicator createRandomVolumeOscillator()
        {
            int rndSlowNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            int rndFastNum = Rnd.Next(MINPERIOD, rndSlowNum);
            if (rndSlowNum == rndFastNum) //Can happen when rndSlowNum == MINPERIOD.
            {
                rndFastNum--;   //Fast should be less than slow
            }
            return new VolumeOscillator(rndFastNum.ToString(), rndSlowNum.ToString());
        }
        /*
         * Returns a WildersMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createWildersMovingAverage(String array, String period)
        {
            return new WildersMovingAverage(array, period);
        }

        public static Indicator createRandomWildersMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new WildersMovingAverage(rndArray, rndNum.ToString());
        }

        /*
         * Returns a WeightedMovingAverage object.
         * FIX? This method could be removed as an object could be created by calling the class directly.
         */
        public static Indicator createWeightedMovingAverage(String array, String period)
        {
            return new WeightedMovingAverage(array, period);
        }

        public static Indicator createRandomWeightedMovingAverage()
        {
            String rndArray = ABPriceArrays[Rnd.Next(ABPriceArrays.Length)];
            int rndNum = Rnd.Next(MINPERIOD, MAXPERIOD);
            return new WeightedMovingAverage(rndArray, rndNum.ToString());
        }
    }
    /*
     * Below are all of the Indicator classes which the factory method above uses to create Indicator objects.
     * 
     * Class Indicator is abstract, and has all the functionality common to all of the subclasses.  Every
     * Indicator has an ABName and a dictionary of parameters. (Some may have no parameters, in which case
     * the dictionary remains empty.)  All Indictor object also return a String representing their
     * Amibroker code so this abstract class implements the method that does that.
     */
    public abstract class Indicator
    {
        protected String ABname;    //The function name that Amibroker uses for the Indicator.
        protected Dictionary<String, String> parameters; //Stores the name an value of parameters.

        public Indicator()
        {
            ABname = "Abstract indicator";
            parameters = new Dictionary<string, string>();
        }
        /*
         * Method isComparable is implements by each Indicator subclass.
         * Its purpose to to compare this indicator with another to see if they can even be
         * compared by Amibroker for crosses and threshold rules.  The mechanisms for
         * validating comparsion are quite basic, and a lot of improvement could be made.
         */
        public abstract bool isComparable(Indicator ind);

        public String getABname()
        {
            return ABname;
        }
        /*
         * Method getABCode returns the Amibroker code needed to call the corresponding indicator in
         * Amibroker.  Returns a String like: name(parameter1,parameter2,...).  There could be zero
         * or more parameters in the parenthesis.
         */
        public virtual String getABCode()
        {
            StringBuilder p = new StringBuilder();
            p.Append("(");
            foreach (KeyValuePair<String, String> pair in parameters)
            {
                p.Append(pair.Value).Append(",");
            }
            if (parameters.Count > 0)
            {
                p.Length -= 1;  //Remove extra "," at end.
            }
            p.Append(")");
            return ABname + p.ToString();
        }
    }

    /*
     * ValueIndicator objects represent numbers that Amibroker uses to compare against other Indicators.
     * For example, and RSI object could be cross the value 80.
     */
    public class ValueIndicator : Indicator
    {
        public ValueIndicator(string value) : base()
        {
            double temp;
            if (double.TryParse(value, out temp))
            {
                ABname = value;
            }
            else
            {
                throw new ArgumentException(value + " cannot be converted to a number.");
            }
            //No parameters to add for value.
        }

        public override String getABCode()
        {
            //It is just a number.  So, no parenthesis or indicators should be returned.
            return ABname;
        }

        public override bool isComparable(Indicator otherIndicator)
        {
            if (otherIndicator is BoundedIndicator)
            {
                return otherIndicator.isComparable(this);
            }
            else if (otherIndicator is UnboundedIndicator)
            {
                return otherIndicator.isComparable(this);
            }

            return false;
        }
    }
    /*
     * PriceOverlayIndicator object represent Amibroker indicators that could be placed directly on the
     * stock price candlestick charts.
     */
    public abstract class PriceOverlayIndicator : Indicator
    {
        public PriceOverlayIndicator() : base()
        {
        }

        public override bool isComparable(Indicator otherIndicator)
        {
            if (otherIndicator is PriceOverlayIndicator)
            {
                return true;
            }
            return false;
        }
    }
    /*
     * BoundedIndicator objects represent Amibroker indicators which do not overlay on the price
     * charts, but do have a limited range, like from 0 to 100.
     */
    public abstract class BoundedIndicator : Indicator
    {
        protected int lowerBound { get; set; }
        protected int upperBound { get; set; }

        public BoundedIndicator(int lower, int upper) : base()
        {
            lowerBound = lower;
            upperBound = upper;
        }

        public BoundedIndicator()
        {
            // TODO: Complete member initialization
        }

        public override bool isComparable(Indicator otherIndicator)
        {
            if (otherIndicator is BoundedIndicator)
            {
                BoundedIndicator BI = (BoundedIndicator)otherIndicator;
                if (lowerBound == BI.lowerBound && upperBound == BI.upperBound)
                {
                    return true;
                }
                return false;
            }
            else if (otherIndicator is ValueIndicator)
            {
                double value;
                if (double.TryParse(otherIndicator.getABname(), out value) && value >= lowerBound && value <= upperBound)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    /*
     * UnboundedIndicator objects represent Amibroker indicators which cannot be overlayed on a price chart,
     * and they do not have a defined upper and lower limit.
     */
    public abstract class UnboundedIndicator : Indicator
    {
        //Can we even comparing an unbounded indicator to anything other than values?
        //We cannot be for sure if the value will be within the unbounded indicator, because the bounds are not known!
        public override bool isComparable(Indicator otherIndicator)
        {
            if (otherIndicator is ValueIndicator)
            {
                return true;
            }
            //If the unboudned indicators are the same type, it should be OK to compare them.
            //Although if they have the same parameters the then will never create a cross!
            else if (GetType().Equals(otherIndicator.GetType()))
            {
                return true;
            }
            return false;
        }
    }

    //TODO create abstract class of Divergence indicators!

    /*
     * ABPriceArrayIndicator object represent Amibroker's built in price arrays.
     */
    public class ABPriceArrayIndicator : PriceOverlayIndicator
    {
        String[] acceptableNames = { "open", "high", "low", "close", "o", "h", "l", "c" };

        public ABPriceArrayIndicator(String arrayName) : base()
        {
            if (acceptableNames.Contains(arrayName.ToLower()))
            {
                ABname = arrayName;
            }
            else
            {
                throw new ArgumentException(arrayName + " is not an acceptable Amibroker arrray name.");
            }
            //No parameters for Amibroker price array.
        }

        public override String getABCode()
        {
            //It is just a name.  So, no parenthesis or indicators should be returned.
            return ABname;
        }
    }
    public class AccumulationDistribution : UnboundedIndicator
    {
        public AccumulationDistribution() : base()
        {
            ABname = "AccDist";
            //No parameters for AccDist.
        }
    }

    public class AverageDirectionalIndex : BoundedIndicator
    {
        public AverageDirectionalIndex(String period) : base(0, 100)
        {
            ABname = "ADX";
            parameters.Add("Period", period);
        }
    }

    public class AverageTrueRange : UnboundedIndicator
    {
        public AverageTrueRange(String period) : base()
        {
            ABname = "ATR";
            parameters.Add("Period", period);
        }
    }

    public class BollingerBandTop : PriceOverlayIndicator
    {
        public BollingerBandTop(String array, String period, String width) : base()
        {
            ABname = "BBandTop";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
            parameters.Add("Width", width);
        }
    }

    public class BollingerBandBottom : PriceOverlayIndicator
    {
        public BollingerBandBottom(String array, String period, String width) : base()
        {
            ABname = "BBandBottom";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
            parameters.Add("Width", width);
        }
    }

    public class CommodityChannelIndex : UnboundedIndicator
    {
        public CommodityChannelIndex(String period) : base()
        {
            ABname = "CCI";
            parameters.Add("Period", period);
        }
    }
    public class Chaikin : UnboundedIndicator
    {
        public Chaikin(String fastPeriod, String slowPeriod) : base()
        {
            ABname = "chaikin";
            parameters.Add("Fast", fastPeriod);
            parameters.Add("Slow", slowPeriod);
        }
    }

    public class DoubleExponentialMovingAverage : PriceOverlayIndicator
    {
        public DoubleExponentialMovingAverage(String array, String period) : base()
        {
            ABname = "dema";
            parameters.Add("PriceArray", array);
            parameters.Add("Period", period);
        }
    }

    public class ExponentialMovingAverage : PriceOverlayIndicator
    {
        public ExponentialMovingAverage(String array, String period) : base()
        {
            ABname = "EMA";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class LinearRegression : PriceOverlayIndicator
    {
        public LinearRegression(String array, String period) : base()
        {
            ABname = "LinearReg";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class MACD : UnboundedIndicator
    {
        public MACD(String fastPeriod, String slowPeriod) : base()
        {
            ABname = "macd";
            parameters.Add("Fast", fastPeriod);
            parameters.Add("Slow", slowPeriod);
        }
    }

    public class MinusDirectionalIndicator : BoundedIndicator
    {
        public MinusDirectionalIndicator(string period) : base(0, 100)
        {
            ABname = "MDI";
            parameters.Add("Period", period);
        }
    }

    public class MoneyFlowIndex : BoundedIndicator
    {
        public MoneyFlowIndex(String period) : base(0, 100)
        {
            ABname = "mfi";
            parameters.Add("Period", period);
        }
    }

    public class OnBalanceVolume : UnboundedIndicator
    {
        public OnBalanceVolume() : base()
        {
            ABname = "obv";
            //no parameters to add.
        }
    }

    public class ParabolicStopAndReverse : PriceOverlayIndicator
    {
        public ParabolicStopAndReverse(String acceleration, String maxAcceleration) : base()
        {
            ABname = "SAR";
            parameters.Add("Acceleration", acceleration);
            parameters.Add("MaxAcceleration", maxAcceleration);
        }
    }

    public class PlusDirectionalIndicator : BoundedIndicator
    {
        public PlusDirectionalIndicator(string period) : base(0, 100)
        {
            ABname = "PDI";
            parameters.Add("Period", period);
        }
    }

    public class PositiveVolumeIndex : UnboundedIndicator
    {
        public PositiveVolumeIndex() : base()
        {
            ABname = "PVI";
            //No parameters for PVI.
        }
    }

    public class PriceOscillator : UnboundedIndicator
    {
        public PriceOscillator(String fastPeriod, String slowPeriod) : base()
        {
            ABname = "OscP";
            parameters.Add("FastPeriod", fastPeriod);
            parameters.Add("SlowPeriod", slowPeriod);
        }
    }

    public class RandomWalkIndex : UnboundedIndicator
    {
        public RandomWalkIndex(String minPeriod, String maxPeriod)
        {
            ABname = "RWI";
            parameters.Add("MinimumPeriod", minPeriod);
            parameters.Add("MaximumPeriod", maxPeriod);
        }
    }

    public class RateOfChange : UnboundedIndicator
    {
        public RateOfChange(String array, String period)
        {
            ABname = "ROC";
            parameters.Add("Period", period);
        }
    }
    public class RelativeMomentumIndex : BoundedIndicator
    {
        public RelativeMomentumIndex(String period, String momentum) : base(0, 100)
        {
            ABname = "RMI";
            parameters.Add("Period", period);
            parameters.Add("Momentum", momentum);
        }
    }

    public class RelativeStrengthIndex : BoundedIndicator
    {
        public RelativeStrengthIndex( String period) : base(0, 100)
        {
            ABname = "RSI";
            
            parameters.Add("Period", period);
        }
    }

    public class SimpleMovingAverage : PriceOverlayIndicator
    {
        public SimpleMovingAverage(String array, String period) : base()
        {
            ABname = "MA";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class StochasticPercentD : BoundedIndicator
    {
        public StochasticPercentD(String period, String kSmooth, String dSmooth) : base(0, 100)
        {
            ABname = "StochD";
            parameters.Add("Period", period);
            parameters.Add("KSmooth", kSmooth);
            parameters.Add("DSmooth", dSmooth);
        }
    }

    public class StochasticPercentK : BoundedIndicator
    {
        public StochasticPercentK(String period, String kSmooth) : base(0, 100)
        {
            ABname = "StochK";
            parameters.Add("Period", period);
            parameters.Add("KSmooth", kSmooth);
        }
    }
    public class TripleExponentialMovingAverage : PriceOverlayIndicator
    {
        public TripleExponentialMovingAverage(String array, String period) : base()
        {
            ABname = "tema";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class TimeSeriesForecast : PriceOverlayIndicator
    {
        public TimeSeriesForecast(String array, String period) : base()
        {
            ABname = "TSF";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class VolumeOscillator : UnboundedIndicator
    {
        public VolumeOscillator(String fastPeriod, String slowPeriod)
        {
            ABname = "OscV";
            parameters.Add("FastPeriod", fastPeriod);
            parameters.Add("SlowPeriod", slowPeriod);
        }
    }

    public class WildersMovingAverage : PriceOverlayIndicator
    {
        public WildersMovingAverage(String array, String period) : base()
        {
            ABname = "Wilders";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class WeightedMovingAverage : PriceOverlayIndicator
    {
        public WeightedMovingAverage(String array, String period) : base()
        {
            ABname = "WMA";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class kairi : BoundedIndicator
    {
        public kairi(String array, String period)
            : base(0, 100)
        {
            ABname = "kairi";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }


    public class PercentRank : UnboundedIndicator
    {
        public PercentRank(String array, String period)
            : base()
        {
            ABname = "PercentRank";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }

    public class Frama : BoundedIndicator
    {
        public Frama(String array, int period):base(0,1)
        {
            ABname = "Frama";

        }
    }

    public class KalmanFilter : UnboundedIndicator
    {
        public KalmanFilter(String array, String period)
            : base()
        {
            ABname = "KalmanFilter";
            parameters.Add("Array", array);
            parameters.Add("Period", period);
        }
    }


    //public class KAMA : UnboundedIndicator
    //{
    //    public KAMA(String array, String Price, String Volume)
    //        : base()
    //    {
    //        ABname = "KAMA";
    //        parameters.Add("Array", array);
    //        parameters.Add("Price", Price);
    //        parameters.Add("Volume", Volume);
    //    }
    //}


    public class ITrend : UnboundedIndicator
    {
        public ITrend(String array, String alpha)
            : base()
        {
            ABname = "ITrend";
            parameters.Add("Array", array);
            parameters.Add("alpha", alpha);
        }
    }

}
