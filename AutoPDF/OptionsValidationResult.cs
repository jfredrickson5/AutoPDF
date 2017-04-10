namespace AutoPDF
{
    class OptionsValidationResult
    {
        public bool Valid { get; set; }
        public string Message { get; set; }

        public OptionsValidationResult() { }

        public OptionsValidationResult(bool valid) : this()
        {
            Valid = valid;
        }

        public OptionsValidationResult(bool valid, string message) : this(valid)
        {
            Message = message;
        }
    }
}
