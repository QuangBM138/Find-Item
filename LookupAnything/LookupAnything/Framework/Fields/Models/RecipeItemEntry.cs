// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.RecipeItemEntry
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal record RecipeItemEntry(
  SpriteInfo? Sprite,
  string DisplayText,
  int? Quality,
  bool IsGoldPrice,
  bool IsValid = true,
  object? Entity = null)
;
