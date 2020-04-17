using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class MissedCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
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

            Console.WriteLine();
        }

        private string[] commands = new[]
        {
            "create", "delete", "exit", "export", "find", "help", "import", "insert", "list", "stat", "purge", "update"
        };
    }
}
