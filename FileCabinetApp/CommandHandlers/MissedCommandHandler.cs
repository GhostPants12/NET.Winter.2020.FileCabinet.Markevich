using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for the missed command.</summary>
    public class MissedCommandHandler : CommandHandlerBase
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        /// <summary>The commands.</summary>
        private readonly string[] commands = new[]
        {
            "create", "delete", "exit", "export", "help", "import", "insert", "select", "stat", "purge", "update",
        };

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null)
            {
                Console.WriteLine($"There is no '{request.Command}' command.");
                Console.WriteLine("The most similar commands are:");
                Dictionary<int, List<string>> stringsDictionary = new Dictionary<int, List<string>>();
                foreach (var command in this.commands)
                {
                    int key = 0;
                    foreach (var character in request.Command)
                    {
                        if (command.Contains(character, StringComparison.InvariantCultureIgnoreCase))
                        {
                            key++;
                        }
                    }

                    if (!stringsDictionary.ContainsKey(key))
                    {
                        stringsDictionary[key] = new List<string>();
                    }

                    stringsDictionary[key].Add(command);
                }

                var counter = 0;
                for (int i = request.Command.Length; i > 0; i--)
                {
                    if (stringsDictionary.ContainsKey(i))
                    {
                        foreach (var element in stringsDictionary[i])
                        {
                            if (counter > 4)
                            {
                                break;
                            }

                            Console.WriteLine($"    {element}");
                            counter++;
                        }
                    }
                }
            }

            Console.WriteLine();
        }
    }
}
