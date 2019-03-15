//
//  DateTimeFormats.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//
using System;
using System.Globalization;
using System.Text;

namespace IndustryCodes.Utility
{
    public class DateTimeFormats
    {
        private static readonly string[] dowTable =
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };

        private static readonly string[] sdowTable =
        {
            "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat"
        };

        private static readonly string[] monthsTable =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        };

        private static readonly string[] shortMonthsTable =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct","Nov", "Dec"
        };

        private StringCharacterIterator specificationIterator;
        private int specificationIndex;

        public string Format(string specification)
        {
            return Format(specification, DateTime.Now);
        }

        /**
         *  Format output using specification and given date and time.
         *
         *  specification :  %a - abbreviated weekday name
         *                   %A - full weekday name
         *                   %b - abbreviated month name
         *                   %B - full month name
         *                   %c - locale-specific date and time
         *                   %d - day of the month as integer (01-31)
         *                   %H - hour (24-hour clock)
         *                   %I - hour (12-hour clock)
         *                   %j - day of the year as integer (001-366)
         *                   %m - month as integer (01-12)
         *                   %M - minute as integer (00-59)
         *                   %p - locale AM/PM designation
         *                   %S - second as integer (00-59)
         *                   %w - weekday as integer (0-6)
         *                   %W - week number of the year (00-52)
         *                   %x - locale specific date
         *                   %X - locale specific time
         *                   %y - year without century (00-99)
         *                   %Y - year with century
         *                   %Z - time zone name
         *                   %% - a single %
         */
        public string Format(string specification, DateTime dateTime)
        {
            var buffer = new StringBuilder { Length = 0 };

            if (string.IsNullOrEmpty(specification))
            {
                return buffer.ToString();
            }

            SetSpecificationIterator(specification);

            while (specificationIndex < specificationIterator.EndIndex)
            {
                var value = GetNextSpecificationCharacter();
                if (value == '%')
                {
                    value = GetNextSpecificationCharacter();

                    if (value == 'c')
                    {
                        return Format("%a %b %d %H:%M:%S %Y", dateTime);
                    }
                    if (value == 'x')
                    {
                        return Format("%m/%d/%y", dateTime);
                    }
                    if (value == 'X')
                    {
                        return Format("%H:%M:%S", dateTime);
                    }
                    buffer = FormatDateUsingSpecificationValue(value, dateTime, buffer);
                }
                else
                {
                    buffer.Append(value);
                }
            }
            return buffer.ToString();
        }

        private void SetSpecificationIterator(string specification)
        {
            specificationIterator = new StringCharacterIterator(specification);
            specificationIndex = 0;
        }

        private char GetNextSpecificationCharacter()
        {
            specificationIndex++;
            if (specificationIndex <= 1)
            {
                return specificationIterator.First();
            }
            return specificationIndex > specificationIterator.EndIndex ? StringCharacterIterator.Done : specificationIterator.Next();
        }

        private static StringBuilder FormatDateUsingSpecificationValue(char value, DateTime dt, StringBuilder buffer)
        {
            if (value == 'a')
            {
                buffer.Append(sdowTable[(int)dt.DayOfWeek]);
            }
            else if (value == 'A')
            {
                buffer.Append(dowTable[(int)dt.DayOfWeek]);
            }
            else if (value == 'b')
            {
                buffer.Append(shortMonthsTable[dt.Month - 1]);
            }
            else if (value == 'B')
            {
                buffer.Append(monthsTable[dt.Month - 1]);
            }
            else if (value == 'd')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", dt.Day));
            }
            else if (value == 'H')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", dt.Hour));
            }
            else if (value == 'I')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", (dt.Hour > 12) ? dt.Hour - 12 : dt.Hour));
            }
            else if (value == 'j')
            {
                buffer.Append(Convert.ToString(dt.DayOfYear, CultureInfo.CurrentCulture));
            }
            else if (value == 'm')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", dt.Month));
            }
            else if (value == 'M')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", dt.Minute));
            }
            else if (value == 'p')
            {
                buffer.Append((dt.Hour > 12) ? "PM" : "AM");
            }
            else if (value == 'S')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,2:00}", dt.Second));
            }
            else if (value == 'w')
            {
                buffer.Append(Convert.ToString((int)dt.DayOfWeek, CultureInfo.CurrentCulture));
            }
            else if (value == 'W')
            {
                buffer.Append(Convert.ToString(Math.Ceiling(dt.DayOfYear / 7.0), CultureInfo.CurrentCulture));
            }
            else if (value == 'y')
            {
                buffer.Append(FormatShortYear(dt.Year));
            }
            else if (value == 'Y')
            {
                buffer.Append(string.Format(CultureInfo.CurrentCulture, "{0,4:00}", dt.Year));
            }
            else if (value == 'Z')
            {
                buffer.Append(TimeZoneInfo.Local.StandardName);
            }
            else if (value == '%')
            {
                buffer.Append(value);
            }
            else
            {
                throw new ArgumentNullException(nameof(value));
            }
            return buffer;
        }

        private static string FormatShortYear(int year)
        {
            var yearString = Convert.ToInt32(year.ToString(CultureInfo.CurrentCulture).Substring(2, 2), CultureInfo.CurrentCulture);

            return string.Format(CultureInfo.CurrentCulture, "{0,2:00}", Convert.ToInt32(yearString, CultureInfo.CurrentCulture));
        }
    }
}