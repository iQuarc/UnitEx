using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace iQuarc.xUnitEx.UnitTests
{
    public class AssertExEquivalentTests
    {
        [Fact]
        public void AreEquivalent_DistinctElementsReverseOrder_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray("a", "b");

            Action act = () => AssertEx.AreEquivalent(e, "b", "a");

            act.ShouldNotThrow();
        }

        [Fact]
        public void AreEquivalent_SameElementsSameLength_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray("a", "a");

            Action act = () => AssertEx.AreEquivalent(e, "a", "a");

            act.ShouldNotThrow();
        }

        [Fact]
        public void AreEquivalent_SameElementsDifferentLength_AssertFails()
        {
            IEnumerable<string> e = YieldArray("a", "a", "a");

            Action act = () => AssertEx.AreEquivalent(e, "a", "a");

            act.ShouldThrow<AssertException>();
        }

        [Fact]
        public void AreEquivalent_EnumerableWithDistinctElementsArrayWithSameElements_AssertFails()
        {
            IEnumerable<string> e = YieldArray("a", "b");

            Action act = () => AssertEx.AreEquivalent(e, "a", "a");

            act.ShouldThrow<AssertException>();
        }

        [Fact]
        public void AreEquivalent_ArrayWithDistinctElementsEnumerableWithSameElements_AssertFails()
        {
            IEnumerable<string> e = YieldArray("a", "a");

            Action act = () => AssertEx.AreEquivalent(e, "a", "b");

            act.ShouldThrow<AssertException>();
        }

        [Fact]
        public void AreEquivalent_SameLengthOneElementDiffers_AssertFails()
        {
            IEnumerable<string> e = YieldArray("a", "b", "c");

            Action act = () => AssertEx.AreEquivalent(e, "a", "b", "d");

            act.ShouldThrow<AssertException>();
        }

        [Fact]
        public void AreEquivalent_EachHasOneNull_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray("a", "b", null);

            Action act = () => AssertEx.AreEquivalent(e, "a", null, "b");

            act.ShouldNotThrow();
        }

        [Fact]
        public void AreEquivalent_ArrayHasNullAndCollectionDoesNot_AssertFail()
        {
            IEnumerable<string> e = YieldArray("a", "b", "c");

            Action act = () => AssertEx.AreEquivalent(e, "a", "b", null);

            act.ShouldThrow<AssertException>();
        }

        [Fact]
        public void AreEquivalent_CustomEquality_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray("ab");

            Action act = () => AssertEx.AreEquivalent(e, ((s1, s2) => s1[0] == s2[0]), "abc");

            act.ShouldNotThrow();
        }

        [Fact]
        public void AreEquivalent_CustomEqualityWithNull_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray("ab", null);

            Func<string, string, bool> equality = (s1, s2) => 
                (s1==null && s2==null) 
                ||
                (s1 !=null && s2!=null && s1[0] == s2[0]);


            Action act = () => AssertEx.AreEquivalent(e, equality, null, "abc");

            act.ShouldNotThrow();
        }

        [Fact]
        public void AreEquivalent_BothEmpty_AssertSuccess()
        {
            IEnumerable<string> e = YieldArray(new string[] {});

            Action act = () => AssertEx.AreEquivalent(e, new string[] {});

            act.ShouldNotThrow();
        }
     
        private IEnumerable<T> YieldArray<T>(params T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                yield return elements[i];
            }
        }
    }
}