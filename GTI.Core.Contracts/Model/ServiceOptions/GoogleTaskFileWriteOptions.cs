namespace GTI.Core.Contracts.Model
{
    public class GoogleTaskFileWriteOptions
    {
        /// <summary>
        /// Path to the folder in which the files generated are to be saved.
        /// </summary>
        /// <remarks>At the moment, the files generated are being named after their list title.</remarks>
        public string OutputDirectory { get; set; }
    }
}