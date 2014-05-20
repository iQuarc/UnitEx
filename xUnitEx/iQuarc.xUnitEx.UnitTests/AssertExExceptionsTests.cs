using System;
using Xunit;
using Xunit.Sdk;

namespace iQuarc.xUnitEx.UnitTests
{
    public class AssertExExceptionsTests
    {
        [Fact]
        public void ShouldThrow_ActionThrows_AssertionSucceeds()
        {
            Action act = () => { throw new Exception(); };

            act.ShouldThrow();
        }

        [Fact]
        public void ShouldThrow_ActionDoesNotThrow_AssertionFails()
        {
            Action act = () => { };

            try
            {
                act.ShouldThrow();
            }
            catch (AssertException ae)
            {
                Assert.True(ae.UserMessage.Contains("No exception was thrown"),
                            "Assertion failed, but not from expected reason");
                return;
            }

            Assert.True(false, "Assertion did not fail as expected");
        }

        [Fact]
        public void ShouldThrow_ThrowsTheAssertedExceptionType_AssertionSucceeds()
        {
            var someException = new ApplicationException();
            Action act = () => { throw someException; };

            act.ShouldThrow<ApplicationException>();
        }

        [Fact]
        public void ShouldThrow_ThrowsOtherExceptionTypeThenAsserted_AssertionFails()
        {
            Action act = () => { throw new Exception(); };

            try
            {
                act.ShouldThrow<ApplicationException>();
            }
            catch (AssertException ae)
            {
                Assert.True(ae.UserMessage.Contains(" " + typeof (Exception).Name),
                            "Assertion failed, but not from expected reason");
                return;
            }
            Assert.True(false, "Assertion did not fail as expected");
        }


        [Fact]
        public void ShouldThrow_ThrowsInheritedExceptionTypeThenAsserted_AssertionSucceeds()
        {
            Action act = () => { throw new ArgumentNullException(); };

            act.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ShouldThrow_DoesNotThrowAndAssertsOnSpecificExceptionType_AssertionFails()
        {
            Action act = () => { };

            try
            {
                act.ShouldThrow<ApplicationException>();
            }
            catch (AssertException ae)
            {
                Assert.True(ae.UserMessage.Contains("No exception was thrown"),
                            "Assertion failed, but not from expected reason");
                return;
            }

            Assert.True(false, "Assertion did not fail as expected");
        }

        [Fact]
        public void ShouldNotThrow_DoesNotThrow_AssertionSucceeds()
        {
            Action act = () => { };

            act.ShouldNotThrow();
        }

        [Fact]
        public void ShouldNotThrow_ThrowsException_AssertionFails()
        {
            Action act = () => { throw new Exception(); };

            try
            {
                act.ShouldNotThrow();
            }
            catch (AssertException ae)
            {
                Assert.True(ae.UserMessage.Contains("thrown when it shouldn't have been"),
                    "Assertion failed, but not from expected reason");
                return;
            }

            Assert.True(false, "Assertion did not fail as expected");
        }

        [Fact]
        public void ShouldNotThrow_ThrowsInheritedExceptionThanExpected_AssertionFails()
        {
            Action act = () => { throw new ArgumentNullException(); };

            try
            {
                act.ShouldNotThrow<ArgumentException>();
            }
            catch (AssertException ae)
            {
                Assert.True(ae.UserMessage.Contains("ArgumentException"),
                    "Assertion failed, but not from expected reason");
                return;
            }

            Assert.True(false, "Assertion did not fail as expected");
        }

        [Fact]
        public void ShouldNotThrow_ThrowsAnotherTypeOfExceptionThenAsserted_AssertionSucceeds()
        {
            Action act = () => { throw new InvalidOperationException(); };

            act.ShouldNotThrow<ArgumentException>();
        }
    }
}