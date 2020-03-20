using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public ExportCommandHandler(IFileCabinetService serivce)
        {
            this.service = serivce;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

            string[] parametersArray = request.Parameters.Split(' ');
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
                    using (StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create)))
                    {
                        this.service.MakeSnapshot().SaveToCsv(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                        return;
                    }
                }

                if (formatName.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create)))
                    {
                        this.service.MakeSnapshot().SaveToXml(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Export failed: " + ex.Message);
            }
        }
    }
}
