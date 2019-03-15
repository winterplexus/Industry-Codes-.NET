//
//  FormCollectionHelper.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-2019
//
using System;
using System.Web.Mvc;

namespace IndustryCodes.Utility
{
    public static class FormCollectionHelper
    {
        public static string GetFormStringValue(string name, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }
            var value = collection.Get(name);
            return value ?? string.Empty;
        }

        public static int GetFormNumberValue(string name, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return 0;
            }
            if (!int.TryParse(collection.Get(name), out var value))
            {
                value = 0;
            }
            return value;
        }

        public static double GetFormFloatingPointeValue(string name, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return 0;
            }
            if (!double.TryParse(collection.Get(name), out var value))
            {
                value = 0.0;
            }
            return value;
        }

        public static DateTime GetFormDateTimeValue(string name, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return DateTime.MinValue;
            }
            if (!DateTime.TryParse(collection.Get(name), out var value))
            {
                value = DateTime.MinValue;
            }
            return value;
        }

        public static string GetFormRadioButtonValue(string name, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }
            return collection.Get(name);
        }

        public static bool IsFormButtonSelected(string name, string value, FormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return false;
            }
            var button = collection.Get(name);
            return button != null && button.Equals(value);
        }
    }
}