using System;
using Xunit;

namespace iQuarc.xUnitEx
{
    public static partial class AssertEx
    {
        /// <summary>
        ///     Assertion that fails if the specified Action does not throw any exception.
        /// </summary>
        public static void ShouldThrow(this Action act)
        {
            ShouldThrow<Exception>(act);
        }

        /// <summary>
        ///     Assertion that fails if the specified Action does not throw the specified type of exception (TException).
        ///     If other types of exceptions are thrown or no exception is thrown, the assert succeeds.
        /// </summary>
        public static void ShouldThrow<TException>(this Action act)
            where TException : Exception
        {
            string message = String.Empty;
            ShouldThrow<TException>(act, message);
        }

        /// <summary>
        ///     Assertion that fails with the specified message if the specified Action throws the specified type of exception (TException).
        ///     If other types of exceptions are thrown or no exception is thrown, the assert succeeds.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ShouldThrow<TException>(this Action act, string message)
            where TException : Exception
        {
            try
            {
                act();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(message))
                    message = "Exception was thrown but it was not of the expected type: " + ex.GetType().Name + " - " + ex.Message;
                Assert.True(false, message);
            }

            if (string.IsNullOrEmpty(message))
                message = "No exception was thrown.";
            Assert.True(false, message);
        }

        /// <summary>
        /// Assertion fails if any type of exception is thrown
        /// </summary>
        public static void ShouldNotThrow(this Action act)
        {
            Exception exception = TryExec(act);
            if (exception != null)
                Assert.True(false, string.Format("Exception of type {0} was thrown when it shouldn't have been.", exception.GetType()));
        }

        /// <summary>
        ///     Assertion that fails if the specified Action throws the specified type of exception (TException).
        ///     If other types of exceptions are thrown or no exception is thrown, the assert succeeds.
        /// </summary>
        public static void ShouldNotThrow<TException>(this Action act)
            where TException : Exception
        {
            string message = "Exception of type " + typeof (TException) + " was thrown when it shouldn't have been.";
            ShouldNotThrow<TException>(act, message);
        }

        /// <summary>
        ///     Assertion that fails with the specified message if the specified Action throws the specified type of exception (TException) or a derived type.
        ///     If other types of exceptions are thrown or no exception is thrown, the assert succeeds.
        /// </summary>
        public static void ShouldNotThrow<TException>(this Action act, string message)
            where TException : Exception
        {
            if (TryExec(act) is TException)
                Assert.True(false, message);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Exception TryExec(Action act)
        {
            try
            {
                act();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}