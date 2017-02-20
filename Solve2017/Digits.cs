using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve2017
{
    /// <summary>
    /// Class to hold a unique set of digits, could be any length.
    /// Designed to track the history of how this set was derived.
    /// </summary>
    public class Digits : List<double>
    {
        public Digits(IEnumerable<double> digits)
        {
            this.AddRange(digits);
        }
        public Digits(IEnumerable<string> digits)
        {
            this.AddRange(digits.Select(f => double.Parse(f)));
            OriginalDigits.AddRange(digits);
        }
        public Digits(Digits previous)
        {
            Previous = previous;
        }

        public string Key { get { return string.Join(" ", this); } }
        public List<string> OriginalDigits { get; } = new List<string>();
        /// <summary>
        /// The digits before this operation is performed
        /// </summary>
        public Digits Previous { get; set; }

        public string OperatorString { get; set; }
        /// <summary>
        /// Index of the argument used in the operation. For unary this is the 
        /// 0-based index of the digit. For binary, this is the 1-based space 
        /// between the two digits. This way we only need to store a single number.  
        /// </summary>
        public int ArgumentIndex { get; set; }
        public bool IsBinary { get { return OperatorString.Contains("{1}"); } }

        public int UniqueCount { get; set; } = 1;

        /// <summary>
        /// Determines the expression used to create these numbers. 
        /// Called recursively through parent Digits.
        /// </summary>
        public List<string> DigitCalculations
        {
            get
            {
                // Get list of digits as a string list
                var myDigits = this.Select(f => f.ToString()).ToList();
                // Replace the last calculated number with the calculation.
                if (Previous != null)
                {
                    // Get the calculations so we can use those too.
                    var previousDigits = Previous.DigitCalculations;
                    if (IsBinary)
                    {
                        for (int i = 0; i < myDigits.Count; i++)
                        {
                            if (i < ArgumentIndex - 1)
                            {
                                myDigits[i] = previousDigits[i];
                            }
                            else if (i == ArgumentIndex - 1)
                            {
                                myDigits[ArgumentIndex - 1] =
                                    string.Format(OperatorString,
                                        previousDigits[ArgumentIndex - 1],
                                        previousDigits[ArgumentIndex]);
                            }
                            else
                            {
                                myDigits[i] = previousDigits[i + 1];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < myDigits.Count; i++)
                        {
                            if (ArgumentIndex == i)
                            {
                                myDigits[ArgumentIndex] =
                                    string.Format(OperatorString,
                                        previousDigits[ArgumentIndex]);
                            }
                            else
                            {
                                myDigits[i] = previousDigits[i];
                            }
                        }
                    }
                }
                else
                {
                    myDigits = OriginalDigits;
                }
                if (this.Count == 1 && myDigits[0].First() == '(' && myDigits[0].Last() == ')')
                {
                    myDigits[0] = myDigits[0].Substring(1, myDigits[0].Length - 2);
                }
                return myDigits;
            }
        }


        /// <summary>
        /// Number of operations it took to get to this set of digits.
        /// Used to determine the shortest path.
        /// </summary>
        public int Depth
        {
            get
            {
                if (Previous != null) return Previous.Depth + 1;
                return 1;
            }
        }
    }
}
