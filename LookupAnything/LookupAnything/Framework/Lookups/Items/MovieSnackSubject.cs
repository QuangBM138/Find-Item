// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.MovieSnackSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class MovieSnackSubject : BaseSubject
{
  private readonly MovieConcession Target;

  public MovieSnackSubject(GameHelper gameHelper, MovieConcession item)
    : base(gameHelper)
  {
    this.Target = item;
    this.Initialize(item.DisplayName, item.getDescription(), I18n.Type_Other());
  }

  public override IEnumerable<ICustomField> GetData()
  {
    MovieSnackSubject movieSnackSubject = this;
    MovieConcession item = movieSnackSubject.Target;
    IModInfo modFromStringId = movieSnackSubject.GameHelper.TryGetModFromStringId(item.Id);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    NPC invitedNpc = ((IEnumerable<MovieInvitation>) Game1.player.team.movieInvitations).FirstOrDefault<MovieInvitation>((Func<MovieInvitation, bool>) (p => p.farmer == Game1.player))?.invitedNPC;
    if (invitedNpc != null)
    {
      string tasteForCharacter = MovieTheater.GetConcessionTasteForCharacter((Character) invitedNpc, item);
      yield return (ICustomField) new GenericField(I18n.Item_MovieSnackPreference(), I18n.ForMovieTasteLabel(tasteForCharacter, ((Character) invitedNpc).displayName));
    }
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    MovieSnackSubject movieSnackSubject = this;
    foreach (IDebugField debugField in movieSnackSubject.GetDebugFieldsFrom((object) movieSnackSubject.Target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    this.Target.drawInMenu(spriteBatch, position, 1f, 1f, 1f, (StackDrawType) 0, Color.White, false);
    return true;
  }
}
