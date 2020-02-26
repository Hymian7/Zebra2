using Bytescout.PDF;
using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;

namespace Zebra.Archiving
{
    public class Stickersheet
    {

        #region Properties

        /// <summary>
        /// Title of the StickerSheet
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Left Margin of the StickerSheet
        /// </summary>
        public float MarginLeft { get; set; }
        /// <summary>
        /// Top Margin of the StickerSheet
        /// </summary>
        public float MarginTop { get; set; }


        /// <summary>
        /// Number of Rows on the Stickersheet
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// Number of Columns on the Stickersheet
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Horizontal Spacing between Stickers
        /// </summary>
        public float HStickerSpacing { get; set; }
        /// <summary>
        /// Vertical Spacing between Stickers
        /// </summary>
        public float VStickerSpacing { get; set; }

        public StickersheetTemplate Template { get; private set; }

        /// <summary>
        /// Stickers on the Stickersheet
        /// </summary>
        public List<Sticker> Stickers { get; set; }

        /// <summary>
        /// Maximal Number of Stickers on the Stickersheet
        /// </summary>
        public int MaxStickers { get { return this.Columns * this.Rows; } }


        private Document Document { get; set; }

        private System.Drawing.PointF StartPoint { get { return new System.Drawing.PointF(MarginLeft, MarginTop); } }

        #endregion

        #region Constructors

        public Stickersheet(StickersheetTemplate template)
        {

            Template = template;

            // Setup Properties from Template
            MarginLeft = template.SheetMarginLeft;
            MarginTop = template.SheetMarginTop;
            Rows = template.Rows;
            Columns = template.Columns;
            HStickerSpacing = template.HStickerSpacing;
            VStickerSpacing = template.VStickerSpacing;

        }

        #endregion

        public void GeneratePDF()
        {

            Document = new Document();

            Document doc = Document;
            Page page = new Page(PaperFormat.A4);

            doc.Pages.Add(page);

            PrintStickersOnPage(page);

        }

        public void FillWithDummyStickers()
        {
            Stickers = new List<Sticker>();

            for (int i = 0; i < MaxStickers; i++)
            {
                Stickers.Add(new Sticker(Template));
            }
        }

        private void PrintStickersOnPage(Page page)
        {

            var nextpoint = StartPoint;

            Pen borderpen = new SolidPen(new ColorRGB(0, 0, 0), (float)0.5);
            SolidBrush fontbrush = new SolidBrush();

            var stickercol = 1;

            foreach (Sticker sticker in Stickers)
            {
                // Draw border
                page.Canvas.DrawRectangle(borderpen, nextpoint.X, nextpoint.Y, sticker.Width, sticker.Height);

                // Draw barcode
                page.Canvas.DrawImage(sticker.BarcodeImage, nextpoint.X + sticker.MarginLeft, nextpoint.Y + sticker.MarginTop, sticker.BarcodeWidth, sticker.BarcodeHeight);

                // Draw Text
                // Line 1
                page.Canvas.DrawString(sticker.Text1, sticker.Font, fontbrush, nextpoint.X + (sticker.Width / 2), nextpoint.Y + sticker.MarginTop);
                // Line 2
                page.Canvas.DrawString(sticker.Text2, sticker.Font, fontbrush, nextpoint.X + (sticker.Width / 2), nextpoint.Y + sticker.MarginTop + sticker.HTextSpacing);
                // Line 3
                page.Canvas.DrawString(sticker.Text3, sticker.Font, fontbrush, nextpoint.X + (sticker.Width / 2), nextpoint.Y + sticker.MarginTop + 3 * sticker.HTextSpacing);

                // Is Sticker last Sticker in the Row?
                if (stickercol == Columns)
                {
                    // Next Sticker will be the first again in the new row
                    stickercol = 1;
                    // Reset X coordinate for next point
                    nextpoint.X = StartPoint.X;

                    // New Y Coordinate for next point
                    nextpoint.Y = nextpoint.Y + VStickerSpacing + sticker.Height;
                }
                else
                {
                    // Next Column
                    stickercol++;
                    // Move X Coordinate to next Point
                    nextpoint.X = nextpoint.X + HStickerSpacing + sticker.Width;
                }


            }

        }

        public void SavePDF(string path)
        {
            Document.Save(path);
        }

        public void PopulateWithZebraItems(Piece piece, List<Part> parts)
        {
            Stickers = new List<Sticker>();

            foreach (Part part in parts)
            {

                Sticker sticker = new Sticker(Template, $"{piece.PieceID}#{part.PartID}", piece.Name, piece.Arranger, part.Name);
                Stickers.Add(sticker);
            }
        
        }


    }

}
