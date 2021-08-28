using Prism.Mvvm;

namespace GenshinCompanion.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Genshin Companion";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel()
        {
        }
    }
}