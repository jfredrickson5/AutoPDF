using System;
using System.Globalization;

namespace AutoPDF
{
    class PATSInputParser : IInputParser
    {
        public string ParseValue(string fieldName, string fieldValue)
        {
            if (String.IsNullOrEmpty(fieldValue))
            {
                return "";
            }

            var result = fieldValue;

            var checkboxFields = new string[]
            {
                "Performance Rating Cash Award",
                "Special Act Cash Award",
                "Spot Cash Award",
                "Time Off Award",
                "Time Off Award PR",
                "Suggestion Award",
                "Non - Monetary Award",
                "Special Act Cash Award Individual",
                "Special Act Cash Award Group"
            };

            var amountFields = new string[]
            {
                "Performance Rating Cash Award Amount",
                "Special Act Cash Award Individual Amount",
                "Special Act Cash Award Group Amount",
                "Spot Cash Award Amount",
                "Time Off Award PR Hours",
                "Time Off Award Hours",
                "Suggestion Award Amount",
                "Non-Monetary Award Type"
            };

            // If the value is "No", return an empty string (i.e., leave the field blank on the form).
            if (fieldValue.ToUpper().Equals("NO"))
            {
                foreach (var checkboxField in checkboxFields)
                {
                    if (fieldName.Equals(checkboxField))
                    {
                        result = "";
                        break;
                    }
                }
            }

            // If the value parses to zero, return an empty string (i.e., leave the field blank on the form).
            try
            {
                if (Decimal.Parse(fieldValue, NumberStyles.Currency) == 0)
                {
                    foreach (var amountField in amountFields)
                    {
                        if (fieldName.Equals(amountField))
                        {
                            result = "";
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
