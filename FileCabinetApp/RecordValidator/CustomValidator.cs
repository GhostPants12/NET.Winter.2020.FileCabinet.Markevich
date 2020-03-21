using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Validator for custom file cabinet.</summary>
    /// <seealso cref="FileCabinetApp.RecordValidator.IRecordValidator" />
    public class CustomValidator : CompositeValidator
    {
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 30), new LastNameValidator(2, 30), new DateOfBirthValidator(new DateTime(1900, 1, 1), DateTime.Today),
                new CodeValidator(1, short.MaxValue), new LetterValidator(" "), new BalanceValidator(0, decimal.MaxValue),
            })
        {
        }
    }
}
