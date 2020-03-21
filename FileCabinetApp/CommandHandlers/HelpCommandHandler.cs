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
            new string[] { "edit", "edits a record in the list", "The 'edit' command leads to the screen where you can recreate the record" },
            new string[] { "find", "finds a record with a specified property and its specified value", "The 'find' command leads to the screen where records can be found" },
            new string[] { "remove", "removes record with a specified index from the cabinet", "The 'remove' command leads to the screen where records can be deleted"},
            new string[] { "purge", "cleans up records' list by removing deleted records", "The 'purge' command leads to the screen where records are purged"},
            new string[] { "stat", "prints the records' statistics", "The 'stat' command prints the count of the list." },
            new string[] { "list", "gets the list of the records", "The 'list' command prints out all the records in list." },
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
