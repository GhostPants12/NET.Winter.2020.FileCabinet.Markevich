using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public static class ValidatorBuilderExtension
    {
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return new CompositeValidator(new IRecordValidator[] {new FirstNameValidator(2, 30), new LastNameValidator(2, 30), new DateOfBirthValidator(new DateTime(1900, 1, 1), DateTime.Today),
                new CodeValidator(1, short.MaxValue), new LetterValidator(" "), new BalanceValidator(0, decimal.MaxValue),
            });
        }

        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return new CompositeValidator(new IRecordValidator[] { new FirstNameValidator(2, 60), new LastNameValidator(2, 60), new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Today),
                new CodeValidator(0, short.MaxValue), new LetterValidator(" "), new BalanceValidator(0, decimal.MaxValue),
            });
        }
    }
}
