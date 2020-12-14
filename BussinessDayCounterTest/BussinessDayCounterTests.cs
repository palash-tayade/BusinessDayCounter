using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BusinessDayCounter;
using System.Collections.Generic;

namespace BussinessDayCounterTest
{
    [TestClass]
    public class BussinessDayCounterTests
    {
        [TestMethod]
        public void Get_Weekdays_Between_Two_Dates_Task1()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 6, 1);
            DateTime secondDate = new DateTime(2021, 6, 21);
            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate);

            // Assert
            Assert.AreEqual(13, weekdays);
        }

        [TestMethod]
        public void Get_Bussiness_Days_Between_Two_Dates_Task2()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 12, 20);
            DateTime secondDate = new DateTime(2022, 1, 5);
            var publicHolidays = new List<DateTime>();

            publicHolidays.Add(new DateTime(2021, 12, 25));
            publicHolidays.Add(new DateTime(2021, 12, 26));
            publicHolidays.Add(new DateTime(2022, 1, 1));

            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.BussinessDaysBetweenTwoDates(firstDate, secondDate, publicHolidays);

            // Assert
            Assert.AreEqual(11, weekdays);
        }


        [TestMethod]
        public void Get_Bussiness_Days_Between_Two_Dates_Task3()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 12, 20);
            DateTime secondDate = new DateTime(2022, 1, 5);
            var holidays = new List<Holiday>();

            // New Year's Day
            holidays.Add(new MonthDayBasedHoliday(1, 1));

            // Queen's Birthday
            holidays.Add(new DayOfWeekBasedHoliday(2, DayOfWeek.Monday, 6));

            // Christmas Day
            holidays.Add(new MonthDayBasedHoliday(12, 25));

            // Boxing Day
            holidays.Add(new MonthDayBasedHoliday(12, 26));
            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.BussinessDaysBetweenTwoDates(firstDate, secondDate,holidays);

            // Assert
            Assert.AreEqual(9, weekdays);
        }

        [TestMethod]
        public void Get_Weekdays_Between_Two_Dates_When_Start_Date_Is_Greater_Than_End_Date_Task1()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 6, 21);
            DateTime secondDate = new DateTime(2021, 6, 1);
            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.WeekdaysBetweenTwoDates(firstDate, secondDate);

            // Assert
            Assert.AreEqual(0, weekdays);
        }

        [TestMethod]
        public void Get_Bussiness_Days_Between_Two_Dates_When_Start_Date_Is_Greater_Than_End_Date_Task2()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 12, 20);
            DateTime secondDate = new DateTime(2021, 12, 19);
            var publicHolidays = new List<DateTime>();

            publicHolidays.Add(new DateTime(2021, 12, 25));
            publicHolidays.Add(new DateTime(2021, 12, 26));
            publicHolidays.Add(new DateTime(2022, 1, 1));

            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.BussinessDaysBetweenTwoDates(firstDate, secondDate, publicHolidays);

            // Assert
            Assert.AreEqual(0, weekdays);
        }


        [TestMethod]
        public void Get_Bussiness_Days_Between_Two_Dates_When_Start_Date_Is_Greater_Than_End_Date_Task3()
        {
            // Arrange
            DateTime firstDate = new DateTime(2021, 12, 20);
            DateTime secondDate = new DateTime(2021, 12, 20);
            var holidays = new List<Holiday>();

            // New Year's Day
            holidays.Add(new MonthDayBasedHoliday(1, 1));

            // Queen's Birthday
            holidays.Add(new DayOfWeekBasedHoliday(2, DayOfWeek.Monday, 6));

            // Christmas Day
            holidays.Add(new MonthDayBasedHoliday(12, 25));

            // Boxing Day
            holidays.Add(new MonthDayBasedHoliday(12, 26));
            BusinessDayCounterChallenge weekdayCounter = new BusinessDayCounterChallenge();

            // Act
            int weekdays = weekdayCounter.BussinessDaysBetweenTwoDates(firstDate, secondDate, holidays);

            // Assert
            Assert.AreEqual(0, weekdays);
        }
    }
}
