using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using System;
using System.IO;

namespace Crefinso.Services
{
    public class PDFReceipts
    {
        public void DownloadPDFReceipt(IJSRuntime JS, string filename = "ReportePago.pdf")
        {
            JS.InvokeVoidAsync("DownloadPDF", filename, Convert.ToBase64String(PDFReport()));
        }

        private byte[] PDFReport()
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    float marginLeft = 36;
                    float marginTop = 36;
                    float marginRight = 36;
                    float marginBottom = 36;

                    // Configuración del documento PDF
                    Document pdf = new Document(PageSize.A4, marginLeft, marginRight, marginTop, marginBottom);
                    pdf.AddTitle("Reporte de Pago");
                    pdf.AddAuthor("CREFINSO");
                    pdf.AddCreationDate();

                    PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);
                    pdf.Open();

                    // Fuentes personalizadas
                    var headerFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.Black);
                    var subHeaderFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.Black);
                    var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.Black);
                    var boldFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.Black);
                    var redFont = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.Red);

                    // Encabezado con logo y título
                    var logo = Image.GetInstance("https://firebasestorage.googleapis.com/v0/b/scrapcode2021.appspot.com/o/logoCrefinsowhite.png?alt=media&token=0e5fe103-7720-42e1-9f17-e0522c1b9357");
                    logo.ScaleAbsolute(100, 50);
                    logo.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(logo);

                    var header = new Paragraph("ASOCIACIÓN COOPERATIVA DE AHORRO Y CRÉDITO\nCRECIMIENTO FINANCIERO DE SONSONATE DE R.L.", headerFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 10f
                    };
                    pdf.Add(header);

                    // Recibo de Ingreso
                    var receiptBox = new PdfPTable(1) { WidthPercentage = 100 };
                    var cell = new PdfPCell(new Phrase("RECIBO DE INGRESO", redFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = new BaseColor(255, 0, 0), // Rojo de fondo
                        Border = PdfPCell.NO_BORDER,
                        Padding = 10
                    };
                    receiptBox.AddCell(cell);
                    cell = new PdfPCell(new Phrase("912", redFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Border = PdfPCell.NO_BORDER,
                        Padding = 10
                    };
                    receiptBox.AddCell(cell);
                    pdf.Add(receiptBox);

                    // Comprobante de pago
                    var paymentHeader = new Paragraph("COMPROBANTE DE PAGO DE PRÉSTAMO", boldFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 10f,
                        SpacingAfter = 10f
                    };
                    pdf.Add(paymentHeader);

                    // Información del cliente
                    var clientInfo = new PdfPTable(new float[] { 1, 2 }) { WidthPercentage = 100, SpacingAfter = 10f };
                    clientInfo.AddCell(new PdfPCell(new Phrase("Nombre", boldFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("ALI ALEXANDER HERNANDEZ MARTIN", bodyFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("Código", boldFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("1104010101019", bodyFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("Fecha", boldFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("30 DE DICIEMBRE 2024", bodyFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("Pago de Cuota", boldFont)) { Border = PdfPCell.NO_BORDER });
                    clientInfo.AddCell(new PdfPCell(new Phrase("$100.00", bodyFont)) { Border = PdfPCell.NO_BORDER });
                    pdf.Add(clientInfo);

                    // Detalles del pago
                    var paymentDetails = new PdfPTable(new float[] { 1, 1 }) { WidthPercentage = 100, SpacingBefore = 10f, SpacingAfter = 10f };
                    paymentDetails.AddCell(new PdfPCell(new Phrase("SALDO ANTERIOR", boldFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
                    paymentDetails.AddCell(new PdfPCell(new Phrase("$ 673.93", bodyFont)));
                    paymentDetails.AddCell(new PdfPCell(new Phrase("CAPITAL", boldFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
                    paymentDetails.AddCell(new PdfPCell(new Phrase("$ 73.20", bodyFont)));
                    paymentDetails.AddCell(new PdfPCell(new Phrase("INTERES", boldFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
                    paymentDetails.AddCell(new PdfPCell(new Phrase("$ 26.80", bodyFont)));
                    paymentDetails.AddCell(new PdfPCell(new Phrase("NUEVO SALDO", boldFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
                    paymentDetails.AddCell(new PdfPCell(new Phrase("$ 600.73", bodyFont)));
                    pdf.Add(paymentDetails);

                    // Firmas
                    var signatures = new PdfPTable(new float[] { 1, 1 }) { WidthPercentage = 100, SpacingBefore = 20f };
                    signatures.AddCell(new PdfPCell(new Phrase("F. Oscar Armando Zelidón Gil\nRecibió", bodyFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    signatures.AddCell(new PdfPCell(new Phrase("F. Ali Alexander Hernandez Martínez\nEntregó", bodyFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdf.Add(signatures);

                    pdf.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
