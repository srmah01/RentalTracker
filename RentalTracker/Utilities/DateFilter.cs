using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalTracker.Utilities
{
    public class DateFilter
    {
        public DateTime? From { get; private set; }
        public DateTime? To { get; private set; }
        public DateFilterSelector Selector { get; private set; }

        public void SetDateFilter(DateFilterSelector selector, string from = null, string to = null)
        {
            Selector = selector;

            switch (Selector)
            {
                case DateFilterSelector.AllDates:
                    From = null;
                    To = null;
                    break;
                case DateFilterSelector.Last3Months:
                    From = DateTime.Today;
                    To = From.Value.AddMonths(-3);
                    break;
                case DateFilterSelector.Last6Months:
                    From = DateTime.Today;
                    To = From.Value.AddMonths(-6);
                    break;
                case DateFilterSelector.Last12Months:
                    From = DateTime.Today;
                    To = From.Value.AddMonths(-12);
                    break;
                default:
                    if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
                    {
                        throw (new ArgumentException());
                    }
                    From = DateTime.Parse("dd/MM/YYYY");
                    To = DateTime.Parse("dd/MM/YYYY");
                    break;
            }
        }
    }
}