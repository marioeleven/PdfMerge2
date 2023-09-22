using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfMerge
{
    class Program
    {
        public static void Main(string[] args)
        {
            string rootPath = @"C:\tempTestPdf2\"; // C:\temp23

            foreach (var directory in Directory.GetDirectories(rootPath))
            {
                MergePdfFiles(directory);
            }
        }
        
        static void MergePdfFiles(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.pdf");

            if (files.Length > 1)
            {
                string outputFile = Path.Combine(directoryPath, Path.GetFileName(directoryPath) + "_merged.pdf");

                using (FileStream stream = new FileStream(outputFile, FileMode.Create))
                {
                    Document pdfDoc = new Document();
                    PdfCopy pdf = new PdfCopy(pdfDoc, stream);

                    pdfDoc.Open();

                    foreach (string file in files.OrderBy(f => f))
                    {
                        // FileInfo fileInfo = new FileInfo(file);
                        // if (fileInfo.Length == 0)
                        // {
                        //     File.AppendAllText("C:\\tempTestPdf2\\zero_size_files.txt", $"Zero size file found: {file} in folder: {directoryPath}\n");
                        //     continue; // Skip this file and proceed with the next one
                        // }
                        
                        try
                        {
                            pdf.AddDocument(new PdfReader(file));
                        }
                        catch (iTextSharp.text.exceptions.InvalidPdfException ex)
                        {
                            Console.WriteLine($"Invalid PDF file: {file}. Error: {ex.Message}");
                            File.AppendAllText("C:\\tempTestPdf2\\merge_log.txt", $"Failed to merge PDF files in folder: {directoryPath}\n");
                            return;
                        }
                    }

                    if (pdfDoc != null)
                        pdfDoc.Close();

                    Console.WriteLine("Merged file created: " + outputFile);
                    
                    File.AppendAllText("C:\\tempTestPdf2\\merge_log.txt", $"Merged PDF files in folder: {directoryPath}\n");
                }
            }
        }
    }
}