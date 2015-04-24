using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Controls
{
    public class DragAdorner : Adorner
    {
        private const int MouseAdjust = 10;

        private AdornerLayer _adornerLayer;
        private UIElement _adornment;
        private Point _currentMousePosition;

        public DragAdorner(UIElement adornedElement, UIElement adornment)
            : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);
            _adornment = adornment;
            IsHitTestVisible = false;
        }

        public Point MousePosition
        {
            get { return _currentMousePosition; }
            set
            {
                if (_currentMousePosition != value)
                {
                    _currentMousePosition = new Point(value.X + MouseAdjust, value.Y);
                    _adornerLayer.Update(AdornedElement);
                }
            }
        }

        public void Detatch()
        {
            _adornerLayer.Remove(this);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _adornment.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(MousePosition.X - 4, MousePosition.Y - 4));

            return result;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _adornment;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _adornment.Measure(constraint);
            return _adornment.DesiredSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
