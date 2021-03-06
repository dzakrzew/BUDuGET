using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUDuGET.Service;

namespace BUDuGET.Model
{
    public class Calendar
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime FocusDateFrom { get; set; }
        public DateTime FocusDateTo { get; set; }
        
        private GoogleCalendarService calendarService;

        public Calendar(string id, string description, GoogleCalendarService _calendarService)
        {
            Id = id;
            Description = description;
            calendarService = _calendarService;
        }

        public string DisplayedName
        {
            get { return string.Format("{0} ({1})", Description, Id);  }
        }

        public List<CalendarEvent> calendarEvents;
        public List<CalendarEvent> CalendarEvents
        {
            get
            {
                return calendarService.FetchEvents(Id, FocusDateFrom, FocusDateTo);
            }
            set
            {
                calendarEvents = value;
            }
        }

        public string FinalBalance
        {
            get
            {
                double sum = 0;

                foreach (CalendarEvent ev in CalendarEvents)
                {
                    sum += ev.Balance;
                }

                return sum.ToString("0.##") + " PLN";
            }
        }

        public string IncomeSum
        {
            get
            {
                double sum = 0;

                foreach (CalendarEvent ev in CalendarEvents)
                {
                    if (ev.Balance >= 0)
                    {
                        sum += ev.Balance;
                    }
                }

                return sum.ToString("0.##") + " PLN";
            }
        }

        public string OutcomeSum
        {
            get
            {
                double sum = 0;

                foreach (CalendarEvent ev in CalendarEvents)
                {
                    if (ev.Balance < 0)
                    {
                        sum += ev.Balance;
                    }
                }

                return sum.ToString("0.##") + " PLN";
            }
        }

        public string HoursSum
        {
            get
            {
                double sum = 0;

                foreach (CalendarEvent ev in CalendarEvents)
                {
                    if (ev.Type == CalendarEventTypeEnum.HourlyEarnings)
                    { 
                        sum += ev.Hours;
                    }
                }

                return sum.ToString();
            }
        }

        public string DailyAverageBalance
        {
            get
            {
                double sum = 0;

                foreach (CalendarEvent ev in CalendarEvents)
                {
                    sum += ev.Balance;
                }

                return (sum / (FocusDateTo - FocusDateFrom).Days).ToString("0.##") + " PLN";
            }
        }
    }
}
