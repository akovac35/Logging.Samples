// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Mocks;
using System;

namespace TestApp
{
    [TestFixture]
    public class TestLoggingExamples
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            customOnWrite = writeContext => {
                Console.WriteLine(writeContext);
            };

            customOnBeginScope = scopeContext => {
                Console.WriteLine(scopeContext);
            };

            serviceCollection = new ServiceCollection();
            // Register TestLogger using extension method (uses TryAdd),
            // always register it as the first service for reliable registration
            serviceCollection.AddTestLogger(onWrite: customOnWrite, onBeginScope: customOnBeginScope);
            serviceCollection.AddTransient(typeof(BusinessLogicMock<>));
        }

        [SetUp]
        public void SetUp()
        {
        }

        private IServiceCollection serviceCollection;

        private Action<WriteContext> customOnWrite;
        private Action<ScopeContext> customOnBeginScope;

        [Test]
        public void Test_WithoutLoggers_Works()
        {
            // Demonstrate that logging doesn't need to be configured for normal operation of the business logic if it is properly designed
            var test = new BusinessLogicMock<object>();
            Assert.AreEqual(test.SecondLevel(123), 123);
            Assert.AreNotEqual(test.SecondLevel(123), 0);
        }

        [Test]
        public void Test_WithLoggingToTestConsole_Works()
        {
            // The service provider should be defined on per-test level or logger writes will accumulate and may result in OOM - clean them with testSink.Clear()
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var blm = serviceProvider.GetRequiredService<BusinessLogicMock<object>>();
            blm.FirstLevel();

            var testSink = serviceProvider.GetRequiredService<ITestSink>();

            Assert.IsTrue(testSink.Writes.Count > 0);
            Assert.IsTrue(testSink.Scopes.Count > 0);
        }
    }
}
