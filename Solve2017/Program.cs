using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve2017
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            var intermediateResults = new IntermediateResults();
            
            string userName = "Administrator";
            string password = "LongPasswordThatIsSecure!";

            var unaryOperators = new List<UnaryOperator>()
            {
                new UnaryOperator("({0})!", (arg) => Factorial(arg), (arg) => FactorialIsValid(arg)),
                new UnaryOperator("({0})!!", (arg) => DoubleFactorial(arg), (arg) => FactorialIsValid(arg)),
                //new UnaryOperator("({0})!!!", (arg) => TrippleFactorial(arg), (arg) => FactorialIsValid(arg)),
                new UnaryOperator("sqrt({0})", (arg) => Math.Sqrt(arg), (arg) => arg>=0 ? true: false),
                //new UnaryOperator("abs({0})", (arg) => Math.Abs(arg), (arg) => arg>=0 ? true: false)
            };

            var binaryOperators = new List<BinaryOperator>()
            {
                new BinaryOperator("({0} + {1})", (arg1, arg2) => arg1+arg2),
                new BinaryOperator("({0} - {1})", (arg1, arg2) => arg1-arg2),
                new BinaryOperator("({0} * {1})", (arg1, arg2) => arg1*arg2),
                new BinaryOperator("({0} / {1})", (arg1, arg2) => arg1/arg2),
                new BinaryOperator("({0} ^ {1})", (arg1, arg2) => Math.Pow(arg1,arg2)),
                // This doesn't work because there isn't a simple modulus operator in the Math library. And Mod isn't allowed in this solution.
                //new BinaryOperator("({0}%{1})", (arg1, arg2) => Mod(arg1,arg2)),
            };

            var start = new List<string>() { "2", "0", "1", "7" };

            // Generate all the Permutations
            foreach (var nums in GetPermutations(start))
            {
                //Console.WriteLine(string.Join(" ", nums));
                intermediateResults.Add(new Digits(nums));
                AddAllDecimals(nums.ToList(), intermediateResults);

                // Combine numbers
                var results = CombineOne(nums.ToList());

                foreach (var result in results)
                {
                    intermediateResults.Add(new Digits(result));

                    // Handle the decimal point
                    AddAllDecimals(result, intermediateResults);

                    // Get combinations
                    //foreach (var combo in GetAllCombos(result))
                    //{
                    //    intermediateResults.Add(new Digits(combo));
                    //}
                }
            }
           
            Console.WriteLine($"{userName}: {password}");
            
            //Handle Negatives
            //foreach (var digits in intermediateResults.Values.ToArray() )
            //{
            //    AddAllNegatives(digits.OriginalDigits, intermediateResults);
            //}


            //foreach (var result in intermediateResults)
            //{
            //    Console.WriteLine($"{result.Key} ");
            //}



            bool gotResults = false;
            do
            {
                var thisIterationList = intermediateResults.Values.Where(f => !f.IsProcessed).ToArray();
                //var thisIterationList = intermediateResults.Values.ToArray();
                gotResults = false;
                // Handle unary  sqrt, factorial, etc. 
                foreach (Digits digits in thisIterationList)
                {
                    foreach (UnaryOperator unaryOperator in unaryOperators)
                    {
                        for (int i = 0; i < digits.Count; i++)
                        {
                            gotResults |= intermediateResults.Add(digits, unaryOperator, i);
                        }
                    }
                }

                // Handle binary  +, -, *, etc.
                foreach (Digits digits in thisIterationList)
                {
                    foreach (BinaryOperator binaryOperator in binaryOperators)
                    {
                        for (int i = 1; i < digits.Count; i++)
                        {
                            gotResults |= intermediateResults.Add(digits, binaryOperator, i);
                        }
                    }
                    digits.IsProcessed = true;
                }
                // Loop until we don't find any new digit combinations. 
            } while (gotResults);

            // Get a list of all numbers found. 
            var finalResult = new List<Digits>();
            foreach (var result in intermediateResults)
            {
                //Console.WriteLine($"{result.Key} ");
                if (result.Value.Count == 1)
                {
                    finalResult.Add(result.Value);
                }
            }

            int count = 0;
            for (double i = 1; i <= 100; i++)
            {
                var digit = finalResult.FirstOrDefault(f => f[0] == i);
                if (digit != null)
                {
                    count++;
                    var value = digit[0];
                    var expression = digit.DigitCalculations[0];
                    Console.WriteLine($"{ i } = { expression }");
                }
                else
                {
                    Console.WriteLine($"{ i } = ?");
                }
            }

            Console.WriteLine($"Total between 1 and 100: {count}");
            Console.WriteLine($"Total found: {finalResult.Count}");
            Console.WriteLine($"Total Time: {(DateTime.Now - startTime).TotalMilliseconds}ms");
            Console.ReadLine();

        }

        private static List<List<string>> CombineOne(List<string> digits)
        {
            var results = new List<List<string>>();

            for (int x = 0; x < digits.Count() - 1; x++)
            {
                var result = new List<string>();
                for (int y = 0; y < digits.Count(); y++)
                {
                    if (x == y)
                    {
                        // Concatenate this digit and the next one
                        var newItem = digits[y] + digits[y + 1];
                        result.Add(newItem);
                    }
                    else if (y == x + 1)
                    {
                        // Do nothing
                    }
                    else
                    {
                        // just add the digit
                        result.Add(digits[y]);
                    }
                }
                results.Add(result);
                if (results.Count > 1) results.AddRange(CombineOne(result));
            }

            return results;
        }

        public static double Factorial(double arg)
        {
            if (arg == 0) return 1;
            double result = 1;
            for (double x = arg; x > 1; x--)
            {
                result *= x;
            }
            return result;
        }

        public static double DoubleFactorial(double arg)
        {
            if (arg == 0) return 1;
            double result = 1;
            for (double x = arg; x > 1; x = x - 2)
            {
                result *= x;
            }
            return result;
        }
        public static double TrippleFactorial(double arg)
        {
            if (arg == 0) return 1;
            double result = 1;
            for (double x = arg; x > 1; x = x - 3)
            {
                result *= x;
            }
            return result;
        }

        public static bool FactorialIsValid(double arg)
        {
            if (arg < 0) return false;
            if ((int)arg == arg) return true;
            return false;
        }


        /// <summary>
        /// Gets all permutations of the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        static IEnumerable<List<T>> GetPermutations<T>(IEnumerable<T> list)
        {
            var result = new Dictionary<string, List<T>>();
            if (list.Count() == 1)
            {
                result.Add("", list.ToList());
            }
            else
            {
                foreach (var c in list)
                {
                    var allButThis = new List<T>(list);
                    allButThis.Remove(c);
                    var childPermutations = GetPermutations(allButThis);
                    foreach (var child in childPermutations)
                    {
                        child.Insert(0, c);
                        var key = new string(child.Select(f => Convert.ToChar(f)).ToArray());
                        if (!result.ContainsKey(key)) result.Add(key, child);
                    }
                }
            }
            return result.Values;
        }

        // Copied from Stack Overflow
        public static List<List<T>> GetAllCombos<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            // head
            result.Add(new List<T>());
            result.Last().Add(list[0]);
            if (list.Count == 1)
                return result;
            // tail
            List<List<T>> tailCombos = GetAllCombos(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }


        /// <summary>
        /// Adds all combinations of decimals.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="intermediateResults"></param>
        public static void AddAllDecimals(List<string> list, IntermediateResults intermediateResults)
        {
            for (int x = 0; x < list.Count; x++)
            {
                var newList = new List<string>(list);
                var digit = list[x];
                if (!digit.Contains("."))
                {
                    int start = 0;
                    if (digit.StartsWith("-")) start = 1;
                    for (int i = start; i < digit.Length; i++)
                    {
                        var newDigit = $"{digit.Substring(0, i)}.{digit.Substring(i, digit.Length - i)}";
                        newList[x] = newDigit;
                        if (intermediateResults.Add(new Digits(newList)))
                        {
                            AddAllDecimals(newList, intermediateResults);
                        }
                    }
                }
            }
        }

        //public static void AddAllNegatives(List<string> list, IntermediateResults intermediateResults)
        //{
        //    for (int x = 0; x < list.Count; x++)
        //    {
        //        var newList = new List<string>(list);
        //        var digit = list[x];
        //        if (!digit.Contains("-"))
        //        {
        //            var newDigit = $"-{digit}";
        //            newList[x] = newDigit;
        //            if (intermediateResults.Add(new Digits(newList)))
        //            {
        //                AddAllNegatives(newList, intermediateResults);
        //            }
        //        }
        //    }
        //}
    }




}
