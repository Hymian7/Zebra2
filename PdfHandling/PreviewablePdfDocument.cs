using System.Collections.Generic;
using System.Threading.Tasks;
using PdfiumLight;

namespace Zebra.PdfHandling
{
    public class PreviewablePdfDocument : PdfDocument
    {
        public IList<PreviewablePdfPage> Pages { get; private set; }

        public PreviewablePdfDocument(string path, string password = null) : base(path, password)
        {
            Initialize();
        }

        public PreviewablePdfDocument(System.IO.Stream stream, string password = null) : base(stream, password)
        {
            Initialize();
        }

        private void Initialize()
        {
            Pages = new List<PreviewablePdfPage>();

            //Add pages to Document
            for (int i = 0; i < PageCount(); i++)
            {
                Pages.Add(GetPreviewablePage(i));
            }
        }

        private Task InitializeAsync()
        {
            return Task.Run(() => {
                Pages = new List<PreviewablePdfPage>();

                //Add pages to Document
                for (int i = 0; i < PageCount(); i++)
                {
                    Pages.Add(GetPreviewablePage(i));
                }
            });
        }

        /// <summary>
        /// This method will load and return the sepecific page
        /// </summary>
        /// <param name="page">The null-based index of the page</param>
        /// <returns>A new PdfPage</returns>
        private PreviewablePdfPage GetPreviewablePage(int page) => new PreviewablePdfPage(_document, _form, page);



       
    }
}
