//
//  Startup.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-2019
//            
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IndustryCodes.Startup))]

namespace IndustryCodes
{
    public partial class Startup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "app"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void Configuration(IAppBuilder app)
        {
        }
    }
}                       