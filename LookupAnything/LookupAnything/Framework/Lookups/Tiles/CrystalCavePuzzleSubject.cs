// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.CrystalCavePuzzleSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using StardewValley;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

internal class CrystalCavePuzzleSubject : TileSubject
{
  private readonly int? CrystalId;

  public CrystalCavePuzzleSubject(
    GameHelper gameHelper,
    ModConfig config,
    GameLocation location,
    Vector2 position,
    bool showRawTileInfo,
    int? crystalId)
    : base(gameHelper, config, location, position, showRawTileInfo)
  {
    this.Name = I18n.Puzzle_IslandCrystalCave_Title();
    this.Description = (string) null;
    this.Type = (string) null;
    this.CrystalId = crystalId;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    CrystalCavePuzzleSubject cavePuzzleSubject = this;
    IslandWestCave1 cave = (IslandWestCave1) cavePuzzleSubject.Location;
    if (cavePuzzleSubject.CrystalId.HasValue && cavePuzzleSubject.Config.ShowPuzzleSolutions)
      yield return (ICustomField) new GenericField(I18n.Puzzle_IslandCrystalCave_CrystalId(), cavePuzzleSubject.Stringify((object) cavePuzzleSubject.CrystalId.Value));
    string label = I18n.Puzzle_Solution();
    if (((NetFieldBase<bool, NetBool>) cave.completed).Value)
      yield return (ICustomField) new GenericField(label, I18n.Puzzle_Solution_Solved());
    else if (!cavePuzzleSubject.Config.ShowPuzzleSolutions)
      yield return (ICustomField) new GenericField(label, (IFormattedText) new FormattedText(I18n.Puzzle_Solution_Hidden(), new Color?(Color.Gray)));
    else if (!((NetFieldBase<bool, NetBool>) cave.isActivated).Value)
      yield return (ICustomField) new GenericField(label, I18n.Puzzle_IslandCrystalCave_Solution_NotActivated());
    else if (!cave.currentCrystalSequence.Any())
    {
      yield return (ICustomField) new GenericField(label, I18n.Puzzle_IslandCrystalCave_Solution_Waiting());
    }
    else
    {
      CheckboxList checkboxList = new CheckboxList(((IEnumerable<int>) cave.currentCrystalSequence).Select<int, Checkbox>((Func<int, int, Checkbox>) ((id, index) =>
      {
        string text = this.Stringify((object) (id + 1));
        return new Checkbox(((NetFieldBase<int, NetInt>) cave.currentCrystalSequenceIndex).Value > index, text);
      })).ToArray<Checkbox>());
      checkboxList.AddIntro(I18n.Puzzle_IslandCrystalCave_Solution_Activated());
      yield return (ICustomField) new CheckboxListField(label, new CheckboxList[1]
      {
        checkboxList
      });
    }
    // ISSUE: reference to a compiler-generated method
    foreach (ICustomField customField in cavePuzzleSubject.\u003C\u003En__0())
      yield return customField;
  }
}
