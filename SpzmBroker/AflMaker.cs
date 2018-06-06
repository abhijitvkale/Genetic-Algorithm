using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDM_GA_Program
{


    // This class takes chromosomes and generates the list of AFL files correspondingly.
    public static class AflMaker
    {
        private static int StopSize = FDM_GA_Program.Properties.Settings.Default.Stop;
      

        private static string EnterFinal, ExitFinal, Enter, Exit;

        // Create AFL files using chromosomes' rules with [] brackets stripped out.
        // Boetticher: It looks like there is double-dipping. This means the procedure is called to set up
        //             the chromosome and also to breed the chromosomes.
        public static void MakeAflFiles(List<Chromosome> chromosomes, string programFolderPath, int tradeType)
        {
            for (int i = 0; i < chromosomes.Count; i++)
            {
                // Boetticher: It looks like breeding involves swapping adjacent chromosomes
                string enterRule = chromosomes[i].EnterRule.Replace("[", string.Empty).Replace("]", string.Empty);
                string exitRule = chromosomes[i].ExitRule.Replace("[", string.Empty).Replace("]", string.Empty);
                SetTradeTypes(tradeType);
                CreateChromosomeAfl(enterRule, exitRule, (i + 1), programFolderPath);
                chromosomes[i].AflPath = programFolderPath + @"\Chromosomes\Chromosome" + (i + 1).ToString() + ".afl";
            }
        }

        // Set AmiBroker variable names according to trade type(long trades or short trades).
        private static void SetTradeTypes(int tradeType)
        {
            // Long trades.
            if (tradeType == 1)
            {
                EnterFinal = "buyFinal";
                ExitFinal = "sellFinal";
                Enter = "Buy";
                Exit = "Sell";
            }
            // Short trades.
            else
            {
                EnterFinal = "shortFinal";
                ExitFinal = "coverFinal";
                Enter = "Short";
                Exit = "Cover";
            }
        }

        // Create AFL files in chromosomes folder.
        private static void CreateChromosomeAfl(string enterRule, string exitRule, int populationCount, string programFolderPath)
        {
            string restrictions = "tn = TimeNum();\r\n" + "startTime = 082500;\r\n" + "endTime = 144000;\r\n" + "exitTime = 145959;\r\n" + "isTradeTime = tn >= startTime AND tn < endTime;\r\n\r\n" + "SetTradeDelays(1,1,1,1);\r\n" + "BuyPrice=SellPrice=ShortPrice=CoverPrice=Open;\r\n" + "pointStop = " + StopSize.ToString() + ";\r\n" + "ApplyStop(stopTypeLoss,stopModePoint,pointStop,2);\r\n" + "SetPositionSize("+FDM_GA_Program.Properties.Settings.Default.PosSize.ToString()+", spsPercentOfEquity);\r\n\r\n";
            string dateRestriction = "Dt = DateNum();\r\n"+"StartDate= ParamDate( \"Start Date\",  \""  +FDM_GA_Program.Properties.Settings.Default.StartDate +"\");\r\n"+"EndDate = ParamDate( \"End Date\", \""+FDM_GA_Program.Properties.Settings.Default.EndDate+"\" );\r\n" +"TradeDates = Dt >=StartDate AND Dt< EndDate;\r\n\n\n";
            string finalRules = EnterFinal + " = " + enterRule + ";\r\n" + ExitFinal + " = " + exitRule + ";\r\n";
            string triggers = Enter + " = " + EnterFinal + " AND isTradeTime AND TradeDates;\r\n" + Exit + " = (" + ExitFinal + " AND isTradeTime) OR Cross(tn,  exitTime) AND TradeDates;";
            System.IO.File.WriteAllText(programFolderPath + @"\Chromosomes\Chromosome" + populationCount.ToString() + ".afl", restrictions+dateRestriction + finalRules + triggers);
        }
    }
}
