// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.ILookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups;

internal interface ILookupProvider
{
  IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile);

  ISubject? GetSubject(IClickableMenu menu, int cursorX, int cursorY);

  ISubject? GetSubjectFor(object entity, GameLocation? location);

  IEnumerable<ISubject> GetSearchSubjects();
}
