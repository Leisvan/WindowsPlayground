using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.Foundation;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace CanvasDrag.Controls
{
    public enum SurfaceBound
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public class DragCanvas : Canvas
    {
        public static readonly DependencyProperty AssociatedElementProperty =
            DependencyProperty.Register(
                nameof(AssociatedElement),
                typeof(UIElement),
                typeof(DragCanvas),
                new PropertyMetadata(null, AssociateElementCallback));

        public static readonly DependencyProperty ElementBoundProperty =
            DependencyProperty.Register(
                nameof(ElementBound),
                typeof(SurfaceBound),
                typeof(DragCanvas),
                new PropertyMetadata(SurfaceBound.TopRight));

        public static readonly DependencyProperty IsMovingProperty =
            DependencyProperty.Register(
                nameof(IsMoving),
                typeof(bool),
                typeof(DragCanvas),
                new PropertyMetadata(false));

        public static readonly DependencyProperty PlacementBoundProperty =
                    DependencyProperty.Register(
                nameof(PlacementBound),
                typeof(double), typeof(DragCanvas),
                new PropertyMetadata(50d));

        public static readonly DependencyProperty PointerBoundProperty =
            DependencyProperty.Register(
                nameof(PointerBound),
                typeof(double),
                typeof(DragCanvas),
                new PropertyMetadata(0d));

        public static readonly DependencyProperty TranslateAnimationDurationProperty =
                    DependencyProperty.Register(
                        nameof(TranslateAnimationDuration),
                        typeof(TimeSpan),
                        typeof(DragCanvas),
                        new PropertyMetadata(TimeSpan.FromSeconds(.6)));

        private Dictionary<SurfaceBound, Point> _boundsMap = new Dictionary<SurfaceBound, Point>();

        private bool _isDragging = false;

        private double _originalX;

        private double _originalY;

        private double _pointerOffsetX;

        private double _pointerOffsetY;

        public DragCanvas()
        {
            SizeChanged += DragCanvas_SizeChanged;
        }

        /// <summary>
        /// The item to drag inside the canvas.
        /// </summary>
        public UIElement AssociatedElement
        {
            get { return (UIElement)GetValue(AssociatedElementProperty); }
            set { SetValue(AssociatedElementProperty, value); }
        }

        public SurfaceBound ElementBound
        {
            get { return (SurfaceBound)GetValue(ElementBoundProperty); }
            set { SetValue(ElementBoundProperty, value); }
        }

        public bool IsMoving
        {
            get { return (bool)GetValue(IsMovingProperty); }
            set { SetValue(IsMovingProperty, value); }
        }

        /// <summary>
        /// Represents the border margins used to determinate the four
        /// corner positions where the associated element will be placed.
        /// </summary>
        public double PlacementBound
        {
            get { return (double)GetValue(PlacementBoundProperty); }
            set { SetValue(PlacementBoundProperty, value); }
        }

        /// <summary>
        /// The margin to determinate the limits the user can drag the item to before
        /// it's considered out of bounds and finds the appropriate corner.
        /// </summary>
        public double PointerBound
        {
            get { return (double)GetValue(PointerBoundProperty); }
            set { SetValue(PointerBoundProperty, value); }
        }

        /// <summary>
        /// The duration of the animation where the associated
        /// element finds the appropriate corner.
        /// </summary>
        public TimeSpan TranslateAnimationDuration
        {
            get { return (TimeSpan)GetValue(TranslateAnimationDurationProperty); }
            set { SetValue(TranslateAnimationDurationProperty, value); }
        }

        private static void AssociateElementCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DragCanvas canvas && e.NewValue is UIElement element)
            {
                element.AddHandler(PointerPressedEvent, new PointerEventHandler(canvas.DraggingPointerPressed), true);
                element.AddHandler(PointerReleasedEvent, new PointerEventHandler(canvas.DraggingPointerReleased), true);
                element.AddHandler(PointerMovedEvent, new PointerEventHandler(canvas.DraggingPointerMoved), true);
            }
        }

        private static double GetDistanceLength(Point p1, Point p2)
            => Math.Abs(Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2)));

        private void ClearImplicitAnimations() =>
            Implicit.SetAnimations(AssociatedElement, null);

        private void DragCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AssociatedElement == null)
            {
                return;
            }

            _boundsMap.Clear();
            var rightBound = RenderSize.Width - PlacementBound - AssociatedElement.RenderSize.Width;
            var bottomBound = RenderSize.Height - PlacementBound - AssociatedElement.RenderSize.Height;
            _boundsMap.Add(SurfaceBound.TopLeft, new Point(PlacementBound, PlacementBound));
            _boundsMap.Add(SurfaceBound.TopRight, new Point(rightBound, PlacementBound));
            _boundsMap.Add(SurfaceBound.BottomLeft, new Point(PlacementBound, bottomBound));
            _boundsMap.Add(SurfaceBound.BottomRight, new Point(rightBound, bottomBound));
            MoveToBounds(ElementBound);
        }

        private void DraggingPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_isDragging)
            {
                var pointerPosition = e.GetCurrentPoint(this).Position;
                var x = pointerPosition.X - _pointerOffsetX;
                var y = pointerPosition.Y - _pointerOffsetY;
                var elementX = GetLeft(AssociatedElement);
                var elementY = GetTop(AssociatedElement);

                //Don't mark the movement unless it's perceptible
                if ((int)x != (int)_originalX && (int)y != (int)_originalY)
                {
                    IsMoving = true;
                }

                //left bound
                if (elementX < PointerBound)
                {
                    _isDragging = false;
                    MoveToBounds();
                    return;
                }

                //right bound
                var rightBound = RenderSize.Width - PointerBound - AssociatedElement.RenderSize.Width;
                if (elementX > rightBound)
                {
                    _isDragging = false;
                    MoveToBounds();
                    return;
                }

                //top bound
                if (elementY < PointerBound)
                {
                    _isDragging = false;
                    MoveToBounds();
                    return;
                }

                //bottom bound
                var bottomBound = RenderSize.Height - PointerBound - AssociatedElement.RenderSize.Height;
                if (elementY > bottomBound)
                {
                    _isDragging = false;
                    MoveToBounds();
                    return;
                }

                SetLeft(sender as Button, x);
                SetTop(sender as Button, y);
            }
        }

        private void DraggingPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var button = sender as Button;
            _isDragging = true;
            var pointerPosition = e.GetCurrentPoint(this).Position;
            _originalX = GetLeft(button);
            _originalY = GetTop(button);
            _pointerOffsetX = pointerPosition.X - _originalX;
            _pointerOffsetY = pointerPosition.Y - _originalY;
            ClearImplicitAnimations();
            button.CapturePointer(e.Pointer);
        }

        private void DraggingPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isDragging = false;
            AssociatedElement.ReleasePointerCapture(e.Pointer);
            MoveToBounds();
        }

        private async void MoveToBounds(SurfaceBound? bounds = null)
        {
            if (AssociatedElement == null)
            {
                return;
            }
            if (bounds == null)
            {
                if (_boundsMap?.Count == 0)
                {
                    return;
                }
                var buttonPoint = new Point(GetLeft(AssociatedElement), GetTop(AssociatedElement));
                bounds = _boundsMap
                    .Select(kvp => kvp)
                    .OrderBy(kvp => GetDistanceLength(kvp.Value, buttonPoint))
                    .FirstOrDefault().Key;
                ElementBound = bounds.Value;
            }

            var coordinates = _boundsMap[bounds.Value];
            if (GetLeft(AssociatedElement) != coordinates.X || GetTop(AssociatedElement) != coordinates.Y)
            {
                SetImplicitOffsetAnimation();

                SetLeft(AssociatedElement, coordinates.X);
                SetTop(AssociatedElement, coordinates.Y);

                await Task.Delay(TranslateAnimationDuration);
            }
            IsMoving = false;
        }

        private void SetImplicitOffsetAnimation()
        {
            ClearImplicitAnimations();
            var offsetTranslationAnimation = new ImplicitAnimationSet
            {
                new OffsetAnimation { Duration = TranslateAnimationDuration }
            };
            Implicit.SetAnimations(AssociatedElement, offsetTranslationAnimation);
        }
    }
}