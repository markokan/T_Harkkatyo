using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Samuxi.WPF.Harjoitus.Controls
{
    /// @author Marko Kangas
    /// @version 26.4.2015
    /// 
    /// <summary>
    /// Drag adorner handling.
    /// </summary>
    public class DragAdorner : Adorner
    {
        #region Fields
        /// <summary>
        /// The mouse adjust
        /// </summary>
        private const int MouseAdjust = 10;

        /// <summary>
        /// The adorner layer
        /// </summary>
        private AdornerLayer _adornerLayer;

        /// <summary>
        /// The adornment UIEleement
        /// </summary>
        private UIElement _adornment;

        /// <summary>
        /// The _current mouse position
        /// </summary>
        private Point _currentMousePosition;

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="DragAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
        /// <param name="adornment">The adornment.</param>
        public DragAdorner(UIElement adornedElement, UIElement adornment)
            : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);
            _adornment = adornment;
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        /// <value>
        /// The mouse position.
        /// </value>
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

        /// <summary>
        /// Detatches this instance.
        /// </summary>
        public void Detatch()
        {
            _adornerLayer.Remove(this);
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _adornment.Arrange(new Rect(finalSize));
            return finalSize;
        }

        /// <summary>
        /// Returns a <see cref="T:System.Windows.Media.Transform" /> for the adorner, based on the transform that is currently applied to the adorned element.
        /// </summary>
        /// <param name="transform">The transform that is currently applied to the adorned element.</param>
        /// <returns>
        /// A transform to apply to the adorner.
        /// </returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(MousePosition.X - 4, MousePosition.Y - 4));

            return result;
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return _adornment;
        }

        /// <summary>
        /// Implements any custom measuring behavior for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Size" /> object representing the amount of layout space needed by the adorner.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            _adornment.Measure(constraint);
            return _adornment.DesiredSize;
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
