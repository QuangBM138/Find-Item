// ModEntry.cs
using Microsoft.Xna.Framework; // Thêm để sử dụng Vector2 và Color
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Find_Item
{
    public class ModEntry : Mod
    {

        public static Dictionary<List<Vector2>, Color> pathColors = new Dictionary<List<Vector2>, Color>();
        public static List<List<Vector2>> paths = new List<List<Vector2>>();
        private Texture2D? tileHighlight;
        public static bool shouldDraw = false;

        // Thêm method để vẽ paths trong RenderedWorld event:
        private void RenderedWorld(object? sender, RenderedWorldEventArgs e)
        {
            if (ModEntry.paths.Count > 0 && ModEntry.shouldDraw)
            {
                this.DrawPath(e, ModEntry.paths);
            }
        }

        private void DrawPath(RenderedWorldEventArgs e, List<List<Vector2>> paths)
        {
            foreach (List<Vector2> path in paths)
            {
                foreach (Vector2 tile in path)
                {
                    Vector2 screenpos = Game1.GlobalToLocal(Game1.viewport, tile * 64f);
                    e.SpriteBatch.Draw(this.tileHighlight, screenpos, pathColors[path]);
                }
            }
        }

        // Thêm event handler khi người chơi di chuyển sang location khác
        private void ChangedLocation(object? sender, WarpedEventArgs e)
        {
            if (e.IsLocalPlayer)
            {
                paths.Clear();
                shouldDraw = false;
            }
        }

        private List<ItemSubject> subjects = new List<ItemSubject>();

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            // Replace the problematic line with Path.Combine for cross-platform compatibility
            this.tileHighlight = helper.ModContent.Load<Texture2D>(Path.Combine("assets", "tileColor.png"));
            helper.Events.Display.RenderedWorld += RenderedWorld;
            helper.Events.Player.Warped += ChangedLocation;
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

                // Remove duplicate items based on DisplayName.
                List<Item> uniqueItems = items
                    .GroupBy(item => item.DisplayName)
                    .Select(g => g.First())
                    .ToList();

                // Wrap these into our ItemSubject type.
                this.subjects = new List<ItemSubject>();
                foreach (Item item in uniqueItems)
                {
                    this.subjects.Add(new ItemSubject(item));
                }

                // Open our custom search menu.
                Game1.activeClickableMenu = new ItemSearchMenu(this.subjects, this.OnResultSelected);
            }
        }

        private void OnResultSelected(Item item)
        {
            // Open the detail menu with the selected item and pass subjects for returning to search
            Game1.activeClickableMenu = new ItemDetailMenu(item, this.subjects, this.OnResultSelected);
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
                    if (obj is Chest chest)
                    {
                        // Lấy items từ tất cả các rương, không chỉ của người chơi hiện tại
                        if (chest.Items != null)  // Sử dụng Items thay vì items
                        {
                            items.AddRange(chest.Items.Where(item => item != null));
                        }
                    }
                }

                // Also, if the location is a farmhouse (or island farmhouse), add fridge contents.
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value != null && house.fridge.Value.Items != null)  // Sử dụng Items
                    {
                        items.AddRange(house.fridge.Value.Items.Where(item => item != null));
                    }
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse)
                {
                    if (islandHouse.fridge.Value != null && islandHouse.fridge.Value.Items != null)  // Sử dụng Items
                    {
                        items.AddRange(islandHouse.fridge.Value.Items.Where(item => item != null));
                    }
                }
            }

            return items;
        }
    }
}