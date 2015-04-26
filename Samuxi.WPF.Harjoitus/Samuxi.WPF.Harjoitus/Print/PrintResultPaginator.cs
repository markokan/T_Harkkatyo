using Samuxi.WPF.Harjoitus.Model;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Print
{
    /// @version 26.4.2015
    /// @author Marko Kangas
    /// 
    /// <summary>
    ///  Game result print paginator.
    /// </summary>
    public class PrintResultPaginator : DocumentPaginator
    {
        #region Fields

        /// <summary>
        /// The document paginator source
        /// </summary>
        private readonly IDocumentPaginatorSource _documentPaginatorSource;

        /// <summary>
        /// The content size
        /// </summary>
        private Size _contentSize;

        /// <summary>
        /// The current game
        /// </summary>
        private readonly IGame _currentGame;

        #endregion

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Size Margin
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintResultPaginator"/> class.
        /// </summary>
        /// <param name="paginator">The paginator.</param>
        /// <param name="currentGame">The current game.</param>
        public PrintResultPaginator(IDocumentPaginatorSource paginator, IGame currentGame)
        {
            _documentPaginatorSource = paginator;
            
            _currentGame = currentGame;
            PageSize = paginator.DocumentPaginator.PageSize;
            Margin = new Size(25, 50);

            _contentSize = new Size(PageSize.Width - 2 * Margin.Width,
                                 PageSize.Height - 3 * Margin.Height);
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Windows.Documents.DocumentPage" /> for the specified page number.
        /// </summary>
        /// <param name="pageNumber">The zero-based page number of the document page that is needed.</param>
        /// <returns>
        /// The <see cref="T:System.Windows.Documents.DocumentPage" /> for the specified <paramref name="pageNumber" />, or <see cref="F:System.Windows.Documents.DocumentPage.Missing" /> if the page does not exist.
        /// </returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = _documentPaginatorSource.DocumentPaginator.GetPage(pageNumber);
            ContainerVisual constructPage = new ContainerVisual
            {
                Transform = new TranslateTransform(Margin.Width, Margin.Height)
            };

            //add header element
            DrawingVisual headerVisual = new DrawingVisual();
            using (DrawingContext ctx = headerVisual.RenderOpen())
            {
                FontFamily font = new FontFamily("Arial");
                Typeface typeface = new Typeface(font, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                FormattedText text = new FormattedText("Tulokset", System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, typeface, 12, Brushes.Black);
                ctx.DrawText(text, new Point(PageSize.Width - Margin.Width - 100, -Margin.Height / 2));
            }


            //add page
            ContainerVisual scaledPageVisual = new ContainerVisual
            {
                Transform = new ScaleTransform((_contentSize.Width / PageSize.Width), (_contentSize.Height / PageSize.Height))
            };
            scaledPageVisual.Children.Add(page.Visual);


            constructPage.Children.Add(scaledPageVisual);
            constructPage.Children.Add(headerVisual);

            return new DocumentPage(constructPage);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether <see cref="P:System.Windows.Documents.DocumentPaginator.PageCount" /> is the total number of pages.
        /// </summary>
        public override bool IsPageCountValid
        {
            get { return _documentPaginatorSource.DocumentPaginator.IsPageCountValid; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a count of the number of pages currently formatted
        /// </summary>
        public override int PageCount
        {
            get { return _documentPaginatorSource.DocumentPaginator.PageCount; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the suggested width and height of each page.
        /// </summary>
        public override System.Windows.Size PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// When overridden in a derived class, returns the element being paginated.
        /// </summary>
        public override IDocumentPaginatorSource Source
        {
            get { return _documentPaginatorSource; }
        }
    }
}
