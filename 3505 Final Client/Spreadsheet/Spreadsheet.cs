//Written by Alex Gritton U0667623 for CS 3500 Fall 2012
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using System.Xml;

namespace SS
{
    /// <summary>
    /// A spreadsheet consists of an infinite number of named cells .
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that despite
    /// their similarity, "A15" and "a15" are different cell names.)  On the other hand, 
    /// "Z", "X07", and "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<String, Cell> spreadsheet; //Keeps track of the Cell names and their Correspoding cell objects.
        private DependencyGraph dependency_graph; //Keeps track of depencies between Cells.
        private bool has_changed;
        /// <summary>
        /// Creates a new Spreadsheet.
        /// </summary>
        public Spreadsheet()
            :base(valid_name, name=>name, "default")
        {
            spreadsheet = new Dictionary<string, Cell> { }; //Keeps track of the Cell names and their Correspoding cell objects.
            dependency_graph = new DependencyGraph(); //Keeps track of depencies between Cells.
            has_changed = false;
        }
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            spreadsheet = new Dictionary<string, Cell> { };
            dependency_graph = new DependencyGraph();
            has_changed = false;
        }
        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            //read save file from filePath
            spreadsheet = new Dictionary<string, Cell> { };
            dependency_graph = new DependencyGraph();
            has_changed = false;
            try
            {
                using (XmlReader read_file = XmlReader.Create(filePath))
                {
                    if (GetSavedVersion(filePath) != version)
                    {
                        throw new SpreadsheetReadWriteException("File versions are not the same.");
                    }
                    try
                    {
                        string cell_name = "";
                        while (read_file.Read())
                        {
                            if (read_file.IsStartElement())
                            {
                                switch (read_file.Name)
                                {
                                    case "spreadsheet":
                                        break;
                                    case "cell":
                                        break;
                                    case "name":
                                        read_file.Read();
                                        cell_name = read_file.Value;
                                        break;
                                    case "contents":
                                        read_file.Read();
                                        try
                                        {
                                            SetContentsOfCell(cell_name, read_file.Value);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new SpreadsheetReadWriteException(ex.Message);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {

                        throw new SpreadsheetReadWriteException("Error while reading from file");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SpreadsheetReadWriteException(ex.Message);
            }
            has_changed = false;
        }
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader read_file = XmlReader.Create(filename))
                {
                    string version_result = "";
                    try
                    {
                        while (read_file.Read())
                        {
                            if (read_file.IsStartElement())
                            {
                                switch (read_file.Name)
                                {
                                    case "spreadsheet":
                                        version_result = read_file["version"];
                                        break;
                                }

                            }
                        }
                    }
                    catch
                    {

                        throw new SpreadsheetReadWriteException("Could not read from file.");
                    }
                    return version_result;
                }
            }
            catch (Exception ex)
            {
                throw new SpreadsheetReadWriteException(ex.Message);
            }
        }
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            try
            {
                using (XmlWriter save_file = XmlWriter.Create(filename))
                {
                    try
                    {
                        save_file.WriteStartDocument();
                        save_file.WriteStartElement("spreadsheet");
                        save_file.WriteAttributeString("version", Version);
                        foreach (KeyValuePair<string, Cell> pair in spreadsheet)
                        {
                            if (pair.Value.get_contents().ToString() != "")
                            {
                                save_file.WriteStartElement("cell");
                                save_file.WriteStartElement("name");
                                save_file.WriteString(pair.Key);
                                save_file.WriteEndElement();
                                save_file.WriteStartElement("contents");
                                if (pair.Value.get_contents() is Formula)
                                {
                                    save_file.WriteString("=" + pair.Value.get_contents().ToString());
                                }
                                else
                                {
                                    save_file.WriteString(pair.Value.get_contents().ToString());
                                }
                                save_file.WriteEndElement();
                                save_file.WriteEndElement();
                            }
                        }
                        save_file.WriteEndElement();
                        save_file.WriteEndDocument();
                        has_changed = false;
                    }
                    catch
                    {

                        throw new SpreadsheetReadWriteException("error writing to the file.");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                throw new SpreadsheetReadWriteException(ex.Message);
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string cell_name)
        {
            string name = Normalize(cell_name);
            if (IsValid(name) == false || name ==null)
            {
                throw new InvalidNameException();
            }
            //norm and validate
            if (spreadsheet.ContainsKey(name))
            {
                return spreadsheet[name].get_value();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> non_empty_cells = new List<string> { };
            foreach (KeyValuePair<String, Cell> pair in spreadsheet)
            {
                non_empty_cells.Add(pair.Key);
            }
            return non_empty_cells;
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string cell_name)
        {
            string name = Normalize(cell_name);
            //checks if the name is null or invalid if so throws a new InvalidNameException();
            if (name == null || IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            //if the spreadsheet already has the key set this returns the contents of that cell.
            if (spreadsheet.ContainsKey(name))
            {
                return spreadsheet[name].get_contents();                
            }
            //if the key has not yet been set up this sets it up and calls the function again returning the contents.
            else
            {
                spreadsheet.Add(name, new Cell(name));
                return GetCellContents(name);
            }
        }
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string cell_name, string content)
        {
            has_changed = true;
            string name = Normalize(cell_name);
            double number;
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            else if (name == null || IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            else if (double.TryParse(content, out number))
            {
                return SetCellContents(name, number);
            }
            else if (content.StartsWith("=") && content.Length>1)
            {
                Formula formula;
                try
                {
                    string sub_content = content.Substring(1);
                    formula = new Formula(normalize_function(sub_content, Normalize));
                }
                catch
                {
                    
                    throw new FormulaFormatException("Invalid Formula");
                }
                try
                {
                    return SetCellContents(name, formula);
                }
                catch (Exception)
                {
                        
                    throw new CircularException();
                }
            }
            else if (content.StartsWith("=") && content.Length <= 1)
            {
                throw new FormulaFormatException("Invalid Fomula");
            }
            else
            {
                return SetCellContents(name, content);
            }

        }
        /// <summary>
        /// This function normalizes the formula being passed.
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="normalize"></param>
        /// <returns></returns>
        private string normalize_function(string formula, Func<string, string> normalize)
        {
            string normalized_formula = "";
            IEnumerable<string> tokens = GetTokens(formula);
            foreach (string token in tokens)
            {
                if (valid_name(token) == true)
                {
                    if (IsValid(token) == true)
                    {
                        normalized_formula += normalize(token);
                    }
                    else
                    {
                        throw new FormulaFormatException("Invalid variable name");
                    }
                }
                else
                {
                    normalized_formula += token;
                }
            }
            return normalized_formula;

        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String cell_name, double number)
        {
            string name = Normalize(cell_name);
            //checks if the name is null or invalid if so throws a new InvalidNameException();
            if (name == null || IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            //checks if the cell with the name has been set up, if so this sets the contents of the cell to the number passed in.
            if (spreadsheet.ContainsKey(name))
            {
                spreadsheet[name].set_contents(number);
            }
            //if the cell hasnt been set up this creates a new cell and sets its contents to the number passed in.
            else
            {
                spreadsheet.Add(name, new Cell(name, number, number));               
            }
            //creates a hash set for the return ISet.
            ISet<string> depends = new HashSet<string> { };
            //looks through each item in the dependency graph and add them to the hash set to return.
            foreach (string item in GetCellsToRecalculate(name))
            {
                depends.Add(item);
            }
            List<string> dependency_fix = new List<string> { };
            spreadsheet[name].set_value(number);
            foreach (string value in dependency_graph.GetDependees(name))
            {
                dependency_fix.Add(value);
            }
            foreach (string key in dependency_fix)
            {
                dependency_graph.RemoveDependency(key, name);
            }
            IEnumerable<string> recalculate_cells = GetCellsToRecalculate(name);
            foreach (string cell in recalculate_cells)
            {
                if (!(cell == name))
                {
                    Formula value_of_formula = (Formula)GetCellContents(cell);
                    spreadsheet[cell].set_value(value_of_formula.Evaluate(Lookup));
                }
            }
            //returns the hash set with all the dependents of name.
            return depends;
        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string cell_name, string text)
        {
            string name = Normalize(cell_name);
            //if the text passed in is null this throws an ArgumentNullException();
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            //checks if the name is null or invalid if so throws a new InvalidNameException();
            if (name == null || IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            else if (text.Length < 1)
            {
                return new HashSet<string> { };
            }
            //checks if the cell with the name has been set up, if so this sets the contents of the cell to the text passed in.
            if (spreadsheet.ContainsKey(name))
            {
                spreadsheet[name].set_contents(text);
            }
            //if the cell hasnt been set up this creates a new cell and sets its contents to the text passed in.
            else
            {
                spreadsheet.Add(name, new Cell(name, text, text));
            }
            //creates a hash set for the return ISet.
            HashSet<string> depends = new HashSet<string> { };
            spreadsheet[name].set_value(text);
            //looks through each item in the dependency graph and add them to the hash set to return.
            foreach (string item in GetCellsToRecalculate(name))
            {
                depends.Add(item);
            }
            List<string> dependency_fix = new List<string> { };
            foreach (string value in dependency_graph.GetDependees(name))
            {
                dependency_fix.Add(value);
            }
            foreach (string key in dependency_fix)
            {
                dependency_graph.RemoveDependency(key, name);
            }
            foreach (string cell in depends)
            {
                if (!(cell == name))
                {
                    Formula value_of_formula = (Formula)GetCellContents(cell);
                    spreadsheet[cell].set_value(value_of_formula.Evaluate(Lookup));
                }
            }
            //returns the hash set with all the dependents of name.
            return depends;
        }
        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string cell_name, SpreadsheetUtilities.Formula formula)
        {
            IEnumerable<string> backup_variables = null;
            object backup_contents = null;
            bool had_name = false;
            string name = Normalize(cell_name);
            //if the formula passed in is null this throws an ArgumentNullException();
            if (formula == null)
            {
                throw new ArgumentNullException("Formula cannot be null");
            }
            //checks if the name is null or invalid if so throws a new InvalidNameException();
            if (name == null || IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            //checks if the cell with the name has been set up, if so this sets the contents of the cell to the formula passed in.
            if (spreadsheet.ContainsKey(name))
            {
                backup_variables = dependency_graph.GetDependees(name);
                backup_contents = spreadsheet[name].get_contents();
                spreadsheet[name].set_contents(formula);
                had_name = true;
            }
            //if the cell hasn't been set up this creates a new cell and sets its contents to the text passed in.
            else
            {
                backup_contents = " ";
                backup_variables = new HashSet<string> { };
                spreadsheet.Add(name, new Cell(name, formula));
            }
            IEnumerable<string> variables = formula.GetVariables(); //Gets all the variables 
            dependency_graph.ReplaceDependees(name, variables); //Replace the dependees of the name with all the variables in the formula passed in.
            //creates a hash set for the return ISet.
            ISet<string> depends = new HashSet<string> { };
            //looks through each item in the dependency graph and add them to the hash set to return.
            try
            {
                IEnumerable<string> recalculate_cells = GetCellsToRecalculate(name);
                foreach (string item in recalculate_cells)
                {
                    depends.Add(item);
                }
            }
            catch (CircularException)
            {
                spreadsheet[name].set_contents(backup_contents);
                dependency_graph.ReplaceDependees(name, backup_variables);
                throw new CircularException();
            }
            //sets the value of the cell
            IEnumerable<string> evaluate_cells = GetCellsToRecalculate(name);
            foreach (string cell in evaluate_cells)
            {
                Formula value_of_formula = (Formula)GetCellContents(cell);
                try
                {
                    spreadsheet[cell].set_value(value_of_formula.Evaluate(Lookup));
                }
                catch(ArgumentException)
                {
                }
            }
            //returns the hash set with all the dependents of name.
            return depends;
        }
        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        protected override IEnumerable<string> GetDirectDependents(string cell_name)
        {
            string name = Normalize(cell_name);
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (IsValid(name) == false)
            {
                throw new InvalidNameException();
            }
            ISet<string> depends = new HashSet<string> { };
            foreach (string item in dependency_graph.GetDependents(name))
            {
                depends.Add(item);
            }
            return depends;
        }
        public override bool Changed
        {
            get
            {
                return has_changed;
            }
            protected set
            {
            }
        }
            
        //this is a helper method to check if the names are valid
        private delegate bool valid_Name (string name); 
        valid_Name validName = valid_name;
        private static bool valid_name(string name)
        {
            bool valid = false;
            Regex var = new Regex(@"^([a-zA-z]+)(\d*)$");
            if (var.IsMatch(name))
            {
                valid = true;
            }
            return valid;
        }
        private delegate string my_normalize(string name);
        my_normalize norm = normalize;
        private static string normalize(string name)
        {
            return name.ToUpper();
        }
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z]+\d+";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
        /// <summary>
        /// This is a lookup delegate for the formula evaluate method. It looks up the value of a variable and returns a double.
        /// </summary>
        /// <param name="cell_name"></param>
        /// <returns></returns>
        private double Lookup(string cell_name)
        {
            double variable = 0;
            try
            {
                    variable = (double)GetCellValue(cell_name);
            }
            catch (Exception)
            {
                
                throw new ArgumentException();
            }
            return variable;
        }
    }
    /// <summary>
    /// Creates a Cell objcet with a cell name and cell contents. This is a helper class for spreadsheet.
    /// </summary>
    public class Cell
    {
        private string cell_name;
        private object contents;
        private object value;
        /// <summary>
        /// Creates a cell with a Name passed in and sets contents to an empty string.
        /// </summary>
        /// <param name="name"></param>
        public Cell(String name)
        {
            cell_name = name;
            contents = "";

        }
        /// <summary>
        /// Creates a cell with a name and contents.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents">double.</param>
        public Cell(String name, string obj, string text)
        {
            cell_name = name;
            contents = obj;
            value = text;
        }
        /// <summary>
        /// Creates a cell with a name and contents.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj">string</param>
        public Cell(String name, double obj, double number)
        {
            cell_name = name;
            contents = obj;
            value = number;
        }
        /// <summary>
        /// Creates a cell with a name and contents.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula">formula</param>
        public Cell(String name, SpreadsheetUtilities.Formula formula)
        {
            cell_name = name;
            contents = formula;
        }
        /// <summary>
        /// Gets the contents of a Cell.
        /// </summary>
        /// <returns>Returns either an object</returns>
        public object get_contents()
        { 
            return contents;
        }
        /// <summary>
        /// Sets a cells contents to the new contents.
        /// </summary>
        /// <param name="content"></param>
        public void set_contents(object content)
        {
            contents = content;
        }
        public object get_value()
        {
            return value;
        }
        public void set_value(object new_value)
        {
            value =  new_value;
        }
    }
}
