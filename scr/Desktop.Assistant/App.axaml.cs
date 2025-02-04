using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Desktop.Assistant.Services;
using Desktop.Assistant.ViewModels;
using Desktop.Assistant.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using Whisper.net;

namespace Desktop.Assistant
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            switch (ApplicationLifetime)
            {
                case IClassicDesktopStyleApplicationLifetime desktop:
                    //desktop.MainWindow = new MainWindow();
                    var suspension = new AutoSuspendHelper(ApplicationLifetime);
                    RxApp.SuspensionHost.CreateNewAppState = () => new MainWindowViewModel();
                    RxApp.SuspensionHost.SetupDefaultSuspendResume(new NewtonsoftJsonSuspensionDriver("appstate.json"));
                    suspension.OnFrameworkInitializationCompleted();
                    // Load the saved view model state.
                    Register();
                    new MainWindow { DataContext = Locator.Current.GetService<IScreen>() }.Show();
                    break;
                case ISingleViewApplicationLifetime singleView:
                    Register();
                    singleView.MainView = new MainView();
                    break;
            }        
            // Create the AutoSuspendHelper.         
            base.OnFrameworkInitializationCompleted();
        }

        private static void Register()
        {
            Locator.CurrentMutable.RegisterConstant<IScreen>(RxApp.SuspensionHost.GetAppState<MainWindowViewModel>());
            Locator.CurrentMutable.Register<IViewFor<MainViewModel>>(() => new MainView());
            Locator.CurrentMutable.Register<IViewFor<ChatViewModel>>(() => new ChatView());
            Locator.CurrentMutable.Register<IViewFor<WelcomeViewModel>>(() => new WelcomeView());
            Locator.CurrentMutable.Register<WhisperFactory>(() => WhisperFactory.FromPath("ggml-base-q5_1.bin"));
            Locator.CurrentMutable.Register(() =>
            {
                // 从构建的服务中获取WhisperFactory
                var whisperFactory = Locator.Current.GetService<WhisperFactory>();
                return whisperFactory.CreateBuilder()
                    .WithLanguage("auto") // 自动识别语言
                    .Build();
            });
        }
    }
}
