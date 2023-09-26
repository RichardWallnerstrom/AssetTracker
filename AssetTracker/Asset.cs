using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC = System.ConsoleColor;


namespace AssetTracker
{
    public class Asset
    {
        public Asset(string type, string brand, string model, string location, double price, DateTime purchaseDate)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Location = location;
            Price = price;
            PurchaseDate = purchaseDate;
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        
        public string Model { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }

        public DateTime PurchaseDate { get; set; }

        public void RunOption(char keyInput)
        {

        }
        public static void AddAsset(List<Asset> assetList)
        {
            Program.Print("\n What type of asset is this?  ", CC.Cyan);
            string type = Console.ReadLine().ToLower();
            while (type != "computer" && type != "phone" && type != "car")
            {
                Program.Print($"\n {type} is not a valid asset. We currently track computers, phones and cars.  \n", CC.Red);
                Program.Print("\n What type of asset is this?  ", CC.Cyan);
                type = Console.ReadLine().ToLower();

            }
            Program.Print("\n What brand is it?  ", CC.Cyan);                        string brand = Console.ReadLine();
            Program.Print("\n Which model is it?  ", CC.Cyan);                       string model = Console.ReadLine();
            Program.Print("\n Which country is the office located in?  ", CC.Cyan);  string location = Console.ReadLine();
            Program.Print("\n How much did it cost in US$?  ", CC.Cyan);             double price = Convert.ToDouble(Console.ReadLine());
            Program.Print("\n Enter the purchase date (yyyy-MM-dd):  ", CC.Cyan);    DateTime purchaseDate;

            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out purchaseDate))
                {
                    Asset newAsset = new Asset(type, brand, model, location, price, purchaseDate);
                    assetList.Add(newAsset);
                    Program.Print("\n  " + char.ToUpper(newAsset.Brand[0]) + newAsset.Brand.Substring(1) + " " 
                        + newAsset.Type + " added to the office in "    
                        + char.ToUpper(newAsset.Location[0]) + newAsset.Location.Substring(1) 
                        + ".\n", CC.DarkGreen);
                    break;
                }
                else
                {
                    Program.Print("\n Invalid date format. ", CC.DarkRed);
                }
            }
        }
        public static void DisplayAssets(List<Asset> assetList)
        {
            foreach (Asset asset in assetList)
            {
                Program.Print($"\n {asset.Type.PadRight(20)}{asset.Brand.PadRight(20)}{asset.Model.PadRight(20)}{asset.Location.PadRight(20)}{asset.Price.ToString().PadRight(20)}{asset.PurchaseDate.ToString().PadRight(20)}  ");
            }
        }
    }
}
