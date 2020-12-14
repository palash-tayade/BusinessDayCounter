using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BusinessDayCounter
{
    public class BusinessDayCounterChallenge
    {
        /// <summary>
        /// Task 1 - Returns number of Weekdays between two dates
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public int WeekdaysBetweenTwoDates(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart >= dateEnd)
            {
                return 0;
            }
            return Convert.ToInt16(GetTotalDaysExclusive(dateStart, dateEnd));
        }

        /// <summary>
        /// Task 2 - Returns number of bussiness days between two dates. This function does not caters for public holidays falling on weekend days.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="publicHolidays"></param>
        /// <returns></returns>
        public int BussinessDaysBetweenTwoDates(DateTime dateStart, DateTime dateEnd, IList<DateTime> publicHolidays)
        {
            if (dateStart >= dateEnd)
            {
                return 0;
            }
            return GetTotalBussinessDaysExclusive(dateStart, dateEnd, publicHolidays);
        }

        /// <summary>
        /// Task 3 - Returns number of bussiness days between two dates. This function caters for public holidays falling on weekend days.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="publicHolidays"></param>
        /// <returns></returns>
        public int BussinessDaysBetweenTwoDates(DateTime dateStart, DateTime dateEnd, IList<Holiday> publicHolidays)
        {
            if (dateStart >= dateEnd)
            {
                return 0;
            }
            return GetTotalBussinessDaysExclusive(dateStart, dateEnd, publicHolidays);
        }



        private int GetTotalDaysExclusive(DateTime startDate, DateTime endDate)
        {
            //skip the start date
            startDate = startDate.AddDays(1);

            double intermediateDays = 0;

            //Move the start date to next monday to get exact number of weekends
            while (startDate.DayOfWeek != DayOfWeek.Monday && startDate <= endDate.AddDays(-1))
            {
                if (!(startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday))
                {
                    intermediateDays++;
                }
                startDate = startDate.AddDays(1);
            }

            var timeBetween = (endDate - startDate).TotalDays;
            int numberOf7DayWeeks = (int)(timeBetween / 7);
            int numberOfWeekendDays = (numberOf7DayWeeks) * 2;
            int workingDays = (int)(timeBetween - numberOfWeekendDays);

            workingDays = endDate.DayOfWeek == DayOfWeek.Sunday ? workingDays - 1 : workingDays;

            return Convert.ToInt16(workingDays + intermediateDays);
        }

        private int GetTotalBussinessDaysExclusive(DateTime startDate, DateTime endDate, IList<DateTime> publicHolidays)
        {
            int totalWeekDays = GetTotalDaysExclusive(startDate, endDate);
            var weekendDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            foreach (var holiday in publicHolidays)
            {
                if (startDate < holiday && holiday < endDate && !weekendDays.Contains(holiday.DayOfWeek))
                {
                    totalWeekDays--;
                }
            }
            return totalWeekDays;
        }

        internal int GetTotalBussinessDaysExclusive(DateTime startDate, DateTime endDate, IList<Holiday> publicHolidays)
        {
            int totalWeekDays = GetTotalDaysExclusive(startDate, endDate);
            var weekendDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
            IList<DateTime> amendedPublicHolidays = new List<DateTime>();

            //Case when public holiday falls on weekend
            foreach (var holiday in publicHolidays)
            {
                amendedPublicHolidays.Add(GetNextNonHolidayWeekDay(holiday.GetDate(startDate.Year).GetValueOrDefault(), publicHolidays, weekendDays));
            }

            foreach (var holiday in amendedPublicHolidays)
            {
                if (startDate < holiday && holiday < endDate && !weekendDays.Contains(holiday.DayOfWeek))
                {
                    totalWeekDays--;
                }
            }
            return totalWeekDays;
        }

        public static DateTime GetNextNonHolidayWeekDay(DateTime date, IList<Holiday> holidays, IList<DayOfWeek> weekendDays)
        {
            date = date.Date.AddDays(1);

            // calculate holidays for both this year and next year
            var holidayDates = holidays.Select(x => x.GetDate(date.Year))
                .Union(holidays.Select(x => x.GetDate(date.Year + 1)))
                .Where(x => x != null)
                .Select(x => x.Value)
                .OrderBy(x => x).ToArray();

            // increment until we get a weekday date
            while (true)
            {
                if (weekendDays.Contains(date.DayOfWeek) || holidayDates.Contains(date))
                    date = date.AddDays(1);
                else
                    return date;
            }
        }

    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            BusinessDayCounterChallenge dayCounter = new BusinessDayCounterChallenge();
            DateTime firstDate = new DateTime(2021, 6, 1);
            DateTime secondDate = new DateTime(2021, 6, 21);

            Console.WriteLine("Task 1: " + dayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate));

            var publicHolidays = new List<DateTime>();
            publicHolidays.Add(new DateTime(2021, 12, 25));
            publicHolidays.Add(new DateTime(2021, 12, 26));
            publicHolidays.Add(new DateTime(2022, 1, 1));

            Console.WriteLine("Task 2 : " + dayCounter.BussinessDaysBetweenTwoDates(firstDate, secondDate, publicHolidays));

            var holidays = new List<Holiday>();

            // New Year's Day
            holidays.Add(new MonthDayBasedHoliday(1, 1));

            // Queen's Birthday
            holidays.Add(new DayOfWeekBasedHoliday(2, DayOfWeek.Monday, 6));

            // Christmas Day
            holidays.Add(new MonthDayBasedHoliday(12, 25));

            // Boxing Day
            holidays.Add(new MonthDayBasedHoliday(12, 26));

            Console.WriteLine("Task 3: " + dayCounter.GetTotalBussinessDaysExclusive(firstDate, secondDate, holidays));

        }
    }
}
