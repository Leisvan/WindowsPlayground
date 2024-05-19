using GoogleBooks.Sources;
using GoogleBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GoogleBooks.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is IMainViewModel vm)
            {
                DataContext = vm;
            }
        }

        #region Search box text updating explicitly
        private void SearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateSearchTermSource(sender);
        }
        private void SearchBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (sender is AutoSuggestBox box && e.Key == VirtualKey.Enter)
            {
                UpdateSearchTermSource(box);
            }
        }
        private void UpdateSearchTermSource(AutoSuggestBox sender)
        {
            sender.GetBindingExpression(AutoSuggestBox.TextProperty).UpdateSource();
        }
        #endregion

        private void ItemLoaded(object sender, RoutedEventArgs e)
        {
            var itemRoot = (UIElement)sender;
            var itemRootVisual = ElementCompositionPreview.GetElementVisual(itemRoot);
            var itemCompositor = itemRootVisual.Compositor;
            
            var pointerEnteredAnimation = itemCompositor.CreateVector3KeyFrameAnimation();
            pointerEnteredAnimation.InsertKeyFrame(1.0f, new Vector3(1.04f));

            var pointerExitedAnimation = itemCompositor.CreateVector3KeyFrameAnimation();
            pointerExitedAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f));

            var tappedAnimation= itemCompositor.CreateVector3KeyFrameAnimation();
            tappedAnimation.InsertKeyFrame(0.5f, new Vector3(1.0f));
            tappedAnimation.InsertKeyFrame(1.0f, new Vector3(1.04f));

            itemRoot.PointerEntered += (sender, args) =>
            {
                itemRootVisual.CenterPoint = new Vector3(itemRootVisual.Size / 2, 0);
                itemRootVisual.StartAnimation("Scale", pointerEnteredAnimation);
            };
            itemRoot.PointerExited += (sender, args) => itemRootVisual.StartAnimation("Scale", pointerExitedAnimation);
            itemRoot.Tapped += (sender, args) =>
            {
                itemRootVisual.CenterPoint = new Vector3(itemRootVisual.Size / 2, 0);
                itemRootVisual.StartAnimation("Scale", tappedAnimation);
            };
        }
        private async void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if ((sender is FrameworkElement felement) 
                && felement.DataContext is BookViewModel viewModel)
            {
                await Launcher.LaunchUriAsync(new Uri(viewModel.InfoLink));
            }
        }
    }
}
