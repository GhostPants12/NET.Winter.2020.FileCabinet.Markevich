using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordValidator
{
    /// <summary>Builder class for the validator.</summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>Validates the first name.</summary>
        /// <param name="min">The minimum length.</param>
        /// <param name="max">The maximum length.</param>
        public void ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
        }

        /// <summary>Validates the last name.</summary>
        /// <param name="min">The minimum length.</param>
        /// <param name="max">The maximum length.</param>
        public void ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
        }

        /// <summary>Validates the date of birth.</summary>
        /// <param name="from">The minimum date.</param>
        /// <param name="to">The maximum date.</param>
        public void ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
        }

        /// <summary>Validates the code.</summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public void ValidateCode(short min, short max)
        {
            this.validators.Add(new CodeValidator(min, max));
        }

        /// <summary>Validates the letter.</summary>
        /// <param name="unsupported">The unsupported characters.</param>
        public void ValidateLetter(string unsupported)
        {
            this.validators.Add(new LetterValidator(unsupported));
        }

        /// <summary>Validates the balance.</summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public void ValidateBalance(decimal min, decimal max)
        {
            this.validators.Add(new BalanceValidator(min, max));
        }

        /// <summary>Creates the instance of CompositeValidator.</summary>
        /// <returns>Returns the CompositeValidator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
