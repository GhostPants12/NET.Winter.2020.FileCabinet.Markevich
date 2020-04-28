using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for the create command.</summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        /// <summary>Initializes a new instance of the <see cref="CreateCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            int id;
            string firstName;
            string lastName;
            short code;
            char letter;
            decimal balance;
            DateTime dateOfBirth;
            try
            {
                Console.Write("First Name: ");
                firstName = Program.ReadInput<string>(Program.ConvertStringToString, Program.ValidateName);
                Console.Write("Last Name: ");
                lastName = Program.ReadInput<string>(Program.ConvertStringToString, Program.ValidateName);
                Console.Write("Code: ");
                code = Program.ReadInput<short>(Program.ConvertStringToShort, Program.ValidateCode);
                Console.Write("Letter: ");
                letter = Program.ReadInput<char>(Program.ConvertStringToChar, Program.ValidateLetter);
                Console.Write("Balance: ");
                balance = Program.ReadInput<decimal>(Program.ConvertStringToDecimal, Program.ValidateBalance);
                Console.Write("Date of birth: ");
                dateOfBirth = Program.ReadInput<DateTime>(Program.ConvertStringToDate, Program.ValidateDate);
                RecordData recordDataToCreate = new RecordData(firstName, lastName, code, letter, balance, dateOfBirth);
                id = this.service.CreateRecord(recordDataToCreate);
                Console.WriteLine($"Record #{id} has been created.");
            }
            #pragma warning disable CA1031 // Do not catch general exception types
            catch
            #pragma warning restore CA1031 // Do not catch general exception types
            {
                this.Handle(request);
            }
        }
    }
}
