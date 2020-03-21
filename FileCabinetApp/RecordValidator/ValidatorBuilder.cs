using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public void ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
        }

        public void ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
        }

        public void ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
        }

        public void ValidateCode(short min, short max)
        {
            this.validators.Add(new CodeValidator(min, max));
        }

        public void ValidateLetter(string unsupported)
        {
            this.validators.Add(new LetterValidator(unsupported));
        }

        public void ValidateBalance(decimal min, decimal max)
        {
            this.validators.Add(new BalanceValidator(min, max));
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(validators);
        }
    }
}
