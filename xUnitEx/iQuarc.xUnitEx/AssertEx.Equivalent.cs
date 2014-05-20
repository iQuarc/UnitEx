using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace iQuarc.xUnitEx
{
    public static partial class AssertEx
    {
        /// <summary>
        ///     Verifies that the specified collections are equivalent.
        ///     Two collections are equivalent if they have the same elements in the same quantity, but in any order.
        ///     Elements are equal if their values are equal, not if they refer to the same object.
        /// </summary>
        public static void AreEquivalent<T>(IEnumerable<T> actual, params T[] expected)
        {
            Func<T, T, bool> equality = EqualityComparer<T>.Default.Equals;
            AreEquivalent(actual, equality, expected);
        }

        /// <summary>
        ///     Verifies that the specified collections are equivalent.
        ///     Two collections are equivalent if they have the same elements in the same quantity, but in any order.
        /// </summary>
        public static void AreEquivalent<T>(IEnumerable<T> actual, Func<T, T, bool> equality, params T[] expected)
        {
            Assert.True(actual.Count() == expected.Length,
                        string.Format("Collections do not have same number of elements. Expected: {0}; Actual: {1}", expected.Length, actual.Count()));

            if (expected.Length == 0)
                return;


            int expectedCount;
            int actualCount;
            T mismatchedElement;
            bool isMismatch = FindMismatchedElement(actual, expected, equality, out expectedCount, out actualCount, out mismatchedElement);

            Assert.False(isMismatch,
                         string.Format("Collections are not equivalent. Mismatch element {0}. Expected count: {1}; Actual: {2}",
                                       mismatchedElement, expectedCount, actualCount));
        }

        private static bool FindMismatchedElement<T>(IEnumerable<T> actual, T[] expected, Func<T, T, bool> equality, out int expectedCount, out int actualCount,
                                                     out T mismatchedElement)
        {
            List<ElementCount<T>> expectedElementCounts = GetElementCounts(expected, equality);
            List<ElementCount<T>> actualElementCounts = GetElementCounts(actual, equality);
            foreach (ElementCount<T> expectedElement in expectedElementCounts)
            {
                ElementCount<T> actualElement = actualElementCounts.Find(e => equality(e.Element, expectedElement.Element));

                if (actualElement == null)
                {
                    expectedCount = expectedElement.Count;
                    actualCount = 0;
                    mismatchedElement = expectedElement.Element;
                    return true;
                }

                if (expectedElement.Count != actualElement.Count)
                {
                    expectedCount = expectedElement.Count;
                    actualCount = actualElement.Count;
                    mismatchedElement = expectedElement.Element;
                    return true;
                }
            }

            expectedCount = 0;
            actualCount = 0;
            mismatchedElement = default(T);
            return false;
        }

        private static List<ElementCount<T>> GetElementCounts<T>(IEnumerable<T> collection, Func<T, T, bool> equality)
        {
            List<ElementCount<T>> elementCounts = new List<ElementCount<T>>();
            foreach (T element in collection)
            {
                ElementCount<T> count = elementCounts.Find(p => equality(p.Element, element));
                if (count == null)
                    elementCounts.Add(new ElementCount<T>(element));
                else
                    count.Increment();
            }
            return elementCounts;
        }

        private class ElementCount<T>
        {
            public ElementCount(T element)
            {
                Element = element;
                Count = 1;
            }

            public T Element { get; private set; }
            public int Count { get; private set; }

            public void Increment()
            {
                Count++;
            }
        }
    }
}