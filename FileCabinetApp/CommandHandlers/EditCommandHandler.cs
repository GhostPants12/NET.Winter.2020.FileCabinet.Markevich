using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            int id = 0;
            string firstName;
            string lastName;
            short code;
            char letter;
            decimal balance;
            DateTime dateOfBirth;
            try
            {
                Console.WriteLine("Id: ");
                id = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
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
                RecordData recordDataToEdit = new RecordData(firstName, lastName, code, letter, balance, dateOfBirth);
                recordDataToEdit.Id = id;
                Program.fileCabinetService.EditRecord(recordDataToEdit);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }
    }
}
