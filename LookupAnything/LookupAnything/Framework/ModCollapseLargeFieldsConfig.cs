// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ModCollapseLargeFieldsConfig
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable disable
namespace Pathoschild.Stardew.LookupAnything.Framework;

public class ModCollapseLargeFieldsConfig
{
  public bool Enabled { get; set; } = true;

  public int BuildingRecipes { get; set; } = 25;

  public int ItemRecipes { get; set; } = 25;

  public int NpcGiftTastes { get; set; } = 75;
}
