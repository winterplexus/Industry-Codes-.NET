//
//  FilterConfig.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.Web.Mvc;

namespace IndustryCodes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters?.Add(new HandleErrorAttribute());
        }
    }
}