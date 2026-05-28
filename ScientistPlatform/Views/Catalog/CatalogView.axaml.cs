using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Model.Core;

namespace ScientistPlatform.Views;

public partial class CatalogView : UserControl, INotifyPropertyChanged
{
    private Publisher? _selectedPublisher;
    private string _submissionIssn = string.Empty;
    private string _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
    private bool _isSuccessMessage = false;

    public CatalogView()
    {
        InitializeComponent();
    }

    public void Initialize(Action<Article> openArticleAction)
    {
        Articles = new ArticleRepository().Articles
            .Select(article => new ArticleListItem(article, openArticleAction))
            .ToList();
        
        Publishers = PublisherRepository.GetAll().ToList();
        SubmitArticleCommand = new RelayCommand(SubmitArticle, () => SelectedPublisher != null && !string.IsNullOrWhiteSpace(SubmissionIssn));
        
        DataContext = this;
        OnPropertyChanged(nameof(Articles));
        OnPropertyChanged(nameof(Publishers));
    }

    public List<ArticleListItem> Articles { get; private set; } = new();
    public List<Publisher> Publishers { get; private set; } = new();
    public IRelayCommand SubmitArticleCommand { get; private set; } = null!;

    public Publisher? SelectedPublisher
    {
        get => _selectedPublisher;
        set
        {
            _selectedPublisher = value;
            _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
            _isSuccessMessage = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            SubmitArticleCommand.NotifyCanExecuteChanged();
        }
    }

    public string SubmissionIssn
    {
        get => _submissionIssn;
        set
        {
            _submissionIssn = value;
            _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
            _isSuccessMessage = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            SubmitArticleCommand.NotifyCanExecuteChanged();
        }
    }

    public string SubmissionMessage => _submissionMessage;
    public string MessageForeground => _isSuccessMessage ? "#34A853" : (_submissionMessage.Contains("не соответствует") || _submissionMessage.Contains("не найдена") ? "#EA4335" : "#70757A");

    private void SubmitArticle()
    {
        if (SelectedPublisher == null || string.IsNullOrWhiteSpace(SubmissionIssn)) return;

        var repository = new ArticleRepository();
        var article = repository.Articles.FirstOrDefault(a => a.ISSN == SubmissionIssn);

        if (article == null)
        {
            _submissionMessage = "Статья с таким ISSN не найдена.";
            _isSuccessMessage = false;
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            return;
        }

        bool matchesTheme = SelectedPublisher.Themes.Any(theme => 
            article.KeyWords.Contains(theme, StringComparer.OrdinalIgnoreCase));

        if (!matchesTheme)
        {
            _submissionMessage = "Тематика статьи не соответствует темам издательства.";
            _isSuccessMessage = false;
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            return;
        }

        _submissionMessage = "Статья успешно отправлена!";
        _isSuccessMessage = true;
        OnPropertyChanged(nameof(SubmissionMessage));
        OnPropertyChanged(nameof(MessageForeground));

        _submissionIssn = string.Empty;
        _selectedPublisher = null;
        OnPropertyChanged(nameof(SubmissionIssn));
        OnPropertyChanged(nameof(SelectedPublisher));
        SubmitArticleCommand.NotifyCanExecuteChanged();
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
