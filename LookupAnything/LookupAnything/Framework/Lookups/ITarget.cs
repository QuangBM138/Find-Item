// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.ITarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups;

internal interface ITarget
{
  SubjectType Type { get; }

  Vector2 Tile { get; }

  Func<ISubject?> GetSubject { get; }

  int Precedence { get; }

  Rectangle GetSpritesheetArea();

  Rectangle GetWorldArea();

  bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea);
}
