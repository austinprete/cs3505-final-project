// Austin Prete
// Octohber 6, 2017
// CS 3500 - Fall 2017

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    //test for push

    /// <summary>
    /// The Spreadsheet class implements the backend logic for a spreadsheet program.
    /// A Spreadsheet contains named cells associated with values that are strings,
    /// numbers, or formulas (which in turn evaluate to numbers). In addition to storing
    /// cell names and values, this class also uses a depedency graph to store 
    /// the relationships between cells, allowing formulas to include references
    /// to other cells.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // A map used to associate cell names and their corresponding Cell objects
        Dictionary<string, Cell> cells;

        // A dependency graph used to store the relationships between cells
        DependencyGraph dependencyGraph;

        // The lookup function that will be passed as a delegate
        // to evaluate formulas in cells.
        private double formulaEvaluatorLookup(string cellName)
        {
            var value = GetCellValue(cellName);

            if (value is double)
            {
                return (double)value;
            }

            throw new ArgumentException();
        }

        // The backing value for the Changed property
        bool changed = false;

        // This property indicates whether a spreadsheet has been
        // modified since its creation or last save.
        public override bool Changed
        {
            get => changed;
            protected set => changed = value;
        }

        /// <summary>
        /// A no argument constructor that creates a spreadsheet
        /// with no additional validity requirements, the identity
        /// function for a normalize function, and the version 
        /// name "default".
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default") { }

        /// <summary>
        /// The primary constructor for instantiating a new spreadsheet.
        /// </summary>
        /// <param name="isValid">a function that returns a boolean value indicating whether the input string is a valid variable name</param>
        /// <param name="normalize">a function that normalizes variable names</param>
        /// <param name="version">the version info for the spreadsheet</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();

            // A newly intialized spreadsheet is considered unmodified
            Changed = false;
        }

        /// <summary>
        /// An additional constructor that creates a new spreadsheet
        /// from a previously saved spreadsheet. Identical to the 
        /// primary constructor except for an additional filename
        /// parameter.
        /// Note: the version info passed into the constructor must
        ///       match the version in the saved spreadsheet.
        /// </summary>
        /// <param name="filename">the previously saved spreadsheet to load</param>
        public Spreadsheet(String filename, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid, normalize, version)
        {
            // Loads the data from the saved spreadsheet at `filename` into the newly initialized spreadsheet.
            LoadSpreadsheetFromXmlFile(filename);

            // The version info must match between the specified value and the previously saved spreadsheet.
            if (Version != version)
            {
                throw new SpreadsheetReadWriteException("Specified version number and version encountered in saved spreadsheet differ");
            }
        }

        /// <summary>
        /// Determines if the provided cell name is valid. Valid cell names 
        /// are composed of one or more letters followed by one or more digits.
        /// Also makes sure that the name is valid according to the user provided
        /// IsValid function. If the cell name is not valid, this function throws 
        /// an InvalidNameException()
        /// </summary>
        /// <param name="cellName">the cell name to evaluate</param>
        private void ValidateCellName(string cellName)
        {
            String namePattern = @"^[a-zA-Z]+\d+$";

            if (ReferenceEquals(null, cellName) || !Regex.IsMatch(Normalize(cellName), namePattern) || !IsValid(Normalize(cellName)))
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Simple helper method to perform a null check on cell content and throw an ArgumentNullException
        /// if the provided contents to set a cell to are null.
        /// </summary>
        /// <param name="contents">the value that the cell's contents are being set to</param>
        private void ValidateContents(object contents)
        {
            if (ReferenceEquals(null, contents))
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Retrieves the contents of the cell associated with the provided name.
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <returns>the contents of the cell</returns>
        public override object GetCellContents(string name)
        {
            ValidateCellName(name);
            string normalizedName = Normalize(name);

            if (!cells.TryGetValue(normalizedName, out Cell cell))
            {
                cell = new Cell(normalizedName);
            }

            return cell.Contents;
        }

        /// <summary>
        /// Finds all cells in the spreadsheet that aren't empty (indicated by the empty string: "")
        /// </summary>
        /// <returns>an enumerable collection containing the list of non-empty cells</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            var nonEmptyCells = cells.Where((kvPair) => !kvPair.Value.Contents.Equals(""));

            return nonEmptyCells.Select((kvPair) => kvPair.Key);
        }

        /// <summary>
        /// Sets the contents of the specified cell to the provided double value.
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="number">the number to set as the cell's value</param>
        /// <returns>A set containing the name of this cell and all of its dependents 
        /// (both direct and indirect)</returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            cells[name] = new Cell(name, number);
            dependencyGraph.ReplaceDependees(name, new List<string>());

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Sets the contents of the specified cell to the provided string value.
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="text">the text to set as the cell's value</param>
        /// <returns>A set containing the name of this cell and all of its dependents 
        /// (both direct and indirect)</returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            cells[name] = new Cell(name, text);
            dependencyGraph.ReplaceDependees(name, new List<string>());

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Sets the contents of the specified cell to the provided Formula instance.
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="formula">the Formula object to set as the cell's value</param>
        /// <returns>A set containing the name of this cell and all of its dependents 
        /// (both direct and indirect)</returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            Cell cell = new Cell(name, formula);

            cells[name] = cell;
            dependencyGraph.ReplaceDependees(name, new List<string>());

            foreach (string variableName in formula.GetVariables())
            {
                dependencyGraph.AddDependency(variableName, name);
            }
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Finds all direct dependents of the specified cell, which are any
        /// cells that directly reference it in a formula.
        /// </summary>
        /// <param name="name">the name of the cell to retrieve dependents of</param>
        /// <returns>an enumerable collection containing the names of directly dependent cells</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencyGraph.GetDependents(name);
        }

        /// <summary>
        /// Provided a path to a saved spreadsheet in XML format,
        /// returns the version information of the spreadsheet.
        /// </summary>
        /// <param name="filename">the saved spreadsheet file to retrieve version info for</param>
        /// <returns>the version info</returns>
        public override string GetSavedVersion(string filename)
        {
            using (XmlReader reader = XmlReader.Create(filename))
            {
                // Reads until the opening tag
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        break;
                    }
                }

                // This should always be true for a valid spreadsheet file since
                // the first tag in the spreadsheet has to be <spreadsheet>
                if (reader.Name.Equals("spreadsheet"))
                {
                    // Attempts to get the value of the attribute `version`
                    var version = reader.GetAttribute("version");
                    
                    // If it is null than the spreadsheet contains no version info
                    if (!ReferenceEquals(version, null))
                    {
                        return version;
                    }
                }
            }

            // This will be thrown unless the version is successfully parsed above
            throw new SpreadsheetReadWriteException(String.Format("Couldn't read version information from specified file: {0}", filename));
        }

        /// <summary>
        /// Saves the current spreadsheet in XML format to the 
        /// specified file. A saved spreadsheet can be loaded 
        /// later with the Spreadsheet class' four argument constructor. 
        /// </summary>
        /// <param name="filename">the name/path to save the spreadsheet XML file to</param>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {

                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", Version);

                // Write out XML representation of cells
                foreach (Cell s in cells.Values)
                    s.WriteXml(writer);

                writer.WriteEndElement(); // End spreadsheet block
                writer.WriteEndDocument();
            }

            // A saved spreadsheet is considered unmodified until the next edit
            changed = false;
        }

        /// <summary>
        /// Similar to GetCellContents, but if the cell contents are
        /// a formula this method will return either the evaluated
        /// Double value or a FormulaError value. String and double
        /// values are returned as their respective types.
        /// </summary>
        /// <param name="name">the name of the cell to retrieve the value of</param>
        /// <returns>the value contained in the specified cell</returns>
        public override object GetCellValue(string name)
        {
            ValidateCellName(name);
            string normalizedName = Normalize(name);

            if (!cells.TryGetValue(normalizedName, out Cell cell))
            {
                cell = new Cell(normalizedName);
            }

            return cell.Value;
        }

        /// <summary>
        /// Sets the contents of the specified cell based on the `content`
        /// parameter. This function will detect whether the `content` is
        /// a formula, string, or double value and use the appropriate
        /// overload of SetCellContents(). For the purposes of the spreadsheet
        /// program any string that begins with "=" will be considered a
        /// formula.
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="content">the desired contents of the cell as a string</param>
        /// <returns>a set containing the modified cell and all of its dependents</returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            ValidateCellName(name);
            ValidateContents(content);

            string normalizedName = Normalize(name);

            Changed = true;

            ISet<string> cellsToRecalculate;

            // Attempts to parse the contents as a formula or double, and falls back to treating them
            // as a string if those fail.
            if (content.StartsWith("="))
            {
                cellsToRecalculate = SetCellContents(normalizedName, new Formula(content.Substring(1), Normalize, IsValid));
            }
            else if (Double.TryParse(content, out double number))
            {
                cellsToRecalculate = SetCellContents(normalizedName, number);
            }
            else
            {
                cellsToRecalculate = SetCellContents(normalizedName, content);
            }

            // Update the values of any cells that need to be recalculated.
            UpdateCells(cellsToRecalculate);

            return cellsToRecalculate;
        }

        /// <summary>
        /// Updates the value property of the specified cells.
        /// This is useful for updating the values of any dependent
        /// cells when setting the contents of a cell.
        /// </summary>
        /// <param name="cellsToRecalculate">a set of cell names to update</param>
        private void UpdateCells(ISet<string> cellsToRecalculate)
        {
            // For each cell that needs to be recalculated, triggers
            // a value update. This will cause formula values to be
            // computed with the most up to date variable values.
            foreach (string cellName in cellsToRecalculate)
            {
                cells[cellName].UpdateValue(formulaEvaluatorLookup);
            }
        }

        /// <summary>
        /// Reads the XML file specified by the filename parameter and loads
        /// the version info and cell data into the current spreadsheet 
        /// instance.
        /// Note: this is a destructive operation and should only be done when
        ///     creating a new spreadsheet or if overwriting the contents of 
        ///     a spreadsheet is the expected behavior
        /// </summary>
        /// <param name="filename">the saved XML file</param>
        private void LoadSpreadsheetFromXmlFile(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {

                    string currentName = null;
                    string currentContents = null;
                    bool reachedFirstCell = false;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    Version = reader.GetAttribute("version");
                                    break;
                                case "cell":
                                    // If currentName and currentContents contain values, sets the cell value accordingly
                                    if (!ReferenceEquals(currentName, null) && !ReferenceEquals(currentContents, null))
                                    {
                                        SetContentsOfCell(currentName, currentContents);
                                    }
                                    else
                                    {
                                        // If we've already reached the first cell, then the above check failing indicates
                                        // that there is not both a <name> and a <contents> tag in the cell XML.
                                        if (reachedFirstCell)
                                        {
                                            throw new SpreadsheetReadWriteException("Spreadsheet malformed");
                                        }
                                    }

                                    currentName = null;
                                    currentContents = null;
                                    reachedFirstCell = true;
                                    break;
                                case "name":
                                    reader.Read();
                                    currentName = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    currentContents = reader.Value;
                                    break;
                            }
                        }
                    }

                    // Similar check to the one in the loop, but this one will only be
                    // run for the last cell in the spreadsheet.
                    if (!ReferenceEquals(currentName, null) && !ReferenceEquals(currentContents, null))
                    {
                        SetContentsOfCell(currentName, currentContents);
                    }
                    else
                    {
                        throw new SpreadsheetReadWriteException("Spreadsheet malformed");
                    }
                }

                // Since this is a destructive operation that essentially replaces the current
                // spreadsheet, we consider it unmodified until the next edit.
                Changed = false;
            }
            catch (Exception e) // If an exception is thrown, we repackage it as a SpreadSheetReadWriteException
            {
                throw new SpreadsheetReadWriteException(
                    String.Format(
                        "Failed to load spreadsheet from path \"{0}\", encountered following error: {1} - {2}",
                        filename,
                        e.GetType(),
                        e.Message
                    )
                );
            }
        }
    }
}
