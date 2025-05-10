// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.TileTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

internal class TileTarget : GenericTarget<Vector2>
{
  public TileTarget(GameHelper gameHelper, Vector2 position, Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.Tile, position, position, getSubject)
  {
    this.Precedence = 1000;
  }

  public override Rectangle GetSpritesheetArea() => Rectangle.Empty;
}
