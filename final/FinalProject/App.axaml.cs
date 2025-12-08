using Avalonia;
using Avalonia.Markup.Xaml;
using FinalProject.Views;

namespace FinalProject;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var lifetime = ApplicationLifetime;
        if (lifetime != null)
        {
            var desktopLifetime = lifetime as dynamic;
            desktopLifetime.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
