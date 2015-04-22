using Samuxi.WPF.Harjoitus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Print
{
    public class PrintResultPaginator : DocumentPaginator
    {
        private readonly IDocumentPaginatorSource _documentPaginatorSource;
       
        private Size _contentSize;
        
        private readonly IGame _currentGame;

        public Size Margin
        {
            get;
            set;
        }

        public PrintResultPaginator(IDocumentPaginatorSource paginator, IGame currentGame)
        {
            _documentPaginatorSource = paginator;
            
            _currentGame = currentGame;
            PageSize = paginator.DocumentPaginator.PageSize;
            Margin = new Size(25, 50);

            _contentSize = new Size(PageSize.Width - 2 * Margin.Width,
                                 PageSize.Height - 3 * Margin.Height);
        }

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

        public override bool IsPageCountValid
        {
            get { return _documentPaginatorSource.DocumentPaginator.IsPageCountValid; }
        }

        public override int PageCount
        {
            get { return _documentPaginatorSource.DocumentPaginator.PageCount; }
        }

        public override System.Windows.Size PageSize
        {
            get;
            set;
        }

        public override IDocumentPaginatorSource Source
        {
            get { return _documentPaginatorSource; }
        }
    }
}
