namespace AutoPDF
{
    class DefaultInputParser : IInputParser
    {
        public string ParseValue(string fieldName, string fieldValue)
        {
            return fieldValue;
        }
    }
}
