// <copyright file="UnitTest1.cs" company="engi">
// Unit tests written for project 3
// </copyright>

namespace Databases_Project_03.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Databases_Project_03;
    using Assignment3;
 
    /// <summary>
    /// Class for testing comparisons made between an indexed and a normal table.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Compare if results from indexed and non indexed table are equal and greater than zero.
        /// </summary>
        [TestMethod]
        public void ResultsTest()
        {
            Assert.IsTrue(DataAccess.Rowcount(true, false) > 0);
            Assert.IsTrue(DataAccess.Rowcount(false, false) > 0);
            Assert.AreEqual(DataAccess.Rowcount(true, false), DataAccess.Rowcount(false, false));
        }

        /// <summary>
        /// Compare if results from indexed and non indexed table are equal and greater than zero
        /// when "where" condition is called. 
        /// </summary>
        [TestMethod]
        public void WhereResultsTest()
        {
            Assert.IsTrue(DataAccess.Rowcount(true, true) > 0);
            Assert.IsTrue(DataAccess.Rowcount(false, true) > 0);
            Assert.AreEqual(DataAccess.Rowcount(true, true), DataAccess.Rowcount(false, true));  
        }

        /// <summary>
        /// Check if indexed query is faster than non indexed.
        /// </summary>
        [TestMethod]
        public void PerformanceTest()
        {
            Assert.IsTrue(DataAccess.Timing(false, true, true) > DataAccess.Timing(indexed: true, true, true));  
        }

        /// <summary>
        /// Check which method (sequential scan or index) was used when queries were called. 
        /// </summary>
        [TestMethod]
        public void ScanUsedTest() 
        {
            Assert.IsTrue(DataAccess.explainAnalyze(false).Contains("Seq Scan"));
            Assert.IsTrue(DataAccess.explainAnalyze(true).Contains("Index Scan"));
        }       
    }
}
