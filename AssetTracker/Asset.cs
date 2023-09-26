using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Program.Print("\n What brand is it?  ", CC.Cyan);                        
            string brand = Console.ReadLine();
            Program.Print("\n Which model is it?  ", CC.Cyan);                       
            string model = Console.ReadLine();
            Program.Print("\n Which country is the office located in?  ", CC.Cyan);  
            string location = Console.ReadLine();
            Program.Print("\n What was the price in US$?  ", CC.Cyan);
            double price;
            while (true)
            {
                string priceInput = Console.ReadLine();
                if (double.TryParse(priceInput, out price)) break;
                else Program.Print("\n Invalid price format. Please enter a valid number. ", CC.DarkRed);
            }

            Program.Print("\n What date was it purchased (yyyy-MM-dd):  ", CC.Cyan);   
            DateTime purchaseDate;

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
            if (assetList.Count == 0)
            {
                Program.Print("\n  You haven't added anything to the Asset Tracker.\n", CC.Red);
                return;
            }
            else
            {
                Program.Print("\n     TYPE".PadRight(20) + "BRAND".PadRight(20) + "MODEL".PadRight(20) + "LOCATION".PadRight(20) + "PRICE".PadRight(20) + "PURCHASE DATE".PadRight(20), CC.DarkYellow);
                Program.Print("\n    -------------------------------------------------------------------------------------------------------------\n", CC.DarkBlue);
                foreach (Asset asset in assetList)
                {

                    Program.Print($"\n    {asset.Type.PadRight(15)}{asset.Brand.PadRight(20)}{asset.Model.PadRight(20)}" +
                        $"{asset.Location.PadRight(20)}{asset.Price.ToString().PadRight(20)}{asset.PurchaseDate.ToString().PadRight(20)}  ");
                }
            }
           
        }

    }
}
