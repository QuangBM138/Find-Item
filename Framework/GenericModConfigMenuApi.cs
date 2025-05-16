using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using Find_Item.Config;

namespace Find_Item.Framework
{
    /// <summary>The API which lets other mods add a config UI through Generic Mod Config Menu.</summary>
    public interface IGenericModConfigMenuApi
    {
        void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);
        void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
        void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
        void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, string fieldId = null);
    }

    public class GenericModConfigMenuIntegration
    {
        private readonly IManifest _modManifest;
        private readonly IGenericModConfigMenuApi _configMenu;
        private ModConfig _config;
        private readonly IModHelper _helper;

        public GenericModConfigMenuIntegration(IManifest modManifest, IGenericModConfigMenuApi configMenu, ModConfig config, IModHelper helper)
        {
            _modManifest = modManifest ?? throw new ArgumentNullException(nameof(modManifest));
            _configMenu = configMenu ?? throw new ArgumentNullException(nameof(configMenu));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public void Register()
        {
            // Register mod configuration
            _configMenu.Register(
                mod: _modManifest,
                reset: () => {
                    _config = new ModConfig();
                    _helper.WriteConfig(_config);
                },
                save: () => _helper.WriteConfig(_config)
            );

            // Add mod enable/disable option
            _configMenu.AddBoolOption(
                mod: _modManifest,
                getValue: () => _config.ModEnabled,
                setValue: value => _config.ModEnabled = value,
                name: () => "Mod Enabled",
                tooltip: () => "Enable or disable the Find Item mod functionality."
            );

            // Add toggle key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.ToggleKey,
                setValue: value => _config.ToggleKey = value,
                name: () => "Toggle Key",
                tooltip: () => "The key that toggles the mod on/off. Default: None"
            );

            // Add search menu key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.SearchKey,
                setValue: value => _config.SearchKey = value,
                name: () => "Search Menu Key",
                tooltip: () => "The key that opens the search menu. Default: F2"
            );

            // Add quick search key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.QuickSearchKey,
                setValue: value => _config.QuickSearchKey = value,
                name: () => "Quick Search Key",
                tooltip: () => "The key for quick search in the detail menu. Default: D"
            );

            // Add hide path key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.HidePathKey,
                setValue: value => _config.HidePathKey = value,
                name: () => "Hide Path Key",
                tooltip: () => "The key that hides the path display. Default: H"
            );

            // Add auto-hide paths option
            _configMenu.AddBoolOption(
                mod: _modManifest,
                getValue: () => _config.AutoHidePaths,
                setValue: value => _config.AutoHidePaths = value,
                name: () => "Auto-hide Paths",
                tooltip: () => "Automatically hide paths after delay"
            );

            // Add auto-hide delay option
            _configMenu.AddNumberOption(
                mod: _modManifest,
                getValue: () => _config.AutoHideDelay,
                setValue: value => _config.AutoHideDelay = value,
                name: () => "Auto-hide Delay",
                tooltip: () => "Seconds to wait before hiding paths",
                min: 5,
                max: 300,
                interval: 5
            );
        }
    }
}