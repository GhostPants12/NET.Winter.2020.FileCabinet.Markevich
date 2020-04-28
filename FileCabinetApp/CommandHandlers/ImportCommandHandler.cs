using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>CommandHandler for the import command.</summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        /// <summary>Initializes a new instance of the <see cref="ImportCommandHandler" /> class.</summary>
        /// <param name="service">The service.</param>
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request != null && !request.Command.Equals("import", StringComparison.InvariantCultureIgnoreCase))
            {
                this.NextHandler.Handle(request);
                return;
            }

            if (request != null)
            {
                string[] parametersArray = request.Parameters.Split(' ');
                string formatName = parametersArray[0];
                string path = parametersArray[1];
                try
                {
                    if (formatName.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open)))
                        {
                            FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
                            snapshot.LoadFromCsv(sr);
                            this.service.Restore(snapshot);
                            sr.Close();
                        }

                        Console.WriteLine("All records were imported.");
                    }

                    if (formatName.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
                            snapshot.LoadFromXml(fs);
                            this.service.Restore(snapshot);
                            fs.Close();
                        }

                        Console.WriteLine("All records were imported.");
                    }
                }
                #pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                #pragma warning restore CA1031 // Do not catch general exception types
                {
                    Console.WriteLine("Import failed: " + ex.Message);
                }
            }
        }
    }
}
