using Model.Interfaces;
namespace Model.Core;

public abstract partial class Article : ICitation
{
    public string Citation { get; private set; }
}