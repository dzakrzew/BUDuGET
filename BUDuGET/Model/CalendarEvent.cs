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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Hours { get; set; }
        public string Description { get; set; }
        public CalendarEventTypeEnum Type { get; set; }
        public double Price { get; set; }
        public double Balance { get; set; }

        public string DisplayedBalance
        {
            get
            {
                return Balance.ToString("0.##") + " PLN";
            }
        }

        public string DisplayedDate
        {
            get
            {
                string date;

                if (Type == CalendarEventTypeEnum.HourlyEarnings)
                {
                    if (StartTime.ToString("MM/dd/yyyy") == EndTime.ToString("MM/dd/yyyy"))
                    {
                        date = StartTime.ToString("yyyy-MM-dd") + " " + StartTime.ToString("HH:mm") + " - " + EndTime.ToString("HH:mm");
                    }
                    else
                    {
                        date = StartTime.ToString() + " - " + EndTime.ToString();
                    }

                    date += " (" + Hours.ToString() + "h)";
                }
                else
                {
                    date = EndTime.ToString("yyyy-MM-dd HH:mm");
                }

                return date;
            }
        }
        
        private void applyFilters()
        {
            string pat = @"(?<type>[\!\+\-])(?<price>\d+(\.\d\d)?)\s(?<description>.*)";
            Regex re = new Regex(pat);

            Match m = re.Match(Summary);

            if (m.Success) {
                string val = m.Groups["price"].Value.Replace('.', ',');
                double dVal = double.Parse(val, System.Globalization.NumberStyles.Any);

                Balance = dVal;
                Price = dVal;
                Description = m.Groups["description"].Value;

                string type = m.Groups["type"].Value;

                if (type == "!")
                {
                    Type = CalendarEventTypeEnum.HourlyEarnings;
                    Balance *= Hours;
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

        public CalendarEvent(string id, string summary, DateTime? startTime, DateTime? endTime)
        {
            Id = id;
            Summary = summary;

            if (startTime != null) {
                StartTime = startTime.Value;
            }

            if (endTime != null)
            {
                EndTime = endTime.Value;
            }

            if (startTime != null && endTime != null)
            {
                Hours = (EndTime - StartTime).TotalHours;
            }
            
            applyFilters();
        }
    }
}
