using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace PanoramioMap.Behaviors
{
    public class MoveToMainPageBehavior : DependencyObject, IBehavior
    {
        private UIElement _element;

        public void Attach(DependencyObject associatedObject)
        {
            _element = (UIElement) associatedObject;
            _element.Tapped += ElementOnTapped;
        }

        private void ElementOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            var page = (Page)((Frame)Window.Current.Content).Content;
            page.Frame.GoBack();
        }

        public void Detach()
        {
            _element = null;
        }

        public DependencyObject AssociatedObject => _element;
    }
}
