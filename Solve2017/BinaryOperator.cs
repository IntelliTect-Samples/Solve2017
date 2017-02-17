using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve2017
{
    public class BinaryOperator
    {
        public BinaryOperator(string operatorString, Func<double, double, double> operation)
        {
            OperatorString = operatorString;
            Operation = operation;
        }

        public string OperatorString { get; set; }

        public Func<double, double, double> Operation { get; set; }

    }
}
