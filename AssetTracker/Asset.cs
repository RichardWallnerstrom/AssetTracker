using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AssetTracker
{
    public class Asset
    {
        public Asset(string type, string brand, string model, string office, double price, double currency, DateTime purchaseDate)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            Price = price;
            Currency = currency;
            PurchaseDate = purchaseDate;
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        
        public string Model { get; set; }
        public string Office { get; set; }
        public double Price { get; set; }
        public double Currency { get; set; }

        public DateTime PurchaseDate { get; set; }

        public void RunOption(char keyInput)
        {

        }
        public static void AddAsset(List<Asset> assetList)
        {
            
        }
    }
}
