using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "create", "creates a record in the list", "The 'create' command leads to the screen where records can be created" },
            new string[] { "insert", "inserts record with specified data in the list", "The 'insert' command leads to the screen where records can be inserted"},
            new string[] { "update", "updates a record in the list", "The 'update' command leads to the screen where you can recreate the record" },
            new string[] { "selects", "selects the records with the specified parameters", "The 'select' command leads to the screen where records with specified parameters can be selected" },
            new string[] { "delete", "removes record with a specified conditions from the file cabinet", "The 'delete' command leads to the screen where records can be removed"},
            new string[] { "purge", "cleans up records' list by removing deleted records", "The 'purge' command leads to the screen where records are purged"},
            new string[] { "stat", "prints the records' statistics", "The 'stat' command prints the count of the list." },
            new string[] { "export", "exports the data to csv or xml format", "The 'export' command leads to the screen where records can be exported" },
            new string[] { "import", "imports the data to csv or xml format", "The 'import' command leads to the screen where records can be imported" },
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            if (!string.IsNullOrEmpty(request.Parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], request.Parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{request.Parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
