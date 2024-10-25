namespace Balea.WebTemplate.Components.Shared;

public class MoveItemsEventArgs<TItem>(IList<TItem> items)
{
    public IList<TItem> Items { get; } = items;
}