// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.CustomFarmingRedux.CustomFarmingReduxIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.CustomFarmingRedux;

internal class CustomFarmingReduxIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<ICustomFarmingApi>("Custom Farming Redux", "Platonymous.CustomFarming", "2.8.5", modRegistry, monitor)
{
  public SpriteInfo? GetSprite(Object obj)
  {
    this.AssertLoaded();
    Tuple<Item, Texture2D, Rectangle, Color> realItemAndTexture = this.ModApi.getRealItemAndTexture(obj);
    return realItemAndTexture == null ? (SpriteInfo) null : new SpriteInfo(realItemAndTexture.Item2, realItemAndTexture.Item3);
  }
}
