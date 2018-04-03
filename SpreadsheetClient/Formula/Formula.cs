// Austin Prete
// u0906796
// CS 3500 - Fall 2017

// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private string formula;
        private Func<string, string> normalize;
        private Func<string, Boolean> isValid;
        private IEnumerable<string> tokens;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            this.formula = formula;
            this.normalize = normalize;
            this.isValid = isValid;
            tokens = GetTokens(formula);

            // Check that there is at least one token.
            if (tokens.Count() == 0)
            {
                throw new FormulaFormatException("Formula can't be empty.");
            }

            // Check the starting token rule
            var firstToken = tokens.First();

            if (!(IsNumber(firstToken) || IsVariable(firstToken) || firstToken.Equals("(")))
            {
                throw new FormulaFormatException("Formula must start with a number, variable, or opening parenthesis.");
            }

            // Check the ending token rule
            var lastToken = tokens.Last();

            if (!(IsNumber(lastToken) || IsVariable(lastToken) || lastToken.Equals(")")))
            {
                throw new FormulaFormatException("Formula must end with a number, variable, or closing parenthesis.");
            }

            // While iterating through the tokens, we'll increment this for every opening paranthesis
            // and decrement it for every closing parenthesis. If the counter is ever negative then
            // there are misplaced parentheses.
            var parenthesisCounter = 0;

            // Using an index-based loop here allows us to easily retrieve the next token
            foreach (int index in Enumerable.Range(0, tokens.Count()))
            {
                var token = tokens.ElementAt(index);
                // This will be set to the value of the next token if there is another token remaining, otherwise null.
                var nextToken = tokens.ElementAtOrDefault(index + 1);

                // A boolean flag for use in checking the following rules.
                var hasNext = index < tokens.Count() - 1;

                // Check that the token is a recognized token type (number, variable, operator, or parenthesis)
                if (!(IsNumber(token) || IsVariable(token) || IsOperator(token) || Regex.IsMatch(token, @"^[\(\)]$")))
                {
                    throw new FormulaFormatException(String.Format("Unrecognized token: {0}", token));
                }

                // If the token is a variable, make sure the normalized version is still a valid variable and passes the validator function
                if (IsVariable(token))
                {
                    var normalized = normalize(token);

                    if (!IsVariable(normalized))
                    {
                        throw new FormulaFormatException(
                            String.Format("Formula contains variable that is invalid after normalization: {0} -> {1}", token, normalized));
                    }

                    if (!isValid(normalized))
                    {
                        throw new FormulaFormatException(
                            String.Format("Formula contains variable that doesn't pass the provided validator after normalization: {0}", normalized));
                    }
                }

                // If the token is an opening parenthesis, increment the counter and check the parenthesis following rule.
                if (token.Equals("("))
                {
                    parenthesisCounter += 1;

                    if (hasNext && !(IsNumber(nextToken) || IsVariable(nextToken) || nextToken.Equals("(")))
                    {
                        throw new FormulaFormatException("An opening parenthesis must be followed by a number, variable, or opening parenthesis.");
                    }
                }
                else if (token.Equals(")")) // If it's a closing parenthesis, simply decrement the parentheses counter
                {
                    parenthesisCounter -= 1;
                }

                // This indicates that a ")" came before the corresponding "("
                if (parenthesisCounter < 0)
                {
                    throw new FormulaFormatException("Parentheses are misplaced");
                }

                // Check the extra following rule
                if (hasNext && (IsNumber(token) || IsVariable(token) || token.Equals(")")))
                {
                    if (!(IsOperator(nextToken) || nextToken.Equals(")")))
                    {
                        throw new FormulaFormatException(
                            String.Format(
                                "Any token that immediately follows a number, a variable," +
                                " or a closing parenthesis must be either an operator or a closing parenthesis\n\n" +
                                "\tFirst token: {0}\n\tFollowing token: {1}",
                                token,
                                nextToken
                            )
                        );
                    }
                }
            }

            // This indicates there wasn't a closing parenthesis for every opening one.
            if (parenthesisCounter != 0)
            {
                throw new FormulaFormatException("Parentheses are unbalanced");
            }
        }

        /// <summary>
        /// A helper method that safely checks if calling Peek() on the
        /// provided stack is possible (there are values remaining), and
        /// returns the result as an out parameter if there is.
        /// </summary>
        /// <param name="stack">the stack to peek</param>
        /// <param name="peekValue">an out parameter to place the peeked value in if successful</param>
        /// <returns>true if Peek() was successful, with the result placed in the peekValue parameter; otherwise, returns false</returns>
        private bool TryStackPeek(Stack<string> stack, out string peekValue)
        {
            if (stack.Count > 0)
            {
                peekValue = stack.Peek();
                return true;
            }

            peekValue = null;
            return false;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string token in tokens)
            {

                double value;
                var tokenIsNumber = false;

                // The logic for the following two cases is identical, so they just set the tokenIsValue flag to true
                // and the value is used later on.
                if (Double.TryParse(token, out value)) // If the token can be parsed as an integer, we treat it as a value
                {
                    tokenIsNumber = true;
                }
                else if (IsVariable(token)) // If the token matches the format of a variable, we call the variableEvaluator method on it
                {
                    var normalizedVariable = normalize(token);

                    // If variableEvaluator can't find the specified variable it will 
                    // likely throw an ArgumentException. In that case we catch it to
                    // throw our own exception indicating that variable lookup failed.
                    try
                    {
                        value = lookup(normalizedVariable);
                    }
                    catch (ArgumentException)
                    {
                        return new FormulaError(String.Format("Variable lookup failed for the following variable: {0}", normalizedVariable));
                    }

                    // Set this flag which is utilized at the bottom of this for loop declaration
                    tokenIsNumber = true;
                }
                else if (token.Equals("*") || token.Equals("/") || token.Equals("(")) // In all of these cases we simply push the operator onto the stack
                {
                    operatorStack.Push(token);
                }
                else if (token.Equals("+") || token.Equals("-"))
                {
                    // If a + or - operator is at the top of the operator stack
                    // we apply it to the top two values in the value stack and
                    // push the result back onto the value stack.
                    if (TryStackPeek(operatorStack, out string topOperator) && (topOperator.Equals("+") || topOperator.Equals("-")))
                    {
                        // Remove the top operator from the stack
                        operatorStack.Pop();

                        // The naming of these variables indicates their order in the expression
                        var secondValue = valueStack.Pop();
                        var firstValue = valueStack.Pop();

                        double newValue;

                        if (topOperator.Equals("+"))
                        {
                            newValue = firstValue + secondValue;
                        }
                        else // This case corresonds to a subtraction sign
                        {
                            newValue = firstValue - secondValue;
                        }

                        valueStack.Push(newValue);
                    }

                    operatorStack.Push(token);
                }
                else if (token.Equals(")"))
                {
                    if (TryStackPeek(operatorStack, out string topOperator) && (topOperator.Equals("+") || topOperator.Equals("-")))
                    {
                        operatorStack.Pop();

                        var secondValue = valueStack.Pop();
                        var firstValue = valueStack.Pop();

                        double newValue;

                        if (topOperator.Equals("+"))
                        {
                            newValue = firstValue + secondValue;
                        }
                        else // This runs if token was a "-"
                        {
                            newValue = firstValue - secondValue;
                        }

                        valueStack.Push(newValue);
                    }

                    operatorStack.Pop();

                    if (TryStackPeek(operatorStack, out topOperator) && (topOperator.Equals("*") || topOperator.Equals("/")))
                    {
                        operatorStack.Pop();

                        var secondValue = valueStack.Pop();
                        var firstValue = valueStack.Pop();

                        double newValue;

                        if (topOperator.Equals("*"))
                        {
                            newValue = firstValue * secondValue;
                        }
                        else // This is executed if topOperator was "/"
                        {
                            if (secondValue == 0)
                            {
                                return new FormulaError("Division by zero");
                            }
                            newValue = firstValue / secondValue;
                        }

                        valueStack.Push(newValue);
                    }
                }

                // This could will be run in two cases: the token was an integer or
                // a variable that was successfully evaluated
                if (tokenIsNumber)
                {
                    if (TryStackPeek(operatorStack, out string topOperator) && (topOperator.Equals("*") || topOperator.Equals("/")))
                    {
                        // The value of topOperator needs to actually be removed from the stack 
                        operatorStack.Pop();

                        // Note: since otherValue was added to the stack first, it came first in the expression.
                        // This matters for non-commutative operations such as - and /
                        double otherValue = valueStack.Pop();

                        double newValue;

                        if (topOperator.Equals("*"))
                            newValue = value * otherValue;
                        else // "/"
                        {
                            if (value == 0)
                            {
                                return new FormulaError("Division by zero");
                            }
                            newValue = otherValue / value;
                        }

                        valueStack.Push(newValue);
                        continue;
                    }

                    valueStack.Push(value);
                }
            }
            // There are two possibles valid cases at this point
            // The first is 1 value in the stack and no operators remaining.
            if (valueStack.Count == 1 && operatorStack.Count == 0)
            {
                return valueStack.Pop();
            }

            // The second is 2 values in the stack and one operator to apply to them.
            if (valueStack.Count != 2)
            {
                return new FormulaError("Misplaced operators");
            }

            var lastOperator = operatorStack.Pop();

            var value2 = valueStack.Pop();
            var value1 = valueStack.Pop();

            if (lastOperator.Equals("+"))
                return value1 + value2;
            else // "-"
                return value1 - value2;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<String> variables = new HashSet<String>();

            foreach (String token in this.tokens)
            {
                // Check if the token matches the standard variable regex
                if (IsVariable(token))
                {
                    var normalizedToken = normalize(token);

                    // Check that normalization didn't make it invalid
                    if (IsVariable(normalizedToken))
                    {
                        // Add it to the set
                        variables.Add(normalizedToken);
                    }
                }
            }

            // Return the set of normalized variables
            return variables;
        }

        /// <summary>
        /// Determines if a string token is a variable according to the standard
        /// regex (not the provided isValid() function)
        /// </summary>
        /// <param name="token">the token to evaluate</param>
        /// <returns>true if token is a variable; otherwise false</returns>
        private Boolean IsVariable(string token)
        {
            String varPattern = @"^[a-zA-Z_]([a-zA-Z_]|\d)*$";

            if (Regex.IsMatch(token, varPattern))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// A helper method that determines if a token is a number by attempting 
        /// to parse it as a double
        /// </summary>
        /// <param name="token">the token to evaluate</param>
        /// <returns>true if the token is a valid double value</returns>
        private Boolean IsNumber(string token)
        {
            return Double.TryParse(token, out double _);
        }

        /// <summary>
        /// A helper method that determines if a token is an operator
        /// </summary>
        /// <param name="token">the token to evaluate</param>
        /// <returns>true if the token is an operator</returns>
        private Boolean IsOperator(string token)
        {
            String opPattern = @"^[\+\-*/]$";

            return Regex.IsMatch(token, opPattern);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string output = "";

            foreach (string token in tokens)
            {
                if (Double.TryParse(token, out double tokenAsDouble))
                {
                    output += tokenAsDouble.ToString();
                }
                else if (IsVariable(token))
                {
                    output += normalize(token);
                }
                else
                {
                    output += token;
                }
            }

            return output;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }
            else if (obj.GetType() != typeof(Formula))
            {
                return false;
            }

            // Since ToString deterministically creates equivalent strings for
            // equivalent formulas, we can rely on string comparison
            return ToString().Equals((obj as Formula).ToString());
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (Object.ReferenceEquals(f1, null) || Object.ReferenceEquals(f2, null))
            {
                return Object.ReferenceEquals(f1, f2);
            }
            else
            {
                return f1.Equals(f2);
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
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
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
