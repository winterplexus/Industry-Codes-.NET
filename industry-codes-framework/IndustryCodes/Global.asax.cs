//
//  Global.asax.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-2019
//
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: CLSCompliant(true)]

namespace IndustryCodes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}