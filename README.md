#xUnitEx - xUnit extensions
======

A set of extensions to help in unit testing using [xUnit](https://github.com/xunit/xunit "xUnit").

### Assert Equivalent Extensions

A set of extensions for collection tests.

**AssertEx.AreEquivalent<T>(IEnumerable<T> actual, params T[] expected)**

Verifies that the specified collections are equivalent.
Two collections are equivalent if they have the same elements in the same quantity, but in any order.
Elements are equal if their values are equal, not if they refer to the same object.

	IEnumerable<string> e = new List<string> { "a", "b" };
	AssertEx.AreEquivalent(e, "b", "a");


**AssertEx.AreEquivalent<T>(IEnumerable<T> actual, Func<T, T, bool> equality, params T[] expected)**

Verifies that the specified collections are equivalent by using a custom equality function.
Two collections are equivalent if they have the same elements in the same quantity, but in any order.
Elements are equal if their values are equal, not if they refer to the same object.

	IEnumerable<string> e = new List<string> { "a", "b" };
	AssertEx.AreEquivalent(e, (s1, s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase), "A", "B");


### Assert Exception Extensions

A set of extensions for exception tests.

**AssertEx.ShouldThrow(this Action act)**

 Assertion that fails if the specified Action does not throw any exception.

	Action act = () => { throw new Exception(); };
    act.ShouldThrow();

**AssertEx.ShouldThrow<TException>(this Action act)**

 Assertion that fails if the specified Action does not throw the specified type of exception (TException).
 If other types of exceptions are thrown or no exception is thrown, the assert fails.

	ApplicationException ex = new ApplicationException();
    Action act = () => { throw ex; };
    act.ShouldThrow<ApplicationException>();

**AssertEx.ShouldThrow<TException>(this Action act, string message)**

 Assertion that fails with the specified message if the specified Action does not throw the specified type of exception (TException).
 If other types of exceptions are thrown or no exception is thrown, the assert fails.

	ApplicationException ex = new ApplicationException();
    Action act = () => { throw ex; };
    act.ShouldThrow<ApplicationException>("Method did not throw an ApplicationException");

**AssertEx.ShouldNotThrow(this Action act)**

Assertion fails if any type of exception is thrown.

	Action act = () => { };
	act.ShouldNotThrow();

**AssertEx.ShouldNotThrow<TException>(this Action act)**

Assertion that fails if the specified Action throws the specified type of exception (TException).
If other types of exceptions are thrown or no exception is thrown, the assert succeeds.

	Action act = () => { throw new InvalidOperationException(); };
	act.ShouldNotThrow<ArgumentException>();

**AssertEx.ShouldNotThrow<TException>(this Action act, string message)**

Assertion that fails with the specified message if the specified Action throws the specified type of exception (TException) or a derived type.
If other types of exceptions are thrown or no exception is thrown, the assert succeeds.

	Action act = () => { throw new InvalidOperationException(); };
	act.ShouldNotThrow<ArgumentException>("ArgumentException was thrown when it shouldn't have been.");