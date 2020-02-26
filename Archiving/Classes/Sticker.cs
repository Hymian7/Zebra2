extern alias barcode;
using barcode::Bytescout.BarCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Archiving
{
    public class Sticker
    {
        #region Properties

        /// <summary>
        /// Sticker Height
        /// </summary>
        public float Height { get; private set; }
        /// <summary>
        /// Sticker Width
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// Left Margin of the Sticker
        /// </summary>
        public float MarginLeft { get; private set; }
        /// <summary>
        /// Right Margin of the Sticker
        /// </summary>
        public float MarginRight { get; private set; }
        /// <summary>
        /// Bottom Margin of the Sticker
        /// </summary>
        public float MarginBottom { get; private set; }
        /// <summary>
        /// Top Margin of the Sticker
        /// </summary>
        public float MarginTop { get; private set; }

        /// <summary>
        /// Horizontal Spacing between the Text Lines
        /// </summary>
        public float HTextSpacing { get; private set; }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }

        public Bytescout.PDF.Image BarcodeImage { get; private set; }

        public string BarcodeValue { get; set; }

        /// <summary>
        /// Height of the Barcode on the Sticker
        /// </summary>
        public float BarcodeHeight { get; private set; }
        /// <summary>
        /// Width of the Barcode on the Sticker
        /// </summary>
        public float BarcodeWidth { get; private set; }

        public Bytescout.PDF.Font Font { get; private set; }

        public StickersheetTemplate Template { get; private set; }

        #endregion

        #region Constructors

        public Sticker(StickersheetTemplate template, object barcodevalue = null, string text1 = "", string text2="", string text3="")
        {
            Template = template;
            
            // Setup Properties from Template

            MarginTop = template.StickerMarginTop;
            MarginLeft = template.StickerMarginLeft;

            Width = template.StickerWidth;
            Height = template.StickerHeight;

            HTextSpacing = template.HStickerSpacing;

            BarcodeHeight = template.BarcodeHeight;
            BarcodeWidth = template.BarcodeWidth;

            Font = new Bytescout.PDF.Font(template.FontName, template.FontSize);

            BarcodeValue = barcodevalue.ToString() ?? DateTime.Now.ToString();
            Text1 = text1;
            Text2 = text2;
            Text3 = text3;

            GenerateBarcodeImage();

            #endregion

        }

        private void GenerateBarcodeImage()
        {
            Barcode bc = new Barcode
            {
                Symbology = SymbologyType.Aztec,

                DrawCaption = false,
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                Angle = barcode::Bytescout.BarCode.RotationAngle.Degrees0,
                Margins = new Margins(0, 0, 0, 0),
                BarHeight = 3,
                NarrowBarWidth = 3,
                WideToNarrowRatio = 3,
                AddChecksum = false,
                AddChecksumToCaption = false,
                DrawQuietZones = true,
                ResolutionX = 300,
                ResolutionY = 300,

                Value = BarcodeValue

            };

            MemoryStream str = new MemoryStream();
            bc.SaveImage(str);

            BarcodeImage = new Bytescout.PDF.Image(str);

            bc.Dispose();
        }

        /// <summary>
        /// Refresh Barcode Image after e.g. the Barcode Value has been changed.
        /// </summary>
        public void RefreshBarcodeImage() => GenerateBarcodeImage();

    }
}
