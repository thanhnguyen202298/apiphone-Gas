using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasData
{
     
    public class DateFormatJson : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        public DateFormatJson(string format)
        {
            DateTimeFormat = format;
        }
    }
}
