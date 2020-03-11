using System;
using System.Globalization;
using System.IO;
using FileCabinetGenerator.Generator;

namespace FileCabinetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputType = "unassigned";
            string path = "unassigned";
            int recordsAmount=0;
            int startId=0;
            if (args.Length >= 1)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Contains("--output-type", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var argsArr = args[i].Split('=');
                        outputType = argsArr[1];
                    }

                    if (args[i].Contains("--output", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var argsArr = args[i].Split('=');
                        path = argsArr[1];
                    }

                    if (args[i].Contains("--records-amount", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var argsArr = args[i].Split('=');
                        if (!Int32.TryParse(argsArr[1], out recordsAmount))
                        {
                            throw new ArgumentException($"Incorrect records amount: {argsArr[1]}");
                        }

                        if (recordsAmount <= 0)
                        {
                            throw new ArgumentOutOfRangeException($"Records amount is less or equal to zero.");
                        }
                    }

                    if (args[i].Contains("--start-id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var argsArr = args[i].Split('=');
                        if (!Int32.TryParse(argsArr[1], out startId))
                        {
                            throw new ArgumentException($"Incorrect start ID: {argsArr[1]}");
                        }

                        if (startId < 0)
                        {
                            throw new ArgumentOutOfRangeException($"Start ID is less than zero.");
                        }
                    }

                    if (args[i].Equals("-t", StringComparison.InvariantCultureIgnoreCase))
                    {
                        i++;
                        outputType = args[i];
                    }

                    if (args[i].Equals("-o", StringComparison.InvariantCultureIgnoreCase))
                    {
                        i++;
                        path = args[i];
                    }

                    if (args[i].Equals("-a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        i++;
                        if (!Int32.TryParse(args[i], out recordsAmount))
                        {
                            throw new ArgumentException($"Incorrect records amount: {args[i]}");
                        }

                        if (recordsAmount <= 0)
                        {
                            throw new ArgumentOutOfRangeException($"Records amount is less or equal to zero.");
                        }
                    }

                    if (args[i].Equals("-i", StringComparison.InvariantCultureIgnoreCase))
                    {
                        i++;
                        if (!Int32.TryParse(args[i], out startId))
                        {
                            throw new ArgumentException($"Incorrect start ID: {args[i]}");
                        }

                        if (startId < 0)
                        {
                            throw new ArgumentOutOfRangeException($"Start ID is less than zero.");
                        }
                    }
                }
            }

            if (File.Exists(path))
            {
                while (true)
                {
                    Console.Write($"File is exist - rewrite {path} [Y/n] ");
                    string answer = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                    Console.WriteLine();
                    if (answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }

                    if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return;
                    }
                }
            }

            switch (outputType)
            {
                case "csv":
                    using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create)))
                    {
                        new CSVWriter(sw).Generate(RecordGenerator.Generate(recordsAmount, startId));
                    }
                    Console.WriteLine($"{recordsAmount} records were written to {path}");
                    break;
                case "xml":
                    break;
                default:
                    throw new ArgumentException($"Incorrect output type: {outputType}");
            }
        }
    }
}
