namespace AutoPDF.Plugin
{
    /// <summary>
    /// Implemented by plugins that parse incoming records from an input data file.
    /// </summary>
    public interface IInputParser
    {
        /// <summary>
        /// Takes a field name and field value, and returns a parsed value.
        /// </summary>
        /// <param name="fieldName">Name of the form field</param>
        /// <param name="fieldValue">Value of the form field from input data</param>
        /// <returns>A parsed value</returns>
        string ParseValue(string fieldName, string fieldValue);
    }
}
