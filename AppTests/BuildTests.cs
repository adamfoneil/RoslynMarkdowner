﻿using AppTests.Extensions;
using JsonSettings;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDoc.Library.Models;
using RoslynDoc.Library.Services;

namespace AppTests
{
    [TestClass]
    public class BuildTests
    {
        [TestMethod]
        public void BuildDapperCXContent()
        {
            var config = Config.GetConfig();

            var wb = new WikiBuilder();
            var si = JsonFile.Load<SolutionInfo>(@"C:\Users\Adam\AppData\Local\RoslynMarkdowner\Dapper.CX.json");
            var helper = new CSharpMarkdownHelper() { OnlinePath = si.SourceFileBase() };
            wb.Build(@"C:\Users\Adam\Source\Repos\Dapper.CX", si, helper,
                new UsernamePasswordCredentials() 
                { 
                    Username = config["UserName"],
                    Password = config["Password"]
                }, 
                new Identity(config["DisplayName"], config["UserName"]));
        }
    }
}
