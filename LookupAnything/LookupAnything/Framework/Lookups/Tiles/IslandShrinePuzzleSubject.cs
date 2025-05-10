// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.IslandShrinePuzzleSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using StardewValley;
using StardewValley.Locations;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

internal class IslandShrinePuzzleSubject : TileSubject
{
  public IslandShrinePuzzleSubject(
    GameHelper gameHelper,
    ModConfig config,
    GameLocation location,
    Vector2 position,
    bool showRawTileInfo)
    : base(gameHelper, config, location, position, showRawTileInfo)
  {
    this.Name = I18n.Puzzle_IslandShrine_Title();
    this.Description = (string) null;
    this.Type = (string) null;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    IslandShrinePuzzleSubject shrinePuzzleSubject = this;
    IslandShrine location = (IslandShrine) shrinePuzzleSubject.Location;
    bool flag = ((NetFieldBase<bool, NetBool>) location.puzzleFinished).Value;
    if (!shrinePuzzleSubject.Config.ShowPuzzleSolutions && !flag)
    {
      yield return (ICustomField) new GenericField(I18n.Puzzle_Solution(), (IFormattedText) new FormattedText(I18n.Puzzle_Solution_Hidden(), new Color?(Color.Gray)));
    }
    else
    {
      Checkbox[] checkboxes = new Checkbox[4];
      string text1 = I18n.Puzzle_IslandShrine_Solution_North((object) ((Item) ((NetFieldBase<Object, NetRef<Object>>) location.northPedestal.requiredItem).Value).DisplayName);
      checkboxes[0] = new Checkbox(flag || ((NetFieldBase<bool, NetBool>) location.northPedestal.match).Value, text1);
      string text2 = I18n.Puzzle_IslandShrine_Solution_East((object) ((Item) ((NetFieldBase<Object, NetRef<Object>>) location.eastPedestal.requiredItem).Value).DisplayName);
      checkboxes[1] = new Checkbox(flag || ((NetFieldBase<bool, NetBool>) location.eastPedestal.match).Value, text2);
      string text3 = I18n.Puzzle_IslandShrine_Solution_South((object) ((Item) ((NetFieldBase<Object, NetRef<Object>>) location.southPedestal.requiredItem).Value).DisplayName);
      checkboxes[2] = new Checkbox(flag || ((NetFieldBase<bool, NetBool>) location.southPedestal.match).Value, text3);
      string text4 = I18n.Puzzle_IslandShrine_Solution_West((object) ((Item) ((NetFieldBase<Object, NetRef<Object>>) location.westPedestal.requiredItem).Value).DisplayName);
      checkboxes[3] = new Checkbox(flag || ((NetFieldBase<bool, NetBool>) location.westPedestal.match).Value, text4);
      CheckboxList checkboxList = new CheckboxList(checkboxes);
      checkboxList.AddIntro(flag ? I18n.Puzzle_Solution_Solved() : I18n.Puzzle_IslandShrine_Solution());
      yield return (ICustomField) new CheckboxListField(I18n.Puzzle_Solution(), new CheckboxList[1]
      {
        checkboxList
      });
    }
    // ISSUE: reference to a compiler-generated method
    foreach (ICustomField customField in shrinePuzzleSubject.\u003C\u003En__0())
      yield return customField;
  }
}
