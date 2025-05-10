// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.BushSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common.Integrations.CustomBush;
using Pathoschild.Stardew.Common.Utilities;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData;
using StardewValley.TerrainFeatures;
using StardewValley.TokenizableStrings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class BushSubject : BaseSubject
{
  private readonly Bush Target;
  private readonly ISubjectRegistry Codex;

  public BushSubject(GameHelper gameHelper, ISubjectRegistry codex, Bush bush)
    : base(gameHelper)
  {
    this.Target = bush;
    this.Codex = codex;
    ICustomBush customBush;
    if (this.TryGetCustomBush(bush, out customBush))
      this.Initialize(TokenParser.ParseText(customBush.DisplayName, (Random) null, (TokenParserDelegate) null, (Farmer) null), TokenParser.ParseText(customBush.Description, (Random) null, (TokenParserDelegate) null, (Farmer) null), I18n.Type_Bush());
    else if (this.IsBerryBush(bush))
      this.Initialize(I18n.Bush_Name_Berry(), I18n.Bush_Description_Berry(), I18n.Type_Bush());
    else if (this.IsTeaBush(bush))
      this.Initialize(I18n.Bush_Name_Tea(), I18n.Bush_Description_Tea(), I18n.Type_Bush());
    else
      this.Initialize(I18n.Bush_Name_Plain(), I18n.Bush_Description_Plain(), I18n.Type_Bush());
  }

  public override IEnumerable<ICustomField> GetData()
  {
    BushSubject bushSubject = this;
    Bush bush = bushSubject.Target;
    bool flag = bushSubject.IsBerryBush(bush);
    bool isTeaBush = bushSubject.IsTeaBush(bush);
    SDate today = SDate.Now();
    (string, WorldDate, WorldDate)[] schedule;
    if (flag && bushSubject.TryGetBushBloomSchedules(bush, out schedule))
    {
      List<Item> items = new List<Item>();
      Dictionary<Item, string> displayText = new Dictionary<Item, string>((IEqualityComparer<Item>) new ObjectReferenceComparer<Item>());
      foreach ((string, WorldDate, WorldDate) valueTuple in (IEnumerable<(string, WorldDate, WorldDate)>) ((IEnumerable<(string, WorldDate, WorldDate)>) schedule).OrderBy<(string, WorldDate, WorldDate), int>((Func<(string, WorldDate, WorldDate), int>) (p => p.StartDay.TotalDays)).ThenBy<(string, WorldDate, WorldDate), int>((Func<(string, WorldDate, WorldDate), int>) (p => p.EndDay.TotalDays)))
      {
        SDate sdate1 = SDate.From(valueTuple.Item3);
        SDate sdate2 = SDate.From(valueTuple.Item2);
        Item obj = ItemRegistry.Create(valueTuple.Item1, 1, 0, false);
        items.Add(obj);
        if (SDate.op_LessThan(sdate2, today))
          sdate2 = today;
        if (!SDate.op_LessThan(sdate1, today))
        {
          Dictionary<Item, string> dictionary = displayText;
          Item key = obj;
          string str;
          if (!SDate.op_Equality(sdate2, sdate1))
            str = $"{obj.DisplayName}: {bushSubject.Stringify((object) sdate2)} - {bushSubject.Stringify((object) sdate1)}";
          else
            str = $"{obj.DisplayName}: {bushSubject.Stringify((object) sdate2)}";
          dictionary[key] = str;
        }
      }
      yield return (ICustomField) new ItemIconListField(bushSubject.GameHelper, I18n.Bush_NextHarvest(), (IEnumerable<Item>) items, false, (Func<Item, string>) (item => displayText.GetValueOrDefault<Item, string>(item)));
    }
    else
    {
      if (flag | isTeaBush)
      {
        SDate nextHarvestDate = bushSubject.GetNextHarvestDate(bush);
        string preface = SDate.op_Equality(nextHarvestDate, today) ? I18n.Generic_Now() : $"{bushSubject.Stringify((object) nextHarvestDate)} ({bushSubject.GetRelativeDateStr(nextHarvestDate)})";
        IList<ItemDropData> drops;
        if (bushSubject.TryGetCustomBushDrops(bush, out drops))
        {
          yield return (ICustomField) new ItemDropListField(bushSubject.GameHelper, bushSubject.Codex, I18n.Bush_NextHarvest(), (IEnumerable<ItemDropData>) drops, preface: preface);
        }
        else
        {
          string str = isTeaBush ? I18n.Bush_Schedule_Tea() : I18n.Bush_Schedule_Berry();
          yield return (ICustomField) new GenericField(I18n.Bush_NextHarvest(), preface + Environment.NewLine + str);
        }
      }
      if (isTeaBush)
      {
        SDate datePlanted = bushSubject.GetDatePlanted(bush);
        int num = SDate.Now().DaysSinceStart - datePlanted.DaysSinceStart;
        SDate dateGrown = bushSubject.GetDateFullyGrown(bush);
        yield return (ICustomField) new GenericField(I18n.Bush_DatePlanted(), $"{bushSubject.Stringify((object) datePlanted)} ({bushSubject.GetRelativeDateStr(-num)})");
        if (SDate.op_GreaterThan(dateGrown, today))
        {
          string str = I18n.Bush_Growth_Summary((object) bushSubject.Stringify((object) dateGrown));
          yield return (ICustomField) new GenericField(I18n.Bush_Growth(), $"{str} ({bushSubject.GetRelativeDateStr(dateGrown)})");
        }
        dateGrown = (SDate) null;
      }
    }
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    BushSubject bushSubject = this;
    Bush target = bushSubject.Target;
    yield return (IDebugField) new GenericDebugField("health", target.health, pinned: true);
    yield return (IDebugField) new GenericDebugField("is town bush", bushSubject.Stringify((object) ((NetFieldBase<bool, NetBool>) target.townBush).Value), pinned: true);
    yield return (IDebugField) new GenericDebugField("is in bloom", bushSubject.Stringify((object) target.inBloom()), pinned: true);
    foreach (IDebugField debugField in bushSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    Bush target = this.Target;
    Rectangle rectangle = ((NetFieldBase<Rectangle, NetRectangle>) target.sourceRect).Value;
    Point point1;
    // ISSUE: explicit constructor call
    ((Point) ref point1).\u002Ector(rectangle.Width * 4, rectangle.Height * 4);
    SpriteEffects spriteEffects = ((NetFieldBase<bool, NetBool>) target.flipped).Value ? (SpriteEffects) 1 : (SpriteEffects) 0;
    float num = Math.Min(size.X / (float) point1.X, size.Y / (float) point1.Y);
    Point point2;
    // ISSUE: explicit constructor call
    ((Point) ref point2).\u002Ector((int) ((double) point1.X * (double) num), (int) ((double) point1.Y * (double) num));
    Vector2 vector2 = Vector2.op_Division(new Vector2(size.X - (float) point2.X, size.Y - (float) point2.Y), 2f);
    ICustomBush customBush;
    Texture2D texture2D = !this.TryGetCustomBush(target, out customBush) ? Bush.texture.Value : (target.IsSheltered() ? Game1.content.Load<Texture2D>(customBush.IndoorTexture) : Game1.content.Load<Texture2D>(customBush.Texture));
    spriteBatch.Draw(texture2D, new Rectangle((int) ((double) position.X + (double) vector2.X), (int) ((double) position.Y + (double) vector2.Y), point2.X, point2.Y), new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, spriteEffects, 0.0f);
    return true;
  }

  private bool IsBerryBush(Bush bush)
  {
    return ((NetFieldBase<int, NetInt>) bush.size).Value == 1 && !((NetFieldBase<bool, NetBool>) bush.townBush).Value && !((TerrainFeature) bush).Location.InIslandContext();
  }

  private bool IsTeaBush(Bush bush) => ((NetFieldBase<int, NetInt>) bush.size).Value == 3;

  private bool TryGetCustomBush(Bush bush, [NotNullWhen(true)] out ICustomBush? customBush)
  {
    customBush = (ICustomBush) null;
    return this.GameHelper.CustomBush.IsLoaded && this.GameHelper.CustomBush.ModApi.TryGetCustomBush(bush, out customBush);
  }

  private bool TryGetCustomBushDrops(Bush bush, [NotNullWhen(true)] out IList<ItemDropData>? drops)
  {
    CustomBushIntegration customBush = this.GameHelper.CustomBush;
    string id;
    IList<ICustomBushDrop> drops1;
    if (customBush.IsLoaded && customBush.ModApi.TryGetCustomBush(bush, out ICustomBush _, out id) && customBush.ModApi.TryGetDrops(id, out drops1))
    {
      drops = (IList<ItemDropData>) new List<ItemDropData>(drops1.Count);
      foreach (ICustomBushDrop customBushDrop in (IEnumerable<ICustomBushDrop>) drops1)
        drops.Add(new ItemDropData(((ISpawnItemData) customBushDrop).ItemId, ((ISpawnItemData) customBushDrop).MinStack, ((ISpawnItemData) customBushDrop).MaxStack, customBushDrop.Chance, customBushDrop.Condition));
      return true;
    }
    drops = (IList<ItemDropData>) null;
    return false;
  }

  private bool TryGetBushBloomSchedules(
    Bush bush,
    [NotNullWhen(true)] out (string UnqualifiedItemId, WorldDate StartDay, WorldDate EndDay)[]? schedule)
  {
    if (this.GameHelper.BushBloomMod.IsLoaded && this.GameHelper.BushBloomMod.ModApi.IsReady())
    {
      SDate sdate = SDate.Now();
      schedule = this.GameHelper.BushBloomMod.ModApi.GetActiveSchedules(sdate.Season.ToString(), sdate.Day, new int?(sdate.Year), ((TerrainFeature) bush).Location, new Vector2?(((TerrainFeature) bush).Tile));
      return true;
    }
    schedule = ((string, WorldDate, WorldDate)[]) null;
    return false;
  }

  private SDate GetDatePlanted(Bush bush)
  {
    SDate datePlanted = new SDate(1, (Season) 0, 1);
    if (this.IsTeaBush(bush) && ((NetFieldBase<int, NetInt>) bush.datePlanted).Value > 0)
      datePlanted = datePlanted.AddDays(((NetFieldBase<int, NetInt>) bush.datePlanted).Value);
    return datePlanted;
  }

  private SDate GetDateFullyGrown(Bush bush)
  {
    SDate dateFullyGrown = this.GetDatePlanted(bush);
    ICustomBush customBush;
    if (this.TryGetCustomBush(bush, out customBush))
      dateFullyGrown = dateFullyGrown.AddDays(customBush.AgeToProduce);
    else if (this.IsTeaBush(bush))
      dateFullyGrown = dateFullyGrown.AddDays(20);
    return dateFullyGrown;
  }

  private int GetDayToBeginProducing(Bush bush)
  {
    ICustomBush customBush;
    if (this.TryGetCustomBush(bush, out customBush))
      return customBush.DayToBeginProducing;
    return this.IsTeaBush(bush) ? 22 : -1;
  }

  private List<Season> GetProducingSeasons(Bush bush)
  {
    ICustomBush customBush;
    if (this.TryGetCustomBush(bush, out customBush))
      return customBush.Seasons;
    if (this.IsTeaBush(bush))
      return new List<Season>(3)
      {
        (Season) 0,
        (Season) 1,
        (Season) 2
      };
    return new List<Season>(2) { (Season) 0, (Season) 2 };
  }

  private SDate GetNextHarvestDate(Bush bush)
  {
    SDate nextHarvestDate1 = SDate.Now();
    SDate sdate1 = nextHarvestDate1.AddDays(1);
    if (((NetFieldBase<int, NetInt>) bush.tileSheetOffset).Value == 1)
      return nextHarvestDate1;
    int toBeginProducing = this.GetDayToBeginProducing(bush);
    if (toBeginProducing >= 0)
    {
      SDate nextHarvestDate2 = this.GetDateFullyGrown(bush);
      if (SDate.op_LessThan(nextHarvestDate2, sdate1))
        nextHarvestDate2 = sdate1;
      if (!bush.IsSheltered())
      {
        List<Season> producingSeasons = this.GetProducingSeasons(bush);
        SDate nextHarvestDate3 = new SDate(Math.Max(1, toBeginProducing), nextHarvestDate2.Season, nextHarvestDate2.Year);
        while (!producingSeasons.Contains(nextHarvestDate3.Season))
          nextHarvestDate3 = nextHarvestDate3.AddDays(28);
        if (SDate.op_LessThan(nextHarvestDate2, nextHarvestDate3))
          return nextHarvestDate3;
      }
      if (nextHarvestDate2.Day < toBeginProducing)
        nextHarvestDate2 = new SDate(toBeginProducing, nextHarvestDate2.Season, nextHarvestDate2.Year);
      return nextHarvestDate2;
    }
    SDate nextHarvestDate4 = new SDate(15, (Season) 0);
    SDate sdate2 = new SDate(18, (Season) 0);
    SDate nextHarvestDate5 = new SDate(8, (Season) 2);
    SDate sdate3 = new SDate(11, (Season) 2);
    if (SDate.op_LessThan(sdate1, nextHarvestDate4))
      return nextHarvestDate4;
    if (SDate.op_GreaterThan(sdate1, sdate2) && SDate.op_LessThan(sdate1, nextHarvestDate5))
      return nextHarvestDate5;
    return SDate.op_GreaterThan(sdate1, sdate3) ? new SDate(nextHarvestDate4.Day, nextHarvestDate4.Season, nextHarvestDate4.Year + 1) : sdate1;
  }
}
