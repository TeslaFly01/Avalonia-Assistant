using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Metadata;
using Desktop.Assistant.Domain.Model;
using Desktop.Assistant.Domain.Models;
using Desktop.Assistant.Services;
using Desktop.Assistant.Views;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Desktop.Assistant.ViewModels
{
    public class SettingWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public string FileJson => "openai.json";
        [Reactive]
        public string Endpoint { get; set; }

        [Reactive]
        public string Key { get; set; }

        [Reactive]
        public string Model { get; set; }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public ViewModelActivator Activator { get; }

        public SettingWindowViewModel()
        {
            Activator = new ViewModelActivator();
            if (File.Exists(FileJson))
            {
                var openaiString = File.ReadAllText(FileJson);
                if (string.IsNullOrEmpty(openaiString))
                {
                    return;
                }
                var openai = JsonConvert.DeserializeObject<OpenAIModel>(openaiString);
                Endpoint = openai.EndPoint;
                Key = openai.Key;
                Model = openai.Model;
            }
            //监听三个参数都不为空的时候才能执行保存命令
            var isobs = Observable.CombineLatest(
            this.WhenAnyValue(s => s.Endpoint, t => !string.IsNullOrEmpty(t)),
            this.WhenAnyValue(s => s.Key, t => !string.IsNullOrEmpty(t)),
            this.WhenAnyValue(s => s.Model, t => !string.IsNullOrEmpty(t)),
            (endpointNotEmpty, keyNotEmpty, modelNotEmpty) => endpointNotEmpty && keyNotEmpty && modelNotEmpty);

            SaveCommand = ReactiveCommand.Create(SaveInfoCommand, isobs);
            CancelCommand = ReactiveCommand.Create(CancelInfoCommand);
        }

        public void SaveInfoCommand()
        {
            try
            {
                var openAIModel = new OpenAIModel();
                openAIModel.EndPoint = Endpoint;
                openAIModel.Key = Key;
                openAIModel.Model = Model;
                var lines = JsonConvert.SerializeObject(openAIModel);
                File.WriteAllText(FileJson, lines);
                //存储变量后面使用
                OpenAIOption.EndPoint = Endpoint;
                OpenAIOption.Key = Key;
                OpenAIOption.Model = Model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CancelInfoCommand() { }
    }
}
