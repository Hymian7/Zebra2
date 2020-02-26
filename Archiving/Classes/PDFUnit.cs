using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Archiving
{
    public static class PDFUnit
    {
        //Returns horizontal Length in Pixels from given Length in mm
        public static float HDistanceFromMM(double mm) { return (float)(mm * 595.28 / 210); }

        //Returns vertical Length in Pixels from given Length in mm
        public static float VDistanceFromMM(double mm) { return (float)(mm * 841.89 / 297); }

    }
}
