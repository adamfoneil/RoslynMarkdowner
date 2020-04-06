using MarkdownViewer.App.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AppTests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void SimplifyNamesTest()
        {
            var names = new string[]
            {
                "WorkTracker.Library.Exceptions",
                "WorkTracker.Library",
                "WorkTracker.Library",
                "WorkTracker.Library",
                "WorkTracker.Library.Models",
                "WorkTracker.Test",
                "WorkTracker.Test",
                "WorkTracker.Test",
                "WorkTracker.Test",
                "WorkTracker.SampleWebhook",
                "WorkTracker.SampleWebhook",
                "WorkTracker.SampleWebhook"
            };

            var result = IEnumerableExtensions.SimplifyNames(names);
            var keyPairs = result.Select(kp => kp);

            var expected = new Dictionary<string, string>()
            {
                { "WorkTracker.Library.Exceptions", "Library.Exceptions" },
                { "WorkTracker.Library", "Library" },
                { "WorkTracker.Library.Models", "Library.Models" },
                { "WorkTracker.Test", "Test" },
                { "WorkTracker.SampleWebhook", "SampleWebhook" }
            }.Select(kp => kp);

            Assert.IsTrue(keyPairs.SequenceEqual(expected));
        }

        [TestMethod]
        public void MoreSimplifyNames()
        {
            var names = new string[]
            {
                "Dapper.CX.SqlServer.Abstract",
                "Dapper.CX.SqlServer.Extensions.Int",
                "Dapper.CX.SqlServer.Extensions.Int",
                "Dapper.CX.SqlServer.Extensions.Int",
                "Dapper.CX.SqlServer.Extensions.Long",
                "Dapper.CX.SqlServer.Extensions.Long",
                "Dapper.CX.SqlServer.Extensions.Long",
                "Dapper.CX.SqlServer.Services",
                "Dapper.CX.SqlServer.Services",
                "Dapper.CX.SqlServer",
                "Dapper.CX.SqlServer",
                "Dapper.CX.SqlServer"
            };

            var result = names.SimplifyNames("(base)").Select(kp => kp);

            var expected = new Dictionary<string, string>()
            {
                { "Dapper.CX.SqlServer.Abstract", "Abstract" },
                { "Dapper.CX.SqlServer.Extensions.Int", "Extensions.Int" },
                { "Dapper.CX.SqlServer.Extensions.Long", "Extensions.Long" },
                { "Dapper.CX.SqlServer.Services", "Services" },
                { "Dapper.CX.SqlServer", "(base)" }
            }.Select(kp => kp);

            Assert.IsTrue(result.SequenceEqual(expected));
        }
    }
}
