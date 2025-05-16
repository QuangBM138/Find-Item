// ModEntry.cs
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Find_Item
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // Only run when a player is in the world.
            if (!Context.IsWorldReady)
                return;

            // Check if F2 was pressed.
            if (e.Button == SButton.F2)
            {
                // Gather items from the player's inventory and storages.
                List<Item> items = this.GetAllOwnedItems();

                // Wrap these into our ItemSubject type.
                List<ItemSubject> subjects = new List<ItemSubject>();
                foreach (Item item in items)
                {
                    subjects.Add(new ItemSubject(item));
                }

                // Open our custom search menu.
                Game1.activeClickableMenu = new ItemSearchMenu(subjects, this.OnResultSelected);
            }
        }

        private void OnResultSelected(Item item)
        {
            // For demonstration, show a HUD message with the selected item's name.
            Game1.addHUDMessage(new HUDMessage($"Selected item: {item.DisplayName}", 3));
        }

        /// <summary>
        /// Gets all items owned by the player, including those in inventories and storage containers like chests and fridges.
        /// </summary>
        private List<Item> GetAllOwnedItems()
        {
            List<Item> items = new List<Item>();

            // Add items from the player's inventory.
            items.AddRange(Game1.player.Items.Where(item => item != null));

            // Iterate through all locations.
            foreach (GameLocation location in Game1.locations)
            {
                // Check each object in the location.
                foreach (var obj in location.objects.Values)
                {
                    if (obj is Chest chest && chest.playerChest.Value)
                    {
                        // Add items from the chest.
                        items.AddRange(chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID));
                    }
                }

                // Also, if the location is a farmhouse (or island farmhouse), add fridge contents.
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value != null)
                        items.AddRange(house.fridge.Value.GetItemsForPlayer(Game1.player.UniqueMultiplayerID));
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse)
                {
                    if (islandHouse.fridge.Value != null)
                        items.AddRange(islandHouse.fridge.Value.GetItemsForPlayer(Game1.player.UniqueMultiplayerID));
                }
            }

            return items;
        }
    }
}