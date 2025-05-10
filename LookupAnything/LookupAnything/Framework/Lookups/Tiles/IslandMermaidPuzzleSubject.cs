// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.IslandMermaidPuzzleSubject
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

internal class IslandMermaidPuzzleSubject : TileSubject
{
  public IslandMermaidPuzzleSubject(
    GameHelper gameHelper,
    ModConfig config,
    GameLocation location,
    Vector2 position,
    bool showRawTileInfo)
    : base(gameHelper, config, location, position, showRawTileInfo)
  {
    this.Name = I18n.Puzzle_IslandMermaid_Title();
    this.Description = (string) null;
    this.Type = (string) null;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    IslandMermaidPuzzleSubject mermaidPuzzleSubject = this;
    IslandSouthEast location = (IslandSouthEast) mermaidPuzzleSubject.Location;
    bool complete = ((NetFieldBase<bool, NetBool>) location.mermaidPuzzleFinished).Value;
    if (!mermaidPuzzleSubject.Config.ShowPuzzleSolutions && !complete)
    {
      yield return (ICustomField) new GenericField(I18n.Puzzle_Solution(), I18n.Puzzle_Solution_Hidden());
    }
    else
    {
      int[] fluteBlockSequence = mermaidPuzzleSubject.GameHelper.Metadata.PuzzleSolutions.IslandMermaidFluteBlockSequence;
      int songIndex = location.songIndex;
      CheckboxList checkboxList = new CheckboxList(((IEnumerable<int>) fluteBlockSequence).Select<int, Checkbox>((Func<int, int, Checkbox>) ((pitch, i) =>
      {
        string text = this.Stringify((object) pitch);
        return new Checkbox(complete || songIndex >= i, text);
      })).ToArray<Checkbox>());
      checkboxList.AddIntro(complete ? I18n.Puzzle_Solution_Solved() : I18n.Puzzle_IslandMermaid_Solution_Intro());
      yield return (ICustomField) new CheckboxListField(I18n.Puzzle_Solution(), new CheckboxList[1]
      {
        checkboxList
      });
    }
    // ISSUE: reference to a compiler-generated method
    foreach (ICustomField customField in mermaidPuzzleSubject.\u003C\u003En__0())
      yield return customField;
  }
}
