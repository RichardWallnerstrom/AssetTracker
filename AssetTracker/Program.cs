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
        internal static void ReadAndSki()
        {
            byte[] code = {
        84, 104, 101, 114, 101, 32, 97, 114, 101, 32, 51, 32, 116, 121, 112, 101,
        115, 32, 111, 102, 32, 112, 101, 111, 112, 108, 101, 46, 32, 84, 104, 111,
        115, 101, 32, 121, 111, 117, 32, 99, 97, 110, 32, 99, 111, 117, 110, 116,
        32, 98, 105, 110, 97, 114, 121, 46, 32, 65, 110, 100, 32, 116, 104, 111,
        115, 101, 32, 119, 104, 111, 32, 99, 97, 110, 32, 99, 111, 117, 110, 116,
        32, 98, 105, 110, 97, 114, 121, 46, 32, 65, 110, 100, 32, 116, 104, 111,
        115, 101, 32, 119, 104, 111, 32, 99, 97, 110, 110, 111, 116
    };

            string decodedText = Encoding.UTF8.GetString(code);

            Print("This is three sentences written in their corresponding ASCII code. Can you decipher it? :)\n");
            Print($"Deciphered Text: {decodedText}\n", CC.DarkCyan);

            string userInput = Console.ReadLine();
            if (userInput == decodedText)
            {
                Print("\nOMG You actually got it?!?!?!\n", CC.DarkMagenta);
            }
            else
            {
                Print("\nSorry, try again!\n", CC.Red);
            }
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
                else if (keyChar == 'p') ReadAndSki();
                else Print($"\n\n          {keyChar} is not a valid option!\n\n", CC.Red);
            }   
        }
    }
   
}


