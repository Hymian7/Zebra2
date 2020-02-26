using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Archiving
{
    public class StickersheetTemplate
    {

        #region Properties
        public int StickersheetTemplateID { get; set; }

        public string Name { get; set; }

        public float SheetMarginLeft { get; set; }

        public float SheetMarginTop { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public float HStickerSpacing { get; set; }

        public float VStickerSpacing { get; set; }

        public float StickerHeight { get; set; }

        public float StickerWidth { get; set; }

        public float StickerMarginLeft { get; set; }

        public float StickerMarginTop { get; set; }

        public float StickerHTextSpacing { get; set; }

        public float BarcodeHeight { get; set; }

        public float BarcodeWidth { get; set; }

        public float FontSize { get; set; }

        public string FontName { get; set; }
        #endregion

        public StickersheetTemplate()
        {

        }


    }
}
