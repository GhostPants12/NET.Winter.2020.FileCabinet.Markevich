using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for export command.</summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="ExportCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            if (request != null)
            {
                string[] parametersArray = request.Parameters.Split(' ');
                if (parametersArray.Length < 2)
                {
                    throw new ArgumentException($"{request.Parameters} are incorrect parameters.");
                }

                string formatName = parametersArray[0];
                string path = parametersArray[1];
                try
                {
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

                    if (formatName.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create));
                        this.service.MakeSnapshot().SaveToCsv(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                        return;
                    }

                    if (formatName.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create));
                        this.service.MakeSnapshot().SaveToXml(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Export failed: " + ex.Message);
                }
            }
        }
    }
}
