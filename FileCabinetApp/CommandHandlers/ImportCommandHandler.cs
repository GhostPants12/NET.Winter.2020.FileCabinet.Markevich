using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {

        }

        public override void Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("import", StringComparison.InvariantCultureIgnoreCase))
            {
                this.nextHandler.Handle(request);
                return;
            }

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
            catch (Exception ex)
            {
                Console.WriteLine("Import failed: " + ex.Message);
            }
        }
    }
}
