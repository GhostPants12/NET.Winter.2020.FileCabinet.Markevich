using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
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
                id = Program.fileCabinetService.CreateRecord(recordDataToCreate);
                Console.WriteLine($"Record #{id} has been created.");
            }
            catch (Exception)
            {
                Handle(request);
            }
        }
    }
}
