using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace System.Windows.Controls
{
    public class ProportionallyStretchingPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
                child.Measure(availableSize);
            return availableSize;
        }

        protected override Size ArrangeOverride(Size availableSize)
        {
            double widthSum = 0.0;
            foreach (UIElement child in Children)
            {
                widthSum += child.DesiredSize.Width;
            }
            double x = 0.0;
            foreach (UIElement child in Children)
            {
                double proportionalWidth = child.DesiredSize.Width / widthSum * availableSize.Width;
                child.Arrange(
                    new Rect(
                        new Point(x, 0.0),
                        new Point(x + proportionalWidth, availableSize.Height)));
                x += proportionalWidth;
            }
            return availableSize;
        }
    }
}