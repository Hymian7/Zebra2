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
            RenderPages();
        }

        public ImageSource Thumbnail { get; private set; }
        public ImageSource RenderedPage { get; private set; }

        private void RenderPages()
        { Thumbnail = Render(300, 0).ConvertToImageSource(); }

        public async Task RenderPagesAsync()
        {

            //var PageRenderTask = RenderAsync(1500, 0);
            var ThumbnailRenderTask = RenderAsync(300, 0);
            //RenderedPage = (await PageRenderTask).ConvertToImageSource();
            Thumbnail = (await ThumbnailRenderTask).ConvertToImageSource();

        }

        public async Task<ImageSource> GetFullScaleImageAsync()
        {
            return (await RenderAsync(1500, 0)).ConvertToImageSource();
        }

        public ImageSource GetFullScaleImage()
        {
            return Render(1500, 0).ConvertToImageSource();
        }

        public static async Task<PreviewablePdfPage> CreateAsync(IntPtr document, IntPtr form, int pageNumber)
        {
            var pg = new PreviewablePdfPage(document, form, pageNumber);
            await pg.RenderPagesAsync();
            return pg;
        }
    }
}
