// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.CustomFarmingRedux.ICustomFarmingApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.CustomFarmingRedux;

public interface ICustomFarmingApi
{
  Tuple<Item, Texture2D, Rectangle, Color>? getRealItemAndTexture(Object dummy);
}
