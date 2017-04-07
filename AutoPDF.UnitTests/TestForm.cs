using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Forms;

namespace AutoPDF.UnitTests
{
    [TestClass]
    public class TestForm
    {
        private string formFixture;
        private Form form;

        [TestInitialize]
        public void Initialize()
        {
            formFixture = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fixtures", "TestForm.pdf");
            form = new Form(formFixture);
        }

        [TestMethod]
        public void TestGetStreamUnfilled()
        {
            var stream = form.GetStream();
            Assert.IsNotNull(stream);
            stream.Flush();
            stream.Position = 0;
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(stream)))
            {
                var pdfForm = PdfAcroForm.GetAcroForm(pdfDocument, false);
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 1"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 2"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 3"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 1"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 2"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 3"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Option"].GetValueAsString());
            }
        }

        [TestMethod]
        public void TestGetStreamFieldsFilledViaConstructor()
        {
            var fields = new Dictionary<string, object>();
            fields.Add("Text Box 1", "abcde");
            fields.Add("Check Box 1", "Yes");
            fields.Add("Option", "2");
            form = new Form(formFixture, fields);
            var stream = form.GetStream();
            Assert.IsNotNull(stream);
            stream.Flush();
            stream.Position = 0;
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(stream)))
            {
                var pdfForm = PdfAcroForm.GetAcroForm(pdfDocument, false);
                Assert.AreEqual("abcde", pdfForm.GetFormFields()["Text Box 1"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 2"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 3"].GetValueAsString());
                Assert.AreEqual("Yes", pdfForm.GetFormFields()["Check Box 1"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 2"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 3"].GetValueAsString());
                Assert.AreEqual("2", pdfForm.GetFormFields()["Option"].GetValueAsString());
            }
        }

        [TestMethod]
        public void TestGetStreamFieldsFilledViaProperty()
        {
            form.Fields.Add("Text Box 1", "abcde");
            form.Fields.Add("Check Box 1", "Yes");
            form.Fields.Add("Option", "2");
            var stream = form.GetStream();
            Assert.IsNotNull(stream);
            stream.Flush();
            stream.Position = 0;
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(stream)))
            {
                var pdfForm = PdfAcroForm.GetAcroForm(pdfDocument, false);
                Assert.AreEqual("abcde", pdfForm.GetFormFields()["Text Box 1"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 2"].GetValueAsString());
                Assert.AreEqual("", pdfForm.GetFormFields()["Text Box 3"].GetValueAsString());
                Assert.AreEqual("Yes", pdfForm.GetFormFields()["Check Box 1"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 2"].GetValueAsString());
                Assert.AreEqual("Off", pdfForm.GetFormFields()["Check Box 3"].GetValueAsString());
                Assert.AreEqual("2", pdfForm.GetFormFields()["Option"].GetValueAsString());
            }
        }

        [TestMethod]
        public void TestGetBytesUnfilled()
        {
            var data = form.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
        }

        [TestMethod]
        public void TestGetCheckboxStateValuesForCheckbox()
        {
            var values = form.GetFieldStateValues("Check Box 1");
            Assert.AreEqual(2, values.Length);
            CollectionAssert.Contains(values, "Yes");
            CollectionAssert.Contains(values, "Off");
        }

        [TestMethod]
        public void TestGetCheckboxStateValuesForOptions()
        {
            var values = form.GetFieldStateValues("Option");
            Assert.AreEqual(4, values.Length);
            CollectionAssert.Contains(values, "1");
            CollectionAssert.Contains(values, "2");
            CollectionAssert.Contains(values, "3");
            CollectionAssert.Contains(values, "Off");
        }
    }
}
