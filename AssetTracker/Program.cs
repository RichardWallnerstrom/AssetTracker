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
                        " Press \"Q\" to quit\n", CC.Cyan);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Hide the key from the console
                char keyChar = char.ToLower(keyInfo.KeyChar);
                string xmlFilePath = "eurofxref-daily.xml";
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
                    Currencies.DownloadXml(xmlFilePath);
                    Currencies.UpdateConversionModifier(assetList, xmlFilePath);
                    Asset.DisplayAssets(assetList);
                }
                else if (keyChar == 'p')
                {

                }
                else Print($"\n\n          {keyChar} is not a valid option!\n\n", CC.Red);
            }   
        }
    }
   
}


