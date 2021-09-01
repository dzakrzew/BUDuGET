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

            FocusDateFrom = DateTime.Today;
            FocusDateTo = DateTime.Today;

            FocusDateFrom = FocusDateFrom.AddDays(-1);
        }

        public string DisplayedName
        {
            get { return string.Format("{0} {1}", Id, Description);  }
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

                return sum.ToString() + " PLN";
            }
        }
    }
}
