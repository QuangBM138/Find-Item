// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.FarmAnimalSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class FarmAnimalSubject : BaseSubject
{
  private readonly FarmAnimal Target;
  private readonly ISubjectRegistry Codex;

  public FarmAnimalSubject(ISubjectRegistry codex, GameHelper gameHelper, FarmAnimal animal)
    : base(gameHelper, ((Character) animal).displayName, (string) null, animal.displayType)
  {
    this.Codex = codex;
    this.Target = animal;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    FarmAnimalSubject farmAnimalSubject = this;
    FarmAnimal animal = farmAnimalSubject.Target;
    bool isFullyGrown = animal.isAdult();
    int daysUntilGrown = 0;
    SDate dayOfMaturity = (SDate) null;
    if (!isFullyGrown)
    {
      daysUntilGrown = animal.GetAnimalData().DaysToMature - ((NetFieldBase<int, NetInt>) animal.age).Value;
      dayOfMaturity = SDate.Now().AddDays(daysUntilGrown);
    }
    IModInfo modFromStringId = farmAnimalSubject.GameHelper.TryGetModFromStringId(((NetFieldBase<string, NetString>) animal.type).Value);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    yield return (ICustomField) new CharacterFriendshipField(I18n.Animal_Love(), farmAnimalSubject.GameHelper.GetFriendshipForAnimal(Game1.player, animal));
    yield return (ICustomField) new PercentageBarField(I18n.Animal_Happiness(), ((NetFieldBase<int, NetInt>) animal.happiness).Value, (int) byte.MaxValue, Color.Green, Color.Gray, I18n.Generic_Percent((object) (int) Math.Round((double) ((NetFieldBase<int, NetInt>) animal.happiness).Value / ((double) farmAnimalSubject.Constants.AnimalMaxHappiness * 1.0) * 100.0)));
    yield return (ICustomField) new GenericField(I18n.Animal_Mood(), animal.getMoodMessage());
    yield return (ICustomField) new GenericField(I18n.Animal_Complaints(), farmAnimalSubject.GetMoodReason(animal));
    yield return (ICustomField) new ItemIconField(farmAnimalSubject.GameHelper, I18n.Animal_ProduceReady(), CommonHelper.IsItemId(((NetFieldBase<string, NetString>) animal.currentProduce).Value, false) ? ItemRegistry.Create(((NetFieldBase<string, NetString>) animal.currentProduce).Value, 1, 0, false) : (Item) null, farmAnimalSubject.Codex);
    if (!isFullyGrown)
      yield return (ICustomField) new GenericField(I18n.Animal_Growth(), $"{I18n.Generic_Days((object) daysUntilGrown)} ({farmAnimalSubject.Stringify((object) dayOfMaturity)})");
    yield return (ICustomField) new GenericField(I18n.Animal_SellsFor(), GenericField.GetSaleValueString(animal.getSellPrice(), 1));
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    FarmAnimalSubject farmAnimalSubject = this;
    FarmAnimal target = farmAnimalSubject.Target;
    yield return (IDebugField) new GenericDebugField("age", $"{target.age} days", pinned: true);
    yield return (IDebugField) new GenericDebugField("friendship", $"{target.friendshipTowardFarmer} (max {farmAnimalSubject.Constants.AnimalMaxHappiness})", pinned: true);
    yield return (IDebugField) new GenericDebugField("fullness", farmAnimalSubject.Stringify((object) ((NetFieldBase<int, NetInt>) target.fullness).Value), pinned: true);
    yield return (IDebugField) new GenericDebugField("happiness", farmAnimalSubject.Stringify((object) ((NetFieldBase<int, NetInt>) target.happiness).Value), pinned: true);
    foreach (IDebugField debugField in farmAnimalSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    FarmAnimal target = this.Target;
    ((Character) target).Sprite.draw(spriteBatch, position, 1f, 0, 0, Color.White, false, size.X / (float) ((Character) target).Sprite.getWidth(), 0.0f, false);
    return true;
  }

  private string GetMoodReason(FarmAnimal animal)
  {
    List<string> values = new List<string>();
    if (Game1.IsWinter && Game1.currentLocation.numberOfObjectsWithName(Constant.ItemNames.Heater) <= 0)
      values.Add(I18n.Animal_Complaints_NoHeater());
    switch (((NetFieldBase<int, NetInt>) animal.moodMessage).Value)
    {
      case 0:
        values.Add(I18n.Animal_Complaints_NewHome());
        break;
      case 4:
        values.Add(I18n.Animal_Complaints_Hungry());
        break;
      case 5:
        values.Add(I18n.Animal_Complaints_WildAnimalAttack());
        break;
      case 6:
        values.Add(I18n.Animal_Complaints_LeftOut());
        break;
    }
    if (!((NetFieldBase<bool, NetBool>) animal.wasPet).Value)
      values.Add(I18n.Animal_Complaints_NotPetted());
    return I18n.List((IEnumerable<object>) values);
  }
}
