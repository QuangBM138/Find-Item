// ItemSubject.cs
using StardewValley;

namespace Find_Item
{
    /// <summary>Wraps an item to be used as a search subject.</summary>
    public class ItemSubject
    {
        /// <summary>The underlying item.</summary>
        public Item Item { get; }

        /// <summary>The name used for filtering and display.</summary>
        public string Name => Item?.DisplayName ?? "Unknown";

        public ItemSubject(Item item)
        {
            this.Item = item;
        }
    }
}