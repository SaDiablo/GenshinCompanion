using GenshinCompanion.Modules.BannersModule;
using GenshinCompanion.Services;
using GenshinCompanion.Services.Interfaces;
using GenshinCompanion.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace GenshinCompanion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BannersModule>();
        }
    }
}
