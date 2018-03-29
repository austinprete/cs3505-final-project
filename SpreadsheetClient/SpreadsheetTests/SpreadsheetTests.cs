// Austin Prete
// October 6, 2017
// CS 3500 - Fall 2017

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace SS.Tests
{
    [TestClass()]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests that GetCellContents() works as expected under normal conditions
        /// </summary>
        [TestMethod()]
        public void GetCellContentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            // Tests GetCellContents() with empty value
            Assert.AreEqual("", sheet.GetCellContents("a1"));

            // Tests GetCellContents() with double value
            double expectedDoubleContent = 5.0;
            sheet.SetContentsOfCell("a1", expectedDoubleContent.ToString());
            Assert.AreEqual(expectedDoubleContent, sheet.GetCellContents("a1"));

            // Tests GetCellContents() with Formula value
            var formula = "=5 + 4";
            sheet.SetContentsOfCell("a1", formula);
            Assert.AreEqual(new Formula(formula.Substring(1)), sheet.GetCellContents("a1"));

            // Tests GetCellContents() with string value
            string expectedStringContent = "horse";
            sheet.SetContentsOfCell("a1", expectedStringContent);
            Assert.AreEqual(expectedStringContent, sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests that GetCellContents() throws an InvalidNameException when provided
        /// an invalid cell name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("1X");
        }

        /// <summary>
        /// Tests that GetCellContents() throws an InvalidNameException when provided
        /// a null value as its `name` argument
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNullNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Tests that GetNamesOfAllEmptyCells() works properly and recognizes changes
        /// from empty to non-empty values and vice-versa
        /// </summary>
        [TestMethod()]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "dog");
            sheet.SetContentsOfCell("C4", "5.0");
            sheet.SetContentsOfCell("B6", "=5");

            Assert.IsTrue(new HashSet<string> { "a1", "C4", "B6" }.SetEquals(sheet.GetNamesOfAllNonemptyCells()));

            // If we set a cell to be empty again, it should no longer show up in the list of non-empty cells
            sheet.SetContentsOfCell("a1", "");
            Assert.IsTrue(new HashSet<string> { "C4", "B6" }.SetEquals(sheet.GetNamesOfAllNonemptyCells()));

            // Even whitespace counts as non-empty, so a1 should show up in the results again
            sheet.SetContentsOfCell("a1", " ");
            Assert.IsTrue(new HashSet<string> { "a1", "C4", "B6" }.SetEquals(sheet.GetNamesOfAllNonemptyCells()));
        }

        /// <summary>
        /// Tests that the Double overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided an invalid cell name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellDoubleInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            Double content = 5.0;
            sheet.SetContentsOfCell("21X", content.ToString());
        }

        /// <summary>
        /// Tests that the String overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided an invalid cell name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellStringInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            String content = "monkey";
            sheet.SetContentsOfCell("-dog", content);
        }

        /// <summary>
        /// Tests that the Formula overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided an invalid cell name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellFormulaInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            string formula = "=x + y";
            sheet.SetContentsOfCell("420", formula);
        }

        /// <summary>
        /// Tests that the Double overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided a null value for its `name` argument
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellDoubleNullNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            double contents = 6.0;

            sheet.SetContentsOfCell(null, contents.ToString());
        }

        /// <summary>
        /// Tests that the String overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided a null value for its `name` argument
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellStringNullNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            string contents = "horse";

            sheet.SetContentsOfCell(null, contents);
        }

        /// <summary>
        /// Tests that the Formula overload of SetContentsOfCell() throws an InvalidNameException
        /// when provided a null value for its `name` argument
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellFormulaNullNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            String formula = "=x + 6";

            sheet.SetContentsOfCell(null, formula);
        }

        /// <summary>
        /// Tests that the String overload of SetContentsOfCell() throws an ArgumentNullException
        /// when provided a null value for its `text` argument
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellNullContentTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            String content = null;

            sheet.SetContentsOfCell("a1", content);
        }

        /// <summary>
        /// Tests that all of the correct dependent cells are returned by SetContentsOfCell()
        /// </summary>
        [TestMethod()]
        public void SetContentsOfCellWithIndirectDependentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("b2", "=5 + a1");
            var cellsToRecalculate = sheet.SetContentsOfCell("a1", "2.0");
            Assert.IsTrue(new HashSet<string> { "a1", "b2" }.SetEquals(cellsToRecalculate));

            sheet.SetContentsOfCell("Z42", "=b2 * 432");
            cellsToRecalculate = sheet.SetContentsOfCell("a1", "2.0");
            Assert.IsTrue(new HashSet<string> { "a1", "b2", "Z42" }.SetEquals(cellsToRecalculate));

            sheet.SetContentsOfCell("ac23", "=a1 / b2");
            cellsToRecalculate = sheet.SetContentsOfCell("a1", "2.0");
            Assert.IsTrue(new HashSet<string> { "a1", "b2", "Z42", "ac23" }.SetEquals(cellsToRecalculate));

            sheet.SetContentsOfCell("VAR2", "=5 + Z42");
            cellsToRecalculate = sheet.SetContentsOfCell("a1", "2.0");
            Assert.IsTrue(new HashSet<string> { "a1", "b2", "Z42", "VAR2", "ac23" }.SetEquals(cellsToRecalculate));

            sheet.SetContentsOfCell("b2", "5");
            cellsToRecalculate = sheet.SetContentsOfCell("a1", "2");
            Assert.IsTrue(new HashSet<string> { "a1", "ac23" }.SetEquals(cellsToRecalculate));

            sheet.SetContentsOfCell("b2", "=50 * (a1)");
            cellsToRecalculate = sheet.SetContentsOfCell("a1", "2.0");
            Assert.IsTrue(new HashSet<string> { "a1", "b2", "Z42", "VAR2", "ac23" }.SetEquals(cellsToRecalculate));
        }

        /// <summary>
        /// Tests that a CircularException is throw once the spreadsheet reaches a state
        /// where it contains a circular dependency between cells.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellCircularDependencyTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "=B1 * 2");
            sheet.SetContentsOfCell("B1", "=c3 / 4");
            sheet.SetContentsOfCell("c3", "=a1 - 6");
        }

        /// <summary>
        /// Test that GetSavedVersion() works properly on a correctly
        /// formatted XML file.
        /// </summary>
        [TestMethod()]
        public void GetSavedVersionTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.AreEqual("version1", sheet.GetSavedVersion("Resources/test.xml"));
        }

        /// <summary>
        /// Tests that GetSavedVersion() throws an exception if the XML
        /// file doesn't contain version information for the spreadsheet.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionWithNoVersionInfoTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            var version = sheet.GetSavedVersion("Resources/no_version.xml");
            Console.WriteLine(version);
        }

        /// <summary>
        /// Tests the GetCellValue() method returns the expected results
        /// for both set and empty cells.
        /// </summary>
        [TestMethod()]
        public void GetCellValueTest()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("B2", "5.0");
            sheet.SetContentsOfCell("C3", "3290");
            sheet.SetContentsOfCell("D4", "dog");
            sheet.SetContentsOfCell("A1", "=B2 * C3 / 2");

            Assert.AreEqual(8225.0, sheet.GetCellValue("A1"));
            Assert.AreEqual(5.0, sheet.GetCellValue("B2"));
            Assert.AreEqual(3290.0, sheet.GetCellValue("C3"));
            Assert.AreEqual("dog", sheet.GetCellValue("D4"));

            // Test that an empty string is returned for an empty cell
            Assert.AreEqual("", sheet.GetCellValue("E5"));
        }

        /// <summary>
        /// A stress test that creates a chain of dependent cells
        /// and then retrieves the value of the final cell in the
        /// chain. This stress tests the resolving behavior of
        /// values within formulas.
        /// </summary>
        [TestMethod()]
        public void StressTest1()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A0", "2.0");

            int max = 25_000;

            // Creates cells A0 - A[MAX]
            for (int counter = 1; counter <= max; counter++)
            {
                string formula = String.Format("=A{0}", counter - 1);

                sheet.SetContentsOfCell(String.Format("A{0}", counter), formula);
            }

            // By retrieving the value of the max cell, all the other cells
            // values will need to be resolved as well.
            var value = sheet.GetCellValue(String.Format("A{0}", max));
        }

        /// <summary>
        /// Similar to the above stress test, but uses a smaller initial chain of
        /// dependent cells and then changes the value of a cell in the middle of
        /// the chain, testing how quickly those changes propogate. (And that they
        /// propogate correctly)
        /// </summary>
        [TestMethod()]
        public void StressTest2()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A0", "2.0");

            int max = 5_000;

            // Creates cells A0 - A[MAX] where each one contains a formula
            // referencing the previously created cell.
            for (int counter = 1; counter <= max; counter++)
            {
                string formula = String.Format("=A{0}", counter - 1);

                sheet.SetContentsOfCell(String.Format("A{0}", counter), formula);
            }

            // Checks that each of the cells has the value stored in A0
            for (int index = 0; index <= max; index++)
            {
                var value = sheet.GetCellValue(String.Format("A{0}", index));
                Assert.AreEqual(2.0, value);
            }

            sheet.SetContentsOfCell("A0", "5.0");

            // After changing the contents of A0, every cell that references in it
            // should have the changed value as well.
            for (int index = 0; index <= max; index++)
            {
                var value = sheet.GetCellValue(String.Format("A{0}", index));
                Assert.AreEqual(5.0, value);
            }

            sheet.SetContentsOfCell(String.Format("A{0}", max / 2), "8.0");

            // By changing the contents of a cell in the middle of the dependency
            // chain the first half of the chain should have their values unchanged
            // and the second half should have the newly set value.
            for (int index = 0; index < max; index++)
            {
                var value = sheet.GetCellValue(String.Format("A{0}", index));

                if (index < max / 2)
                    Assert.AreEqual(5.0, value);
                else
                    Assert.AreEqual(8.0, value);
            }
        }

        /// <summary>
        /// Tests the Save() method of the spreadsheet class by
        /// saving a spreadsheet and loading into a new one, and
        /// then comparing the values.
        /// </summary>
        [TestMethod()]
        public void SaveTest()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "version2");

            Assert.IsFalse(sheet.Changed);

            sheet.SetContentsOfCell("a1", "=B1 * 2");
            sheet.SetContentsOfCell("B1", "=c3 / 4.2");
            sheet.SetContentsOfCell("c3", "6.32");

            Assert.IsTrue(sheet.Changed);

            sheet.Save("test.xml");

            Assert.IsFalse(sheet.Changed);

            Spreadsheet loadedSheet = new Spreadsheet("test.xml", s => true, s => s, "version2");

            Assert.AreEqual(new Formula("B1*2"), sheet.GetCellContents("a1"));
            Assert.AreEqual(new Formula("c3 / 4.2"), sheet.GetCellContents("B1"));
            Assert.AreEqual(6.32, sheet.GetCellContents("c3"));

            Assert.AreEqual(sheet.Version, loadedSheet.Version);
        }

        /// <summary>
        /// Tests that the four argument constructor that loads a saved spreadsheet
        /// functions properly by loading an example XML file from the Resources folder.
        /// </summary>
        [TestMethod()]
        public void ConstructorLoadFromFileTest()
        {
            Spreadsheet sheet = new Spreadsheet("Resources/test.xml", s => true, s => s, "version1");

            Assert.AreEqual(3.0, sheet.GetCellValue("a1"));
            Assert.AreEqual(1.5, sheet.GetCellValue("B2"));
            Assert.AreEqual(6.0, sheet.GetCellValue("c3"));
            Assert.AreEqual("great red shark", sheet.GetCellValue("d4"));
            Assert.AreEqual("version1", sheet.Version);
        }

        /// <summary>
        /// Tests that the four argument constructor throws an exception
        /// if passed a non-existent file or path.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ConstructorLoadFromFileFailureTest()
        {
            Spreadsheet sheet = new Spreadsheet("non_existent.xml", s => true, s => s, "version1");
        }

        /// <summary>
        /// Tests that the four argument constructor throws an exception if
        /// the specified version and the saved version don't match.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ConstructorLoadFromFileNonMatchingVersionsTest()
        {
            Spreadsheet sheet = new Spreadsheet("Resources/test.xml", s => true, s => s, "default");
        }

        /// <summary>
        /// Tests that the four argument constructor throws an exception if
        /// there is a malformed cell. (One that doesn't contain both a name
        /// and contents).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ConstructorLoadFromFileMalformedTest()
        {
            Spreadsheet sheet = new Spreadsheet("Resources/malformed.xml", s => true, s => s, "default");
        }

        /// <summary>
        /// Tests that the four argument constructor throws an exception if
        /// the final cell is malformed. This is necessary as the final cell
        /// is checked with separate logic than the others.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ConstructorLoadFromFileMalformedLastCellTest()
        {
            Spreadsheet sheet = new Spreadsheet("Resources/malformed_last_cell.xml", s => true, s => s, "default");
        }
    }
}