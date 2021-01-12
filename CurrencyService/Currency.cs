using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyService
{
    public class Currency
    {
        public string DuoName { get; set; }
        public decimal Value { get; set; }
        public DateTime UpdateDate { get; set; }
        public Currency()
        {

        }
        public Currency(string douName,decimal value,DateTime updateDate)
        {
            douName = DuoName;
            value = Value;
            updateDate = UpdateDate;
        }
    }
}
