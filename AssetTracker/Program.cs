using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<Asset> assetList = new List<Asset>();
            Print(" ---------------------------------\n | ", CC.DarkBlue);
            Print(" Welcome to the AssetTracker ", CC.DarkYellow);
            Print(" | \n ---------------------------------\n", CC.DarkBlue);
            while (true)
            {
                Print("\n Press a desired key to select an option.\n\n", CC.Blue, CC.Black);
                Print(  " Press \"A\" to add an Asset\n" +
                        " Press \"D\" to display Assets\n" +
                        " Press \"Q\" to quit\n", CC.Cyan);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Hide the key from the console
                char keyChar = char.ToLower(keyInfo.KeyChar);
                if (keyChar == 'q')
                {
                    Print("\nExiting application...\n", CC.Red);
                    break;
                }
                else if (keyChar == 'a') Asset.AddAsset(assetList);
                else if (keyChar == 'd') Asset.DisplayAssets(assetList);
                else Print($"\n{keyChar} is not a valid option!\n", CC.Red);
            }   
        }
    }
   
}


