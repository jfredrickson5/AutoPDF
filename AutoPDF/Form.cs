using iText.Forms;
using iText.Kernel.Pdf;
using System.Collections.Generic;
using System.IO;

namespace AutoPDF
{
    public class Form
    {
        public IDictionary<string, object> Fields { get; set; }
        public FileInfo PdfTemplateFile { get; set; }

        public Form(string pdfTemplateFile)
        {
            PdfTemplateFile = new FileInfo(pdfTemplateFile);
        }

        public Form(FileInfo pdfTemplateFile)
        {
            PdfTemplateFile = pdfTemplateFile;
        }

        public Form(string pdfTemplateFile, IDictionary<string, object> fields) : this(pdfTemplateFile)
        {
            Fields = fields;
        }

        public Form(FileInfo pdfTemplateFile, IDictionary<string, object> fields) : this(pdfTemplateFile)
        {
            Fields = fields;
        }

        public MemoryStream GetStream()
        {
            var ms = new MemoryStream();
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(PdfTemplateFile.FullName), new PdfWriter(ms)))
            {
                var pdfForm = PdfAcroForm.GetAcroForm(pdfDocument, false);
                foreach (var field in Fields)
                {
                    pdfForm.GetField(field.Key).SetValue(field.Value.ToString());
                }
                pdfDocument.SetCloseWriter(false);
                pdfDocument.Close();
                return ms;
            }
        }

        public byte[] GetBytes()
        {
            using (var stream = GetStream())
            {
                return stream.ToArray();
            }
        }

        private string[] GetCheckboxStateValues(string fieldName)
        {
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(PdfTemplateFile.FullName)))
            {
                var form = PdfAcroForm.GetAcroForm(pdfDocument, false);
                var field = form.GetFormFields()[fieldName];
                var states = field.GetAppearanceStates();
                return states;
            }
        }
    }
}
