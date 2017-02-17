using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve2017
{
    /// <summary>
    /// Collection to manage all the intermediate arrays of numbers. 
    /// This takes care of removing duplicates and tracking the operations into the Digits objects.
    /// </summary>
    public class IntermediateResults : Dictionary<string, Digits>
    {
        public int MaxValue { get; set; } = 1000;

        public bool Add(Digits digits)
        {
            if (digits.Count == 1 && (int)digits[0] != digits[0])
            { return false; }
            if (!this.ContainsKey(digits.Key))
            {
                this.Add(digits.Key, digits);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to perform the operation on the digit.
        /// Returns true if a unique result is found. If not false.
        /// </summary>
        /// <param name="digits">Original digits to operate on</param>
        /// <param name="operation">Unary operation to perform</param>
        /// <param name="paramIndex">Index of the digit to perform operation on. 0 based.</param>
        /// <returns></returns>
        public bool Add(Digits digits, UnaryOperator operation, int paramIndex)
        {
            if (!operation.IsValid(digits[paramIndex])) return false;
            var result = new Digits(digits);
            result.OperatorString = operation.OperatorString;
            result.ArgumentIndex = paramIndex;
            for (int x = 0; x < digits.Count; x++)
            {
                if (x == paramIndex)
                {
                    double newDigit = operation.Operation(digits[x]);
                    // Make sure the new digit is whole and under MaxValue.
                    if ((int)(newDigit * MaxValue) == newDigit * MaxValue && newDigit < MaxValue && newDigit > -1 * MaxValue)
                    {
                        result.Add(newDigit);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    result.Add(digits[x]);
                }
            }

            return AddDigits(result);
        }

        /// <summary>
        /// Attempts to perform the operation on the digit.
        /// Returns true if a unique result is found. If not false.
        /// </summary>
        /// <param name="digits">Original digits to operate on</param>
        /// <param name="operation">Binary operation to perform</param>
        /// <param name="paramSpace">Space between two digits to operate on. 1 based.</param>
        /// <returns></returns>
        public bool Add(Digits digits, BinaryOperator operation, int paramSpace)
        {
            var result = new Digits(digits);
            result.OperatorString = operation.OperatorString;
            result.ArgumentIndex = paramSpace;
            for (int x = 0; x < digits.Count; x++)
            {
                if (x == paramSpace - 1)
                {
                    double newDigit = operation.Operation(digits[x], digits[x + 1]);
                    // Make sure the new digit is whole and under MaxValue;
                    if ((int)newDigit == newDigit && newDigit < MaxValue && newDigit > -1 * MaxValue)
                    {
                        result.Add(newDigit);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (x == paramSpace)
                {
                    // Do Nothing We handled this before.
                }

                else
                {
                    result.Add(digits[x]);
                }
            }

            return AddDigits(result);
        }

        /// <summary>
        /// Adds the digits to the collection if they are new or have a shorter path.
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public bool AddDigits(Digits digits)
        {
            if (digits.Count == 1 && (int)digits[0] != digits[0])
            { return false; }
            if (this.ContainsKey(digits.Key))
            {
                var existingDigits = this[digits.Key];
                existingDigits.UniqueCount++;
                if (digits.Depth < existingDigits.Depth)
                {
                    this[digits.Key] = digits;
                }
                return false;
            }
            this.Add(digits.Key, digits);
            return true;
        }

    }
}
