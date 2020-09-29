using System;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace FrameworkExtentTests
{
    [TestFixture]
    public class ExtentTests
    {
        private ExtentReports _extent;
        private ExtentHtmlReporter _html;

        private static readonly string AssemblyPath =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string ResourcePath =
            Path.Combine(AssemblyPath, "Resources");
        private static string ReportPath =>
            Path.Combine(AssemblyPath, "Report", TestContext.CurrentContext.Test.MethodName);

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _html = new ExtentHtmlReporter(
                Path.Combine(ReportPath, DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")) + "\\");
        }

        [Test]
        public void BasicTestWithInMemoryConfig()
        {
            _html.Config.JS = File.ReadAllText(Path.Combine(ResourcePath, "Extent.js"));
            _html.Config.DocumentTitle = TestContext.CurrentContext.Test.MethodName;
            _html.Config.ReportName = "My Report";

            GenerateBasicTest();
        }

        [Test]
        public void BasicTestWithXmlConfig()
        {
            _html.LoadConfig(Path.Combine(ResourcePath, "html-config.xml"));

            GenerateBasicTest();
        }

        private void GenerateBasicTest()
        {
            _extent.AttachReporter(_html);

            var feature = _extent.CreateTest<Feature>("Some Feature");
            var scenario = feature.CreateNode<Scenario>("Some Test");
            var step = scenario.CreateNode(new GherkinKeyword("Given"), "Some Step");
            step.Pass("Some info");
        }

        [TearDown]
        public void TearDown()
        {
            _extent.Flush();
        }
    }
}