using StardewModdingAPI;

namespace Find_Item.Config
{
    public class ModConfig
    {
        // Key to toggle the mod functionality
        public SButton ToggleKey { get; set; } = SButton.None;

        // Key to open the search menu
        public SButton SearchKey { get; set; } = SButton.F2;

        // Key for quick search in detail menu
        public SButton QuickSearchKey { get; set; } = SButton.D;

        // Key to hide path display
        public SButton HidePathKey { get; set; } = SButton.H;

        // Whether the mod is enabled
        public bool ModEnabled { get; set; } = true;

        // Whether to automatically hide paths after delay
        public bool AutoHidePaths { get; set; } = true;

        // Delay in seconds before hiding paths
        public int AutoHideDelay { get; set; } = 60;
    }
}