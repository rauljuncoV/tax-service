using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Services.Artifacs.Exceptions
{
    public class TaxServiceCalculatorException : Exception
    {
        public TaxServiceCalculatorException(string message) : base(message)
        {
        }
    }
}
