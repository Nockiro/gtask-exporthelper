using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using System.Collections.Generic;
using System.IO;

namespace GTI.Core.Services
{
    public class GoogleTaskFileWriter : IGoogleTaskWriter
    {
        private readonly IGoogleTaskToICalSerializer _taskSerializer;
        private readonly GoogleTaskFileWriteOptions _options;

        public GoogleTaskFileWriter(IGoogleTaskToICalSerializer taskSerializer, GoogleTaskFileWriteOptions options)
        {
            this._taskSerializer = taskSerializer;
            this._options = options;
        }

        public void Write(List<GoogleTaskList> listsToWrite)
        {
            if (!string.IsNullOrWhiteSpace(_options.OutputDirectory) && !Directory.Exists(_options.OutputDirectory))
            {
                Directory.CreateDirectory(_options.OutputDirectory);
            }

            foreach (GoogleTaskList list in listsToWrite)
            {
                File.WriteAllText(Path.Combine(_options.OutputDirectory, $"{list.Title}.ics"), _taskSerializer.Serialize(list));
            }
        }
    }
}