using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Desktop.Assistant.ViewModels;
using ReactiveUI;

namespace Desktop.Assistant.Views
{
    public partial class ChatAIView : ReactiveUserControl<ChatAIViewModel>
    {
        public RoutingState Router { get; }
        public ChatAIView()
        {
            Router = new RoutingState();
            ViewModel = new ChatAIViewModel(Router);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
