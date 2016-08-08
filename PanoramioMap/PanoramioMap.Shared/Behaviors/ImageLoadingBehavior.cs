using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Xaml.Interactivity;

namespace PanoramioMap.Behaviors
{
    public class ImageLoadingBehavior : DependencyObject, IBehavior
    {
        private BitmapImage _bitmapImage;

        public void Attach(DependencyObject associatedObject)
        {
            _bitmapImage = (BitmapImage) associatedObject;
            _bitmapImage.ImageOpened += ImageOpened;
            _bitmapImage.ImageFailed += ImageFailed;
        }

        private void ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadingProgressRing.Visibility = Visibility.Collapsed;
        }

        private void ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            LoadingProgressRing.Visibility = Visibility.Collapsed;
            FailedTextBlock.Visibility = Visibility.Visible;
        }

        public void Detach()
        {
            _bitmapImage.ImageOpened -= ImageOpened;
            _bitmapImage.ImageFailed -= ImageFailed;
            _bitmapImage = null;
        }

        public DependencyObject AssociatedObject => _bitmapImage;

        public static readonly DependencyProperty LoadingProgressRingProperty = DependencyProperty.Register(
            "LoadingProgressRing", typeof (ProgressRing), typeof (ImageLoadingBehavior), new PropertyMetadata(default(ProgressRing)));

        public ProgressRing LoadingProgressRing
        {
            get { return (ProgressRing) GetValue(LoadingProgressRingProperty); }
            set { SetValue(LoadingProgressRingProperty, value); }
        }

        public static readonly DependencyProperty FailedTextBlockProperty = DependencyProperty.Register(
            "FailedTextBlock", typeof (TextBlock), typeof (ImageLoadingBehavior), new PropertyMetadata(default(TextBlock)));

        public TextBlock FailedTextBlock
        {
            get { return (TextBlock) GetValue(FailedTextBlockProperty); }
            set { SetValue(FailedTextBlockProperty, value); }
        }
    }
}
