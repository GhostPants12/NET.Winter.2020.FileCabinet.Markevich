using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator for default file cabinet service.</summary>
    /// <seealso cref="FileCabinetApp.RecordValidator.IRecordValidator" />
    public class DefaultValidator : CompositeValidator
    {
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 60), new LastNameValidator(2, 60), new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Today),
                new CodeValidator(0, short.MaxValue), new LetterValidator(" "), new BalanceValidator(0, decimal.MaxValue),
            })
        {
        }
    }
}
