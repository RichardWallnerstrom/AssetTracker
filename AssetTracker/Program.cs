using Nager.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CC = System.ConsoleColor;

namespace AssetTrackerEfCore {
    internal class Program {
        internal static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;     //To allow currency symbols.
            string filePath = "eurofxref-daily.xml";
            List<Asset> assetList = new List<Asset>();
            PrintWelcome();
            while (true) {
                PrintStartOptions();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Hide the key from the console
                char keyChar = char.ToLower(keyInfo.KeyChar);
                if (keyChar == 'q') {
                    Print("\nExiting application...\n", CC.Red);
                    return;
                } else if (keyChar == 'a') {
                    Asset.AddAsset();
                } else if (keyChar == 'd') {
                    Currencies.DownloadXml(filePath);
                    using (var context = new AssetContext()) {
                        assetList = context.Assets.ToList();
                        Currencies.UpdateConversionModifier(assetList, filePath);
                        context.SaveChanges();
                    }
                    Asset.DisplayAssets();
                } else if (keyChar == 'e') { 
                    Asset.EditAsset();
                } else if (keyChar == 's') { 
                    Asset.FindAsset();
                } else if (keyChar == 'h') {
                    PrintHelp();
                } else
                    Print($"\n\n          {keyChar} is not a valid option!\n\n", CC.Red);
            }
        }
        internal static void Print(string text, CC fgColor = CC.White, CC bgColor = CC.Black) {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.Write(text);
            Console.ResetColor();
        }
        internal static string Truncate(string value) {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= 14 ? value : value.Substring(0, 14);
        }
        internal static string TruncateNumber(string value) {     // If number string is long truncate to millions or billions
            string[] truncedValue;
            if (value.Contains(",")) { // IF/Else to deal with possible comma (,)
                truncedValue = value.Split(',');
                if (truncedValue[0].Length > 9) {   /// Truncate to billions
                    return truncedValue[0].Substring(0, truncedValue[0].Length - 9)
                        + "," + value.Substring(value.Length - 9, 1)
                        + "B";
                } else if (truncedValue[0].Length > 6) {    /// Truncate to millions
                    return truncedValue[0].Substring(0, truncedValue[0].Length - 6)
                        + "," + value.Substring(value.Length - 6, 1)
                        + "M";
                } else {
                    return value;
                }
            } else {  // If no comma
                if (value.Length > 9) { /// Truncate to billions
                    return value.Substring(0, value.Length - 9)
                        + "," + value.Substring(value.Length - 9, 1)
                        + "B";
                } else if (value.Length > 6) {  /// Truncate to millions
                    return value.Substring(0, value.Length - 6)
                        + "," + value.Substring(value.Length - 6, 1)
                        + "M";
                } else {
                    return value;
                }
            }
        }
        private static void PrintStartOptions() {
        Print("\n Press a desired key to select an option.\n\n", CC.Blue, CC.Black);
        Print(" Press \"A\" to add an Asset\n" +
                        " Press \"D\" to display Assets\n" +
                        " Press \"E\" to edit in Database\n" +
                        " Press \"S\" to search in Database\n" +
                        " Press \"H\" to display Help\n" +
                        " Press \"Q\" to Quit\n", CC.Cyan);
        }
        private static void PrintHelp() {
            Print("\n With the Asset tracker you can save products and information about them.\n" +
    " Display Products will display them sorted by cars, then computers and finally phones\n" +
    " Within each Type they will be sorted by oldest first\n" +
    " To keep the display readable all fields will be shortened to 14 characters\n" +
    " You can add almost any country in the world and it will find the proper country code and currency\n" +
    " I'm using European Central Banks daily exchange rates to calculate current value\n" +
    " If you can't display certain currency symbols try to change your console font to: Consolas\n" +
    " If value is displayed as \"Unknown\" its because I couldn't find the exchange rate\n", CC.DarkYellow);
        }
        private static void PrintWelcome() {
            Print(" ---------------------------------\n | ", CC.DarkBlue);
            Print(" Welcome to the AssetTracker ", CC.DarkYellow);
            Print(" | \n ---------------------------------\n", CC.DarkBlue);
        }
    }
}


