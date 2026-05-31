using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Model.Core;
using System.Linq;

namespace ScientistPlatform.Views;

public partial class CitationView : UserControl
{
    private Action? _goBack;

    public CitationView()
    {
        InitializeComponent();
    }

    public CitationView(Action goBack) : this()
    {
        _goBack = goBack;
        
        var articleRepo = new ArticleRepository();
        ArticlesComboBox.ItemsSource = articleRepo.Articles.Where(a => a.Publisher != null).ToList();
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        _goBack?.Invoke();
    }

    private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GenerateButton.IsEnabled = ArticlesComboBox.SelectedItem != null;
        MessageText.Text = string.Empty;
    }

    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ArticlesComboBox.SelectedItem is Article article && article.Publisher != null)
        {
            try
            {
                string citation = CitationService.CreateCitation(article, article.Publisher);
                ResultTextBox.Text = citation;
                SaveButton.IsEnabled = !string.IsNullOrWhiteSpace(citation);
                MessageText.Text = string.Empty;
            }
            catch (System.Exception ex)
            {
                ResultTextBox.Text = $"Ошибка: {ex.Message}";
                SaveButton.IsEnabled = false;
            }
        }
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ArticlesComboBox.SelectedItem is Article article && !string.IsNullOrWhiteSpace(ResultTextBox.Text))
        {
            try
            {
                string fileName = $"Citation_{article.ISSN}.txt";
                string path = CitationService.SaveCitationToTxt(ResultTextBox.Text, fileName);
            }
            catch (System.Exception ex)
            {
                MessageText.Text = $"Ошибка: {ex.Message}";
                MessageText.Foreground = Avalonia.Media.Brush.Parse("#EA4335");
            }
        }
    }
}
