using System;
using System.Threading.Tasks;
using System.Windows.Media;
using PdfiumLight;

namespace ZebraDesktop
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

        private ImageSource _thumbnail;
        public ImageSource Thumbnail
        {
            get
            {
                if (_thumbnail == null)
                {
                    _thumbnail = Render(300, 0).ConvertToImageSource();
                    return _thumbnail;
                }
                else return _thumbnail;            
            }

                } 
        private ImageSource _renderedPage;
        public ImageSource RenderedPage {
            get
            {
                if (_renderedPage == null)
                {
                    _renderedPage = Render(1500, 0).ConvertToImageSource();
                    return _renderedPage;
                }
                else return _renderedPage;
            }
        }
    }
}
