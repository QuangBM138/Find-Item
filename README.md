# Find Item

Find Item is a mod for Stardew Valley that helps you quickly locate where your items are stored—whether in your inventory, chests, fridge, or other containers.

## ⭐ Features

- Instantly search for any item you own and see exactly where it is stored.
- Supports searching across inventory, chests, fridge, and more.
- Configurable hotkeys for seamless searching.
- Saves you time and frustration—no more manually checking every chest!

## 🛠 Requirements

- Stardew Valley version **1.6** or higher
- SMAPI version **4.0** or higher
- Generic Mod Config Menu (optional)

## 📥 Installation

1. Download the latest Find Item mod release.
2. Extract the contents into your `Mods` folder within your Stardew Valley directory.
3. Make sure you have the required version of SMAPI installed.
4. Launch the game to start using the mod.

## 🎮 How to Use

Find Item is configured and operated through its `config.json` file, which is generated in the `Mods/Find-Item` folder after you launch the game once with the mod installed.

### Config Overview

Below is the default configuration:
```json
{
  "ToggleKey": "None",
  "SearchKey": "F2",
  "QuickSearchKey": "D",
  "HidePathKey": "H",
  "ModEnabled": true,
  "AutoHidePaths": true,
  "AutoHideDelay": 60
}
```

#### Config Options Explained

- **ToggleKey**: Set a key to enable/disable the mod. `"None"` means no toggle.
- **SearchKey**: Key to open the item search interface. Default is `F2`.
- **QuickSearchKey**: Key for quick search functionality. Default is `D`.
- **HidePathKey**: Key to hide item location paths in the UI. Default is `H`.
- **ModEnabled**: Enables or disables the mod (`true`/`false`).
- **AutoHidePaths**: Automatically hides item paths after a set delay (`true`/`false`).
- **AutoHideDelay**: Time in seconds before paths auto-hide. Default is `60`.

You can edit these settings in the `config.json` file using any text editor. Save your changes and restart the game for them to take effect.

## 📦 Releases

- [Latest Release on GitHub](https://github.com/QuangBM138/Find-Item/releases)
- [Source Code](https://github.com/QuangBM138/Find-Item)
- [Nexus Mods Page](https://www.nexusmods.com/stardewvalley/mods/34024)

## 💡 Feedback & Support

Encounter a bug or have a suggestion? Please open an issue on the [GitHub Issues page](https://github.com/QuangBM138/Find-Item/issues).
