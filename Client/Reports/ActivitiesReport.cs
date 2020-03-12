using Data.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Reports
{
    public class ActivitiesReport
    {
        #region Declaration
        int totalColumn = 4;
        Document _document;
        Font _fontStyles;
        PdfPTable _pdfTable = new PdfPTable(4);
        PdfPCell _pdfCell;
        MemoryStream _memoryStream = new MemoryStream();
        List<ToDoListVM> toDoLists = new List<ToDoListVM>();
        #endregion

        public byte[] PrepareReport(List<ToDoListVM> toDoListVMs)
        {
            toDoLists = toDoListVMs;

            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f,20f,20f,20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyles = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 20f, 150f, 100f, 100f});
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();
        }

        public void ReportHeader()
        {
            _fontStyles = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfCell = new PdfPCell(new Phrase("Activities Report", _fontStyles));
            _pdfCell.Colspan = totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            _fontStyles = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyles));
            _pdfCell.Colspan = totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
        }

        public void ReportBody()
        {
            #region Table header
            _fontStyles = FontFactory.GetFont("Tahoma", 11f, 1);

            _pdfCell = new PdfPCell(new Phrase("Id", _fontStyles));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Name", _fontStyles));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Status", _fontStyles));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Completed Time", _fontStyles));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            #endregion

            #region table body
            _fontStyles = FontFactory.GetFont("Tahoma", 8f, 0);
            int serialNumber = 1;
            foreach (ToDoListVM item in toDoLists)
            {
                _pdfCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyles));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(item.Name, _fontStyles));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(_pdfCell);

                if (item.Status==false)
                {
                    _pdfCell = new PdfPCell(new Phrase("Active", _fontStyles));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);

                    _pdfCell = new PdfPCell(new Phrase("-", _fontStyles));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                }
                else
                {
                    _pdfCell = new PdfPCell(new Phrase("Completed", _fontStyles));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);

                    _pdfCell = new PdfPCell(new Phrase(item.CompletedTime.ToString("MM/dd/yyyy"), _fontStyles));
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfCell);
                }
            }
            #endregion
        }
    }
}
