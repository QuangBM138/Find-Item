using StardewValley;
using System;

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

        /// <summary>Checks whether this item matches the given search query.</summary>
        public bool Matches(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return true;

            query = query.Trim().ToLowerInvariant();

            // Match localized display name
            if (this.Name.ToLowerInvariant().Contains(query))
                return true;

            // Match internal English name
            if (this.Item?.Name != null && this.Item.Name.ToLowerInvariant().Contains(query))
                return true;

            // Match item ID
            if (this.Item?.ItemId != null && this.Item.ItemId.ToLowerInvariant().Contains(query))
                return true;

            // Match category name
            try
            {
                string? categoryName = this.Item?.getCategoryName();
                if (!string.IsNullOrEmpty(categoryName) && categoryName.ToLowerInvariant().Contains(query))
                    return true;
            }
            catch
            {
                // Ignore any category resolution errors
            }

            return false;
        }
    }
}
