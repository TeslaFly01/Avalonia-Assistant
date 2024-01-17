using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using Desktop.Assistant.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Desktop.Assistant.Views
{
    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            ViewModel = new MainViewModel(new RoutingState());
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            //this.WhenActivated(disposables => { 
            //    this.BindCommand(ViewModel, vm => vm.GetStartedCommand, v => v.GetStartedButton).DisposeWith(disposables);
            //});
            AvaloniaXamlLoader.Load(this);
        }

        public void ClickHandler(object sender, RoutedEventArgs args)
        {
            this.Content = new WelcomeView();
        }

        private void ToggleSwitch_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var app = Application.Current;
            if (app is not null)
            {
                var theme = app.ActualThemeVariant;
                app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
            }
        }
    }
}
