using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BUDuGET.Model
{
    public enum CalendarEventTypeEnum
    {
        Income,
        Outcome,
        HourlyEarnings
    }

    public class CalendarEvent
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public CalendarEventTypeEnum Type;
        public double Balance;

        public string DisplayedBalance
        {
            get
            {
                return Balance.ToString() + " PLN";
            }
        }
        
        private void applyFilters()
        {
            string pat = @"(?<type>[\!\+\-])(?<price>\d+(\.\d\d?)?)\s(?<description>.*)";
            Regex re = new Regex(pat);

            Match m = re.Match(Summary);

            if (m.Success) {
                Balance = double.Parse(m.Groups["price"].Value);
                Description = m.Groups["description"].Value;

                string type = m.Groups["type"].Value;

                if (type == "!")
                {
                    Type = CalendarEventTypeEnum.HourlyEarnings;
                }
                else if (type == "+")
                {
                    Type = CalendarEventTypeEnum.Income;
                }
                else if (type == "-")
                {
                    Type = CalendarEventTypeEnum.Outcome;
                    Balance *= -1;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public CalendarEvent(string id, string summary)
        {
            Id = id;
            Summary = summary;
            
            applyFilters();
        }
    }
}
