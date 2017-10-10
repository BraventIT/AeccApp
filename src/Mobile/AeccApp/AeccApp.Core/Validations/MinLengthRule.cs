using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Validations
{
    /// <summary>
  /// Validation rule. Case : Minimum lenght not reached
  /// </summary>
    public class MinLengthRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; }

        private readonly int _minLeght;

        public MinLengthRule(int minLeght)
        {
            _minLeght = minLeght;
        }

        public bool Check(string value)
        {
            return value?.Length >= _minLeght;
        }
    }
}
