using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace Find_Item.Framework
{
    /// <summary>
    /// Provides reflection-based helpers to detect and extract items stored inside Item Bags (from the ItemBags mod)
    /// without creating a compile-time dependency on ItemBags.dll.
    /// Supports BoundedBag, BundleBag, Rucksack, and nested bags inside OmniBag.
    /// </summary>
    public static class ItemBagHelper
    {
        // Cached reflection properties for performance
        private static PropertyInfo? _contentsProp;
        private static PropertyInfo? _nestedBagsProp;
        private static bool _reflectionInitialized = false;

        private static void EnsureReflectionInitialized(Type bagType)
        {
            if (_reflectionInitialized && _contentsProp != null)
                return;

            try
            {
                _contentsProp ??= bagType.GetProperty("Contents", BindingFlags.Public | BindingFlags.Instance);
                _nestedBagsProp ??= bagType.GetProperty("NestedBags", BindingFlags.Public | BindingFlags.Instance);
                _reflectionInitialized = true;
            }
            catch
            {
                // Ignore reflection errors
            }
        }

        /// <summary>
        /// Checks whether the given item is an ItemBag from the ItemBags mod.
        /// </summary>
        public static bool IsBag(Item? item)
        {
            if (item == null)
                return false;

            Type type = item.GetType();
            string? fullName = type.FullName;

            if (fullName != null && fullName.StartsWith("ItemBags.Bags.", StringComparison.Ordinal))
                return true;

            // Check inheritance hierarchy if wrapped or subclassed
            Type? baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType.FullName != null && baseType.FullName.StartsWith("ItemBags.Bags.", StringComparison.Ordinal))
                    return true;
                baseType = baseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Extracts all items stored inside an ItemBag, including any nested bags within OmniBags.
        /// Returns tuples containing the item and the bag's display path (e.g. "Miner's Bag" or "Omni Bag > Miner's Bag").
        /// </summary>
        public static IEnumerable<(Item Item, string BagPath)> GetItemsInsideBag(Item? bagItem, string parentPath = "")
        {
            if (bagItem == null || !IsBag(bagItem))
                yield break;

            Type bagType = bagItem.GetType();
            EnsureReflectionInitialized(bagType);

            string currentName = bagItem.DisplayName ?? bagItem.Name ?? "Bag";
            string currentPath = string.IsNullOrEmpty(parentPath)
                ? currentName
                : $"{parentPath} > {currentName}";

            // 1. Extract direct Contents (List<StardewValley.Object>)
            PropertyInfo? contentsProp = _contentsProp ?? bagType.GetProperty("Contents", BindingFlags.Public | BindingFlags.Instance);
            if (contentsProp != null)
            {
                object? contentsVal = null;
                try
                {
                    contentsVal = contentsProp.GetValue(bagItem);
                }
                catch { }

                if (contentsVal is IEnumerable contentsList)
                {
                    foreach (object? obj in contentsList)
                    {
                        if (obj is Item item && item != null)
                        {
                            yield return (item, currentPath);
                        }
                    }
                }
            }

            // 2. Extract nested bags if this is an OmniBag (List<ItemBag>)
            PropertyInfo? nestedBagsProp = _nestedBagsProp ?? bagType.GetProperty("NestedBags", BindingFlags.Public | BindingFlags.Instance);
            if (nestedBagsProp != null)
            {
                object? nestedVal = null;
                try
                {
                    nestedVal = nestedBagsProp.GetValue(bagItem);
                }
                catch { }

                if (nestedVal is IEnumerable nestedList)
                {
                    foreach (object? nestedBagObj in nestedList)
                    {
                        if (nestedBagObj is Item nestedBag && nestedBag != null)
                        {
                            foreach (var subItem in GetItemsInsideBag(nestedBag, currentPath))
                            {
                                yield return subItem;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Scans a collection of items (such as inventory or chest slots) for any ItemBags,
        /// and returns all items contained inside those bags.
        /// </summary>
        public static IEnumerable<(Item Item, string BagPath)> GetItemsInsideBags(IEnumerable<Item?>? source)
        {
            if (source == null)
                yield break;

            foreach (Item? item in source)
            {
                if (item != null && IsBag(item))
                {
                    foreach (var bagItem in GetItemsInsideBag(item))
                    {
                        yield return bagItem;
                    }
                }
            }
        }
    }
}
