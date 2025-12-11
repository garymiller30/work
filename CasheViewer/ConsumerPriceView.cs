using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasheViewer
{
    public class ConsumerPriceView
    {
        ConsumerPrice _price;
        public ConsumerPriceView()
        {
            _price = new ConsumerPrice() { dt = DateTime.Now.ToString("yyyyMMdd"), value = 0 };
        }
        public ConsumerPriceView(ConsumerPrice consumerPrice) { 
            _price = consumerPrice;
        }

        public decimal ValueTask {get => 100 + _price.value;}
        public DateTime DateTask { get => DateTime.ParseExact(_price.dt, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture); }

        public decimal OriginalPrice { get;set;}
        public decimal AdjustedPrice { get=> OriginalPrice * ValueTask / 100; }
    }
}
