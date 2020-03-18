using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = Program.fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in arrayOfRecords)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
            }
        }
    }
}
