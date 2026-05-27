using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Model.Core;

namespace WPF.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isCatalogVisible;
    public MainWindowViewModel()
    {
        Articles = new ArticleRepository().Articles
            .Select(article => new ArticleListItemViewModel(article))
            .ToList();
        StartSearchCommand = new RelayCommand(OpenCatalog);
    }

    public IReadOnlyList<ArticleListItemViewModel> Articles { get; }
    public IRelayCommand StartSearchCommand { get; }
    public bool IsWelcomeVisible => !_isCatalogVisible;
    public bool IsCatalogVisible => _isCatalogVisible;
    private void OpenCatalog()
    {
        _isCatalogVisible = true;
        OnPropertyChanged(nameof(IsWelcomeVisible));
        OnPropertyChanged(nameof(IsCatalogVisible));
    }
}
