using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Test
{
    [TestClass]
    public class PreconditionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullTest_Null()
        {
            Preconditions.NotNull(null);
        }

        [TestMethod]
        public void NotNullTest_NotNull()
        {
            Preconditions.NotNull("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullOrWhiteSpaceTest_Null()
        {
            Preconditions.NotNullOrWhiteSpace(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullOrWhiteSpaceTest_Empty()
        {
            Preconditions.NotNullOrWhiteSpace("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullOrWhiteSpaceTest_WhiteSpace()
        {
            Preconditions.NotNullOrWhiteSpace("  ");
        }
    }
}
