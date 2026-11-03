// ModEntry.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private static CancellationTokenSource? pathCancelSource;

        public static ModConfig? Config { get; private set; }
        public static IModHelper? ModHelper { get; private set; }
        private IGenericModConfigMenuApi? ConfigMenu;

        private List<ItemSubject> subjects = new List<ItemSubject>();

        /// <summary>Translates a key with optional formatting tokens using SMAPI translation helper.</summary>
        public static string I18n(string key, params object[] tokens)
        {
            if (ModHelper == null)
                return key;

            Translation translation = ModHelper.Translation.Get(key);
            if (!translation.HasValue())
                return key;

            return tokens.Length > 0 ? string.Format(translation.ToString(), tokens) : translation.ToString();
        }

        public static void SchedulePathAutoHide()
        {
            pathCancelSource?.Cancel();
            if (Config == null || !Config.AutoHidePaths || Config.AutoHideDelay <= 0)
                return;

            pathCancelSource = new CancellationTokenSource();
            var token = pathCancelSource.Token;
            Task.Delay(Config.AutoHideDelay * 1000, token).ContinueWith(t =>
            {
                if (!token.IsCancellationRequested)
                {
                    shouldDraw = false;
                    paths.Clear();
                    pathColors.Clear();
                }
            }, token);
        }

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;

            // Load config
            Config = helper.ReadConfig<ModConfig>();

            // Register event handlers
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.tileHighlight = helper.ModContent.Load<Texture2D>(Path.Combine("assets", "tileColor.png"));
            helper.Events.Display.RenderedWorld += this.RenderedWorld;
            helper.Events.Player.Warped += this.ChangedLocation;

            // Register for GameLaunched event to initialize GMCM integration after all mods are loaded
            helper.Events.GameLoop.GameLaunched += (sender, e) =>
            {
                ConfigMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
                if (ConfigMenu != null && Config != null)
                {
                    var configMenuIntegration = new GenericModConfigMenuIntegration(this.ModManifest, ConfigMenu, Config, this.Helper);
                    configMenuIntegration.Register();
                }
            };
        }

        private void RenderedWorld(object? sender, RenderedWorldEventArgs e)
        {
            if (ModEntry.paths.Count > 0 && ModEntry.shouldDraw && this.tileHighlight != null)
            {
                this.DrawPath(e, ModEntry.paths);
            }
        }

        private void DrawPath(RenderedWorldEventArgs e, List<List<Vector2>> paths)
        {
            foreach (List<Vector2> path in paths)
            {
                if (!pathColors.ContainsKey(path))
                    continue;

                Color color = pathColors[path];
                foreach (Vector2 tile in path)
                {
                    Vector2 screenpos = Game1.GlobalToLocal(Game1.viewport, tile * 64f);
                    e.SpriteBatch.Draw(this.tileHighlight, screenpos, color);
                }
            }
        }

        private void ChangedLocation(object? sender, WarpedEventArgs e)
        {
            if (e.IsLocalPlayer)
            {
                pathCancelSource?.Cancel();
                paths.Clear();
                pathColors.Clear();
                shouldDraw = false;
            }
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // Only run when a player is in the world and mod is enabled
            if (!Context.IsWorldReady || Config == null)
                return;

            // Toggle mod if toggle key is pressed
            if (e.Button == Config.ToggleKey && Config.ToggleKey != SButton.None)
            {
                Config.ModEnabled = !Config.ModEnabled;
                this.Helper.WriteConfig(Config);
                string stateText = Config.ModEnabled ? I18n("message.enabled") : I18n("message.disabled");
                Game1.showGlobalMessage(I18n("message.mod-toggled", stateText));
                return;
            }

            if (!Config.ModEnabled)
                return;

            // Check if search key was pressed
            if (e.Button == Config.SearchKey)
            {
                // Suppress hotkey so it doesn't trigger game actions or leak character into the textbox
                this.Helper.Input.Suppress(e.Button);

                List<Item> items = this.GetAllOwnedItems();
                List<Item> uniqueItems = items
                    .Where(item => item != null && !string.IsNullOrEmpty(item.DisplayName))
                    .GroupBy(item => item.DisplayName)
                    .Select(g => g.First())
                    .OrderBy(item => item.DisplayName)
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
        /// Gets all items owned by the player, including those in inventories, storage containers (chests, fridges),
        /// and nested items inside Item Bags (from ItemBags mod).
        /// </summary>
        private List<Item> GetAllOwnedItems()
        {
            List<Item> items = new List<Item>();

            // Helper to collect items directly and unpack items stored inside Item Bags
            void AddItemsFromContainer(IEnumerable<Item?>? container)
            {
                if (container == null)
                    return;

                foreach (var item in container)
                {
                    if (item == null)
                        continue;

                    items.Add(item);

                    if (ItemBagHelper.IsBag(item))
                    {
                        foreach (var bagItem in ItemBagHelper.GetItemsInsideBag(item))
                        {
                            if (bagItem.Item != null)
                            {
                                items.Add(bagItem.Item);
                            }
                        }
                    }
                }
            }

            // Add items from the player's inventory
            AddItemsFromContainer(Game1.player.Items);

            // Helper method to check and add items from fridges
            void AddFridgeItems(GameLocation location)
            {
                if (location is StardewValley.Locations.FarmHouse house && house.fridge.Value?.Items != null)
                {
                    AddItemsFromContainer(house.fridge.Value.Items);
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse && islandHouse.fridge.Value?.Items != null)
                {
                    AddItemsFromContainer(islandHouse.fridge.Value.Items);
                }
            }

            // Get all game locations including buildings
            List<GameLocation> allLocations = new List<GameLocation>();
            allLocations.AddRange(Game1.locations);

            // Add buildings from all farms
            foreach (var location in Game1.locations)
            {
                if (location is Farm farm)
                {
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
                        AddItemsFromContainer(chest.Items);
                    }
                    // Support Auto-Grabbers (which store items in heldObject)
                    else if (obj.heldObject.Value is Chest grabberChest && grabberChest.Items != null)
                    {
                        AddItemsFromContainer(grabberChest.Items);
                    }
                }

                // Check location's fridge
                AddFridgeItems(location);
            }

            return items;
        }
    }
}
