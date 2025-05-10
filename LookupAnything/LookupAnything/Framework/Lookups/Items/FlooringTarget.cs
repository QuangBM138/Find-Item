// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.FlooringTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class FlooringTarget : GenericTarget<Flooring>
{
  public FlooringTarget(
    GameHelper gameHelper,
    Flooring value,
    Vector2 tilePosition,
    Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.Object, value, tilePosition, getSubject)
  {
    this.Precedence = 999;
  }

  public override Rectangle GetSpritesheetArea() => Rectangle.Empty;
}
