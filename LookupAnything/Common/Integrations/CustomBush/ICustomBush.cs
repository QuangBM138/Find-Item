// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.CustomBush.ICustomBush
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using StardewValley.GameData;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.CustomBush;

public interface ICustomBush
{
  int AgeToProduce { get; }

  int DayToBeginProducing { get; }

  string Description { get; }

  string DisplayName { get; }

  string IndoorTexture { get; }

  List<Season> Seasons { get; }

  List<PlantableRule> PlantableLocationRules { get; }

  string Texture { get; }

  int TextureSpriteRow { get; }
}
