using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for help command.</summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "create", "creates a record in the list", "The 'create' command leads to the screen where records can be created" },
            new string[] { "insert", "inserts record with specified data in the list", "The 'insert' command leads to the screen where records can be inserted" },
            new string[] { "update", "updates a record in the list", "The 'update' command leads to the screen where you can recreate the record" },
            new string[] { "selects", "selects the records with the specified parameters", "The 'select' command leads to the screen where records with specified parameters can be selected" },
            new string[] { "delete", "removes record with a specified conditions from the file cabinet", "The 'delete' command leads to the screen where records can be removed" },
            new string[] { "purge", "cleans up records' list by removing deleted records", "The 'purge' command leads to the screen where records are purged" },
            new string[] { "stat", "prints the records' statistics", "The 'stat' command prints the count of the list." },
            new string[] { "export", "exports the data to csv or xml format", "The 'export' command leads to the screen where records can be exported" },
            new string[] { "import", "imports the data to csv or xml format", "The 'import' command leads to the screen where records can be imported" },
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            if (request != null && !string.IsNullOrEmpty(request.Parameters))
            {
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], request.Parameters, StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine(index >= 0
                    ? HelpMessages[index][ExplanationHelpIndex]
                    : $"There is no explanation for '{request.Parameters}' command.");
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
