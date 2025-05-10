// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.BaseLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups;

internal abstract class BaseLookupProvider : ILookupProvider
{
  protected readonly IReflectionHelper Reflection;
  protected readonly GameHelper GameHelper;

  public virtual IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    yield break;
  }

  public virtual ISubject? GetSubject(IClickableMenu menu, int cursorX, int cursorY)
  {
    return (ISubject) null;
  }

  public virtual ISubject? GetSubjectFor(object entity, GameLocation? location) => (ISubject) null;

  public virtual IEnumerable<ISubject> GetSearchSubjects()
  {
    yield break;
  }

  protected BaseLookupProvider(IReflectionHelper reflection, GameHelper gameHelper)
  {
    this.Reflection = reflection;
    this.GameHelper = gameHelper;
  }
}
