using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Archiving
{
    public static class StickersheetTemplates
    {
        public static StickersheetTemplate AVERYL4732REV
        { get
            { return new StickersheetTemplate()
                {

                    Name = "Avery L4732REV",
                    SheetMarginTop = PDFUnit.VDistanceFromMM(11),
                    SheetMarginLeft = PDFUnit.HDistanceFromMM(13.5),


                    Rows = 16,
                    Columns = 5,

                    HStickerSpacing = PDFUnit.HDistanceFromMM(2.8),
                    VStickerSpacing = 0,

                    StickerMarginTop = PDFUnit.VDistanceFromMM(2),
                    StickerMarginLeft = PDFUnit.HDistanceFromMM(2),

                    StickerWidth = PDFUnit.HDistanceFromMM(35.6),
                    StickerHeight = PDFUnit.VDistanceFromMM(16.9),

                    StickerHTextSpacing = PDFUnit.VDistanceFromMM(3),

                    BarcodeHeight = PDFUnit.VDistanceFromMM(13),
                    BarcodeWidth = PDFUnit.HDistanceFromMM(13),

                    FontName = "Arial",
                    FontSize = 5
            };
            }
                
        }
    }
}
