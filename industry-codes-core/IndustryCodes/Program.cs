//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IndustryCodes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .Build();
    }
}