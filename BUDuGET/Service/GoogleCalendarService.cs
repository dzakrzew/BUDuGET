
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using BUDuGET.Model;

namespace BUDuGET.Service
{
    public class GoogleCalendarService
    {
        private UserCredential credential;
        private string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private string ApplicationName = "BUDuGET";
        private CalendarService calendarService;

        public GoogleCalendarService()
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                ).Result;
            }

            calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public List<Model.Calendar> GetCalendars()
        {
            CalendarListResource.ListRequest listRequest = calendarService.CalendarList.List();
            CalendarList calendarList = listRequest.Execute();

            List<Model.Calendar> calendars = new List<Model.Calendar>();

            foreach (CalendarListEntry entry in calendarList.Items)
            {
                calendars.Add(new Model.Calendar(entry.Id, entry.Description, this));
            }

            return calendars;
        }

        public List<CalendarEvent> FetchEvents(string calendarId, DateTime dateFrom, DateTime dateTo)
        {
            EventsResource.ListRequest request = calendarService.Events.List(calendarId);
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.TimeMin = dateFrom;
            request.TimeMax = dateTo;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            Events events = request.Execute();

            foreach (Event ev in events.Items)
            {
                if (ev.Summary != null && (ev.Summary.StartsWith("!") || ev.Summary.StartsWith("+") || ev.Summary.StartsWith("-")))
                {
                    calendarEvents.Add(new CalendarEvent(ev.Id, ev.Summary));
                }
            }
            
            return calendarEvents;
        }
    }
}
