using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views;

public partial class WelcomeView : UserControl
{
    public WelcomeView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
