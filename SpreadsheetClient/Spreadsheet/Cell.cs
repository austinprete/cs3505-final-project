// Austin Prete
// September 28, 2017
// CS 3500 - Fall 2017

using SpreadsheetUtilities;
using System;
using System.Xml;

namespace SS
{
    /// <summary>
    /// A simple class that represents an individual cell in a spreadsheet.
    /// </summary>
    class Cell
    {
        // The name of the cell, used to reference it in a spreadsheet
        string name;

        // The stored contents of the cell. Its default value of an empty
        // string represents an empty cell
        object contents = "";

        object value;

        // A public getter for the contents field
        public object Contents
        {
            get
            {
                return contents;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// A cell created with a provided value defaults to being an empty cell, 
        /// represented by the empty string as its contents.
        /// </summary>
        /// <param name="name"></param>
        public Cell(string name) : this(name, "") { }

        /// <summary>
        /// A basic constructor to instantiate a cell with the provided name and
        /// value for its contents. Valid values for the `contents` field are
        /// strings, doubles, and Formula instances.
        /// </summary>
        public Cell(string name, object contents)
        {
            this.name = name;
            this.contents = contents;
            this.value = contents;
        }

        public void UpdateValue(Func<string, double> lookup)
        {
            if (contents is Formula)
            { 
                value = ((Formula)contents).Evaluate(lookup);
            } else
            {
                value = contents;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("cell");

            writer.WriteElementString("name", name);

            string contentsToWrite;

            if (contents is Formula)
            {
                contentsToWrite = string.Format("={0}", contents.ToString());
            } else
            {
                contentsToWrite = contents.ToString();
            }

            writer.WriteElementString("contents", contentsToWrite);

            writer.WriteEndElement();
        }
    }
}
