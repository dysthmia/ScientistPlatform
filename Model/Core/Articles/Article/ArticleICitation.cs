using Model.Interfaces;
namespace Model.Core;

public partial class Article : ICitation
{
    public string Citation { get; private set; }
}