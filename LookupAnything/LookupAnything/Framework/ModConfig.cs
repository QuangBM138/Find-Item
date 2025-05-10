// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ModConfig
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Runtime.Serialization;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal class ModConfig
{
  public ModConfigKeys Controls { get; set; } = new ModConfigKeys();

  public bool ShowUnknownGiftTastes { get; set; } = true;

  public bool ShowUncaughtFishSpawnRules { get; set; } = true;

  public bool ShowUnknownRecipes { get; set; } = true;

  public bool ShowPuzzleSolutions { get; set; } = true;

  public bool HighlightUnrevealedGiftTastes { get; set; }

  public ModGiftTasteConfig ShowGiftTastes { get; set; } = new ModGiftTasteConfig();

  public ModCollapseLargeFieldsConfig CollapseLargeFields { get; set; } = new ModCollapseLargeFieldsConfig();

  public bool ShowUnownedGifts { get; set; } = true;

  public bool HideOnKeyUp { get; set; }

  public bool EnableTargetRedirection { get; set; } = true;

  public bool EnableTileLookups { get; set; }

  public bool ForceFullScreen { get; set; }

  public int ScrollAmount { get; set; } = 160 /*0xA0*/;

  public bool ShowDataMiningFields { get; set; }

  public bool ShowInvalidRecipes { get; set; }

  [System.Runtime.Serialization.OnDeserialized]
  public void OnDeserialized(StreamingContext context)
  {
    if (this.Controls == null)
    {
      ModConfigKeys modConfigKeys;
      this.Controls = modConfigKeys = new ModConfigKeys();
    }
    if (this.ShowGiftTastes == null)
    {
      ModGiftTasteConfig modGiftTasteConfig;
      this.ShowGiftTastes = modGiftTasteConfig = new ModGiftTasteConfig();
    }
    if (this.CollapseLargeFields != null)
      return;
    ModCollapseLargeFieldsConfig largeFieldsConfig;
    this.CollapseLargeFields = largeFieldsConfig = new ModCollapseLargeFieldsConfig();
  }
}
