using Desktop.Assistant.Services;
using Desktop.Assistant.Domain.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reactive;

namespace Desktop.Assistant.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //public ICommand GetStartedCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> OpenSettingWindowCommand { get; }
        public MainViewModel(RoutingState router) : base(router) 
        {
            //this.GetStartedCommand = ReactiveCommand.Create(NavigateToLogin);
            OpenSettingWindowCommand = ReactiveCommand.Create(() =>
            {
                MessageBus.Current.SendMessage(new object());
            });
        }
        //public void NavigateToLogin()
        //{
        //    Router.Navigate.Execute(new WelcomeViewModel(Router));
        //}
    }
}
