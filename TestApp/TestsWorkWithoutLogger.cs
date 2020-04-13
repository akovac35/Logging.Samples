// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using NUnit.Framework;
using Shared.Mocks;

namespace TestApp
{
    public class TestsWorkWithoutLogger
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Demonstrate that logging doesn't need to be configured for normal operation of the business logic
            var test = new BusinessLogicMock<object>();
            Assert.AreEqual(test.SecondLevel(123), 123);
            Assert.AreNotEqual(test.SecondLevel(123), 0);
        }
    }
}
