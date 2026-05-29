using System;
using Avalonia.Controls;
using Model.Core;
using ScientistPlatform.Views;

namespace ScientistPlatform;

public partial class MainWindow : Window
{
    private bool _isInitialized;

    public MainWindow()
    {
        InitializeComponent();

        StorageFormatComboBox.ItemsSource = Enum.GetValues<StorageFormat>();
        StorageFormatComboBox.SelectedItem = StorageConfig.CurrentFormat;

        WelcomePage.Initialize(OpenCatalog);
        CatalogPage.Initialize(OpenArticle);

        ShowWelcome();
        _isInitialized = true;
    }

    private void StorageFormatComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_isInitialized || StorageFormatComboBox.SelectedItem is not StorageFormat newFormat)
            return;

        var repository = new ArticleRepository();
        repository.MigrateToFormat(newFormat);
        CatalogPage.Initialize(OpenArticle);
    }

    private void ShowWelcome()
    {
        WelcomePage.IsVisible = true;
        CatalogPage.IsVisible = false;
        ArticlePanel.IsVisible = false;
        ArticleHost.Content = null;
        SettingsButton.IsVisible = false;
    }

    private void OpenCatalog()
    {
        CatalogPage.Initialize(OpenArticle);

        WelcomePage.IsVisible = false;
        CatalogPage.IsVisible = true;
        ArticlePanel.IsVisible = false;
        ArticleHost.Content = null;
        SettingsButton.IsVisible = true;
    }

    private void OpenArticle(Article article)
    {
        ArticleHost.Content = new ArticleView(article, OpenCatalog);

        WelcomePage.IsVisible = false;
        CatalogPage.IsVisible = false;
        ArticlePanel.IsVisible = true;
        SettingsButton.IsVisible = false;
    }
}
