using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views;

public partial class CatalogView : UserControl
{
    public CatalogView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
