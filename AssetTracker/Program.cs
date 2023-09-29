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

namespace AssetTracker
{
    internal class Program
    {
        internal static void Print(string text, CC fgColor = CC.White, CC bgColor = CC.Black)
        {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.Write(text);
            Console.ResetColor();
        }
        internal static string Truncate(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= 14 ? value : value.Substring(0, 14);
        }
        internal static string TruncateNumber(string value)     // If number string is long truncate to millions or billions
        {
            string[] truncedValue;
            if (value.Contains(","))                            // If comma is present
            {
                truncedValue = value.Split(',');
                if (truncedValue[0].Length > 9)                 
                {
                    return truncedValue[0].Substring(0, truncedValue[0].Length - 9) + "," + value.Substring(value.Length - 9, 1) + "B"; // Truncate to billions
                }
                else if (truncedValue[0].Length > 6)
                {
                    return truncedValue[0].Substring(0, truncedValue[0].Length - 6) + "," + value.Substring(value.Length - 6, 1) + "M"; // Truncate to millions
                }
                else
                {
                    return value;
                }
            }
            else        // If no comma
            {
                if (value.Length > 9) 
                {
                    return value.Substring(0, value.Length - 9) + "," + value.Substring(value.Length - 9, 1) + "B"; // Truncate to billions
                }
                else if (value.Length > 6) // Check if there are more than 6 digits
                {
                    return value.Substring(0, value.Length - 6) + "," + value.Substring(value.Length - 6, 1) + "M"; // Truncate to millions
                }
                else
                {
                    return value;
                }
            }
        } 
        internal static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Asset> assetList = new List<Asset>();
            Print(" ---------------------------------\n | ", CC.DarkBlue);
            Print(" Welcome to the AssetTracker ", CC.DarkYellow);
            Print(" | \n ---------------------------------\n", CC.DarkBlue);
            while (true)
            {
                Print("\n Press a desired key to select an option.\n\n", CC.Blue, CC.Black);
                Print(" Press \"A\" to add an Asset\n" +
                        " Press \"D\" to display Assets\n" +
                        " Press \"H\" to display Help\n" +
                        " Press \"Q\" to Quit\n", CC.Cyan);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Hide the key from the console
                char keyChar = char.ToLower(keyInfo.KeyChar);
                string filePath = "eurofxref-daily.xml";
                if (keyChar == 'q')
                {
                    Print("\nExiting application...\n", CC.Red);
                    break;
                }
                else if (keyChar == 'a')
                {
                    Asset.AddAsset(assetList);
                }
                else if (keyChar == 'd')
                {
                    Currencies.DownloadXml(filePath);
                    Currencies.UpdateConversionModifier(assetList, filePath);
                    Asset.DisplayAssets(assetList);
                }
                else if (keyChar == 'h')
                {
                    Print("\n With the Asset tracker you can save products and information about them.\n" +
                        " Display Products will display them sorted by cars, then computers and finally phones\n" +
                        " Within each Type they will be sorted by oldest first\n" +
                        " To keep the display readable all fields will be shortened to 14 characters\n" +
                        " You can add almost any country in the world and it will find the proper country code and currency\n" +
                        " I'm using European Central Banks daily exchange rates to calculate current value\n" +
                        " If you can't display certain currency symbols try to change your console font to: Consolas\n" +
                        " If value is displayed as \"Unknown\" its because I couldn't find the exchange rate\n", CC.DarkYellow);
                }
                else Print($"\n\n          {keyChar} is not a valid option!\n\n", CC.Red);
            }   
        }
    }
   
}


