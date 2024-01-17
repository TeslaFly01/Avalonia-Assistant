using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Avalonia.ReactiveUI;
using Desktop.Assistant.ViewModels;
using Avalonia.Interactivity;
using System;
using System.Reactive.Linq;
using Avalonia.Controls.Notifications;

namespace Desktop.Assistant.Views
{
    public partial class SettingWindow : ReactiveWindow<SettingWindowViewModel>
    {
        private WindowNotificationManager? _manager;
        public SettingWindow()
        {
            ViewModel = new SettingWindowViewModel();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                // 当View被激活时，订阅CloseCommand
                ViewModel?.CancelCommand.Subscribe(_ => Cancel());
                ViewModel?.SaveCommand.Subscribe(_ => Save());
                var topLevel = TopLevel.GetTopLevel(this.Owner);
                _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
            });
            AvaloniaXamlLoader.Load(this);
        }

        public void ClickHandler(object sender, RoutedEventArgs args)
        {
            this.Content = new ChatView();
        }

        private void Save()
        {
            _manager.Show(new Notification("提示", "保存成功", NotificationType.Success));
            Close(true);
        }

        private void Cancel()
        {
            Close(true);
        }
    }
}
