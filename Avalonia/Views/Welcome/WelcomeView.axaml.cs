using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ScientistPlatform.Views;

public partial class WelcomeView : UserControl
{
    private Action? _openCatalog;

    public WelcomeView()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public void Initialize(Action openCatalog)
    {
        _openCatalog = openCatalog;
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e)
    {
        _openCatalog?.Invoke();
    }
}
