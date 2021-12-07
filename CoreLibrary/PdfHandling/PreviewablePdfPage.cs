using System;
using PdfiumLight;

namespace Zebra.Library.PdfHandling
{
    public class PreviewablePdfPage : PdfPage
    {

        /// <summary>
        /// Initializes a new instance of PdfPage
        /// </summary>
        /// <param name="document">The PDF document</param>
        /// <param name="form"></param>
        /// <param name="pageNumber">Number of this page in the document</param>
        public PreviewablePdfPage(IntPtr document, IntPtr form, int pageNumber) : base(document, form, pageNumber)
        {
            
        }

        private System.Drawing.Image _thumbnail;
        public System.Drawing.Image Thumbnail
        {
            get
            {
                if (_thumbnail == null)
                {
                    _thumbnail = Render(300, 0);
                    return _thumbnail;
                }
                else return _thumbnail;            
            }

                } 
        private System.Drawing.Image _renderedPage;
        public System.Drawing.Image RenderedPage {
            get
            {
                if (_renderedPage == null)
                {
                    _renderedPage = Render(1500, 0);
                    return _renderedPage;
                }
                else return _renderedPage;
            }
        }
    }
}
