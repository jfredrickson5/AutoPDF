# AutoPDF

A command-line tool that batch fills a PDF form multiple times based on input from a delimited text file.

## Plugins

AutoPDF can be extended via plugins. Just create a new library that references [AutoPDF.Plugins](https://www.nuget.org/packages/AutoPDF.Plugin/), and implement any plugin interface that you need:

* `IFileNameGenerator` - determines what the output PDF file names will be
* `IInputParser` - parses values from the input data
