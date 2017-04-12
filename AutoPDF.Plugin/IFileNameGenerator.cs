using System.Collections.Generic;

namespace AutoPDF.Plugin
{
    /// <summary>
    /// Implemented by plugins that generate an output file name for completed PDF forms.
    /// </summary>
    public interface IFileNameGenerator
    {
        /// <summary>
        /// Generates an output file name for a completed PDF form.
        /// </summary>
        /// <param name="formFields">A dictionary of form field names and values from input data</param>
        /// <param name="currentIndex">An index representing the current input data record number</param>
        /// <param name="numRecords">The total number of input data records</param>
        /// <param name="templateFile">The file name of the PDF template file</param>
        /// <param name="inputFile">The file name of the input data file</param>
        /// <returns>A file name that can be used to save the completed form</returns>
        string GenerateFileName(IDictionary<string, object> formFields, int currentIndex, int numRecords, string templateFile, string inputFile);
    }
}
