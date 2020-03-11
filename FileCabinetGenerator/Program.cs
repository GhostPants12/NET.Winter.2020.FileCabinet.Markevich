using System;
using System.IO;

namespace FileCabinetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator;
            string outputType = "unassigned";
            string path;
            int recordsAmount;
            int startId;
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

            switch (outputType)
            {
                case "csv":
                    generator = new CSVGenerator();
                    break;
                case "xml":
                    generator = new XMLGenerator();
                    break;
                default:
                    throw new ArgumentException($"Incorrect output type: {outputType}");
            }
        }
    }
}
