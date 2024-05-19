using GoogleBooks.Services;
using GoogleBooks.Sources;
using GoogleBooks.ViewModels;
using GoogleBooks.Views;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace GoogleBooks
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private ILogger _logger;
        public App()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            ServiceCollection services = new ServiceCollection();
            _logger = new SerilogFileLogger();

            _logger.Information("Starting service registration.");
            RegisteringServices(services);
            _serviceProvider = services.BuildServiceProvider();
            _logger.Information("Service registration ended.");
        }

        private void RegisteringServices(ServiceCollection services)
        {
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<IBooksSource, GBooksAPISource>();
            services.AddSingleton<IImageCacheService, ImageCacheService>();
            services.AddSingleton<IHttpPool, FlurlHttpPool>();
            services.AddSingleton(_logger);
            
        }
        private bool Activate(LaunchActivatedEventArgs e)
        {
            _logger.Information("Activating app");
            if (Window.Current.Content is not Frame rootFrame)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    var vm = _serviceProvider.GetService<IMainViewModel>();
                    rootFrame.Navigate(typeof(MainPage), vm);
                }
                Window.Current.Activate();
                _logger.Information("App succesfully activated.");
                return true;
            }
            return false;
        }

        #region App events
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Activate(e))
            {
                UIService.ExtendTitleBar();
                await ThemeSelectorService.SetThemeAsync(ElementTheme.Default);
            }
        }
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            _logger.Information("Failed to load Page" + e.SourcePageType.FullName);
        }
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            
            deferral.Complete();
        } 
        #endregion
    }
}
