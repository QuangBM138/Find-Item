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
using Find_Item.Config;
using Find_Item.Framework;
using StardewValley.Locations;
namespace Find_Item
{
    public class ModEntry : Mod
    {
        public static Dictionary<List<Vector2>, Color> pathColors = new Dictionary<List<Vector2>, Color>();
        public static List<List<Vector2>> paths = new List<List<Vector2>>();
        private Texture2D? tileHighlight;
        public static bool shouldDraw = false;

        // Thay đổi từ private sang public static để có thể truy cập từ ItemDetailMenu
        public static ModConfig? Config { get; private set; }
        private IGenericModConfigMenuApi? ConfigMenu;

        private List<ItemSubject> subjects = new List<ItemSubject>();

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

        public override void Entry(IModHelper helper)
        {
            // Load config
            Config = helper.ReadConfig<ModConfig>();

            // Original initialization code...
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.tileHighlight = helper.ModContent.Load<Texture2D>(Path.Combine("assets", "tileColor.png"));
            helper.Events.Display.RenderedWorld += RenderedWorld;
            helper.Events.Player.Warped += ChangedLocation;
            
            // Register for GameLaunched event to initialize GMCM integration after all mods are loaded
            helper.Events.GameLoop.GameLaunched += (sender, e) =>
            {
                // Add Generic Mod Config Menu integration
                ConfigMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
                if (ConfigMenu != null)
                {
                    var configMenuIntegration = new GenericModConfigMenuIntegration(this.ModManifest, ConfigMenu, Config, this.Helper);
                    configMenuIntegration.Register();
                }
            };
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // Only run when a player is in the world and mod is enabled
            if (!Context.IsWorldReady || !Config.ModEnabled)
                return;

            // Toggle mod if toggle key is pressed
            if (e.Button == Config.ToggleKey && Config.ToggleKey != SButton.None)
            {
                Config.ModEnabled = !Config.ModEnabled;
                this.Helper.WriteConfig(Config);
                Game1.showGlobalMessage($"Find Item mod {(Config.ModEnabled ? "enabled" : "disabled")}");
                return;
            }

            // Check if search key was pressed
            if (e.Button == Config.SearchKey)
            {
                // Original search menu code...
                List<Item> items = this.GetAllOwnedItems();
                List<Item> uniqueItems = items
                    .GroupBy(item => item.DisplayName)
                    .Select(g => g.First())
                    .ToList();

                this.subjects = new List<ItemSubject>();
                foreach (Item item in uniqueItems)
                {
                    this.subjects.Add(new ItemSubject(item));
                }

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

            // Add items from the player's inventory
            items.AddRange(Game1.player.Items.Where(item => item != null));

            // Helper method to check and add items from fridges
            void AddFridgeItems(GameLocation location)
            {
                // Check if location has a fridge (FarmHouse or IslandFarmHouse)
                if (location is StardewValley.Locations.FarmHouse house && house.fridge.Value?.Items != null)
                {
                    items.AddRange(house.fridge.Value.Items.Where(item => item != null));
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse && islandHouse.fridge.Value?.Items != null)
                {
                    items.AddRange(islandHouse.fridge.Value.Items.Where(item => item != null));
                }
            }

            // Get all game locations including buildings
            List<GameLocation> allLocations = new List<GameLocation>();
            
            // Add main locations
            allLocations.AddRange(Game1.locations);
            
            // Add buildings from all farms
            foreach (var location in Game1.locations)
            {
                // Check if location is a farm
                if (location is Farm farm)
                {
                    // Add all buildings in the farm (barns, coops, etc.)
                    foreach (var building in farm.buildings)
                    {
                        if (building.indoors.Value != null)
                        {
                            allLocations.Add(building.indoors.Value);
                        }
                    }
                }
            }

            // Process all locations
            foreach (GameLocation location in allLocations)
            {
                // Check each object in the location
                foreach (var obj in location.objects.Values)
                {
                    if (obj is Chest chest && chest.Items != null)
                    {
                        items.AddRange(chest.Items.Where(item => item != null));
                    }
                }

                // Check location's fridge
                AddFridgeItems(location);
            }

            return items;
        }
    }
}