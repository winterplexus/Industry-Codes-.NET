//
//  FormCollectionAssistant.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System;
using Microsoft.AspNetCore.Http;

namespace IndustryCodes.Utility
{
    public static class FormCollectionAssistant
    {
        public static string GetFormStringValue(string name, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }
            var value = (string)collection[name];
            return value ?? string.Empty;
        }

        public static int GetFormNumberValue(string name, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return 0;
            }
            if (!int.TryParse(collection[name], out var value))
            {
                value = 0;
            }
            return value;
        }

        public static double GetFormFloatingPointValue(string name, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return 0;
            }
            if (!double.TryParse(collection[name], out var value))
            {
                value = 0.0;
            }
            return value;
        }

        public static DateTime GetFormDateTimeValue(string name, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return DateTime.MinValue;
            }
            if (!DateTime.TryParse(collection[name], out var value))
            {
                value = DateTime.MinValue;
            }
            return value;
        }

        public static string GetFormRadioButtonValue(string name, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }
            return collection[name];
        }

        public static bool IsFormButtonSelected(string name, string value, IFormCollection collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return false;
            }
            var button = (string)collection[name];
            return button != null && button.Equals(value);
        }
    }
}