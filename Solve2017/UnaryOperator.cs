using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve2017
{
    public class UnaryOperator
    {
        public UnaryOperator(string operatorString, Func<double, double> operation, Func<double, bool> isValid)
        {
            OperatorString = operatorString;
            Operation = operation;
            IsValid = isValid;
        }

        public string OperatorString { get; set; }

        public Func<double, double> Operation { get; set; }

        public Func<double, bool> IsValid { get; set; }


    }
}
