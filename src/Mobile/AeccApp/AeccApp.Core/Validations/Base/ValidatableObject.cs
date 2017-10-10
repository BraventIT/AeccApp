using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Validations
{
    public class ValidatableObject<T> : BaseNotifyProperty, IValidity
    {
        #region Properties
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Set(ref _value, value))
                {
                    Validate();
                }
            }
        }


        private List<string> _errors;

        public List<string> Errors
        {
            get { return _errors; }
            set { Set(ref _errors, value); }
        }

        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set { Set(ref _isValid , value); }
        }

        private readonly List<IValidationRule<T>> _validations;

        public List<IValidationRule<T>> Validations =>  _validations;
        #endregion

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _validations
                .Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
