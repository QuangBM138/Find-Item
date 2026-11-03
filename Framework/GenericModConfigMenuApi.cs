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
        void Unregister(IManifest mod);
        void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
        void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
        void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name, Func<string>? tooltip = null, int? min = null, int? max = null, int? interval = null, string? fieldId = null);
    }

    public class GenericModConfigMenuIntegration
    {
        private readonly IManifest _modManifest;
        private readonly IGenericModConfigMenuApi _configMenu;
        private readonly ModConfig _config;
        private readonly IModHelper _helper;
        private static bool _isRegistered = false;

        public GenericModConfigMenuIntegration(IManifest modManifest, IGenericModConfigMenuApi configMenu, ModConfig config, IModHelper helper)
        {
            _modManifest = modManifest ?? throw new ArgumentNullException(nameof(modManifest));
            _configMenu = configMenu ?? throw new ArgumentNullException(nameof(configMenu));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public void Register()
        {
            if (_isRegistered)
                return;

            try
            {
                // Unregister first to prevent duplicate registrations if already registered
                _configMenu.Unregister(_modManifest);
            }
            catch
            {
                // Unregister method might not exist in older GMCM versions
            }

            // Register mod configuration
            _configMenu.Register(
                mod: _modManifest,
                reset: () => {
                    ModConfig defaults = new ModConfig();
                    _config.ModEnabled = defaults.ModEnabled;
                    _config.ToggleKey = defaults.ToggleKey;
                    _config.SearchKey = defaults.SearchKey;
                    _config.QuickSearchKey = defaults.QuickSearchKey;
                    _config.HidePathKey = defaults.HidePathKey;
                    _config.AutoHidePaths = defaults.AutoHidePaths;
                    _config.AutoHideDelay = defaults.AutoHideDelay;
                    _helper.WriteConfig(_config);
                },
                save: () => _helper.WriteConfig(_config)
            );

            // Add mod enable/disable option
            _configMenu.AddBoolOption(
                mod: _modManifest,
                getValue: () => _config.ModEnabled,
                setValue: value => _config.ModEnabled = value,
                name: () => _helper.Translation.Get("config.mod-enabled.name"),
                tooltip: () => _helper.Translation.Get("config.mod-enabled.desc")
            );

            // Add toggle key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.ToggleKey,
                setValue: value => _config.ToggleKey = value,
                name: () => _helper.Translation.Get("config.toggle-key.name"),
                tooltip: () => _helper.Translation.Get("config.toggle-key.desc")
            );

            // Add search menu key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.SearchKey,
                setValue: value => _config.SearchKey = value,
                name: () => _helper.Translation.Get("config.search-key.name"),
                tooltip: () => _helper.Translation.Get("config.search-key.desc")
            );

            // Add quick search key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.QuickSearchKey,
                setValue: value => _config.QuickSearchKey = value,
                name: () => _helper.Translation.Get("config.quick-search-key.name"),
                tooltip: () => _helper.Translation.Get("config.quick-search-key.desc")
            );

            // Add hide path key binding
            _configMenu.AddKeybind(
                mod: _modManifest,
                getValue: () => _config.HidePathKey,
                setValue: value => _config.HidePathKey = value,
                name: () => _helper.Translation.Get("config.hide-path-key.name"),
                tooltip: () => _helper.Translation.Get("config.hide-path-key.desc")
            );

            // Add auto-hide paths option
            _configMenu.AddBoolOption(
                mod: _modManifest,
                getValue: () => _config.AutoHidePaths,
                setValue: value => _config.AutoHidePaths = value,
                name: () => _helper.Translation.Get("config.auto-hide-paths.name"),
                tooltip: () => _helper.Translation.Get("config.auto-hide-paths.desc")
            );

            // Add auto-hide delay option
            _configMenu.AddNumberOption(
                mod: _modManifest,
                getValue: () => _config.AutoHideDelay,
                setValue: value => _config.AutoHideDelay = value,
                name: () => _helper.Translation.Get("config.auto-hide-delay.name"),
                tooltip: () => _helper.Translation.Get("config.auto-hide-delay.desc"),
                min: 5,
                max: 300,
                interval: 5
            );

            _isRegistered = true;
        }
    }
}
