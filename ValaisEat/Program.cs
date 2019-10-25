﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DAL;

namespace ConsoleApp
{
    class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Restaurants_DB restaurants_DB = new Restaurants_DB();
            restaurants_DB.Add(new Restaurant());
        }
    }
}
