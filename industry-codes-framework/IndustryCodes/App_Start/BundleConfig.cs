﻿//
//  BundleConfig.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.Web.Optimization;

namespace IndustryCodes
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (bundles != null)
            {
                bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
                bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
                bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
                bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));
            }
        }
    }
}