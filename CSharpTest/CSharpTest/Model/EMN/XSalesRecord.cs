using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AISSystem;


namespace cPlusPlusTest.Models
{
    public class XSalesRecord:XBase 
    { 
        public string PakageName { get; set; }
        public string Buyer { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public DateTime SettleDT { get; set; }
        public Guid? PackageID { get; set; }
        public string BuyerContactUrl { get; set; }

        public Guid GetID()
        {
            return IDHelper.GetGuid(string.Format("{0}{1}{2}",PackageID ,Buyer ,SettleDT.ToDateKey2()));
        }
    }
}
