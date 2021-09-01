using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using BUDuGET.Model;
using BUDuGET.View;
using BUDuGET.Service;

namespace BUDuGET.ViewModel
{
    class MainViewModel : BaseClass.ViewModel
    {
        GoogleCalendarService calendarService;

        public MainViewModel()
        {
            availableCalendars = new ObservableCollection<Calendar>();
            ShowPage(AllPages.AuthPage);
        }

        public Calendar selectedCalendar;
        public Calendar SelectedCalendar
        {
            get { return selectedCalendar; }
            set
            {
                selectedCalendar = value;
                onPropertyChange(nameof(SelectedCalendar));
            }
        }

        private ObservableCollection<Calendar> availableCalendars;
        public ObservableCollection<Calendar> AvailableCalendars
        {
            get { return availableCalendars; }
            set
            {
                availableCalendars = value;
                onPropertyChange(nameof(AvailableCalendars));
            }
        }

        private DateTime focusDateFrom;
        public DateTime FocusDateFrom
        {
            get
            {
                return SelectedCalendar is null ? focusDateFrom : SelectedCalendar.FocusDateFrom;
            }
            set
            {
                focusDateFrom = value;
                SelectedCalendar.FocusDateFrom = value;
                onPropertyChange(nameof(focusDateFrom));
                onPropertyChange(nameof(SelectedCalendar));
            }
        }

        private DateTime focusDateTo;
        public DateTime FocusDateTo
        {
            get
            {
                return SelectedCalendar is null ? focusDateTo : SelectedCalendar.FocusDateTo;
            }
            set
            {
                focusDateTo= value;
                SelectedCalendar.FocusDateTo = value;
                onPropertyChange(nameof(focusDateTo));
                onPropertyChange(nameof(SelectedCalendar));
            }
        }


        private Visibility homePageVisibility;
        public Visibility HomePageVisibility
        {
            get { return homePageVisibility; }
            set
            {
                homePageVisibility = value;
                onPropertyChange(nameof(HomePageVisibility));
            }
        }

        private Visibility authPageVisibility;
        public Visibility AuthPageVisibility
        {
            get { return authPageVisibility; }
            set
            {
                authPageVisibility = value;
                onPropertyChange(nameof(AuthPageVisibility));
            }
        }

        private Visibility calendarSelectVisibility = Visibility.Hidden;
        public Visibility CalendarSelectVisibility
        {
            get { return calendarSelectVisibility; }
            set
            {
                calendarSelectVisibility = value;
                onPropertyChange(nameof(CalendarSelectVisibility));
            }
        }

        public void ShowPage(AllPages page)
        {
            if (page == AllPages.HomePage)
            {
                HomePageVisibility = Visibility.Visible;
                AuthPageVisibility = Visibility.Hidden;
            }

            if (page == AllPages.AuthPage)
            {
                HomePageVisibility = Visibility.Hidden;
                AuthPageVisibility = Visibility.Visible;
            }
        }

        private ICommand login;
        public ICommand Login
        {
            get { return login ?? (login = new BaseClass.RelayCommand(onLogin, _ => true)); }
        }

        public void onLogin(object _)
        {
            calendarService = new GoogleCalendarService();
            
            AvailableCalendars.Clear();

            foreach (Model.Calendar calendar in calendarService.GetCalendars())
            {
                AvailableCalendars.Add(calendar);
            }

            CalendarSelectVisibility = Visibility.Visible;
        }

        private ICommand selectCalendar;
        public ICommand SelectCalendar
        {
            get { return selectCalendar ?? (selectCalendar = new BaseClass.RelayCommand(onSelectCalendar, _ => true)); }
        }

        public void onSelectCalendar(object _)
        {
            ShowPage(AllPages.HomePage);
        }
    }
}
