// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.FarmerSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Integrations.SpaceCore;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewValley;
using StardewValley.GameData;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class FarmerSubject : BaseSubject
{
  private readonly Farmer Target;
  private readonly bool IsLoadMenu;
  private readonly Lazy<XElement?>? RawSaveData;

  public FarmerSubject(GameHelper gameHelper, Farmer farmer, bool isLoadMenu = false)
    : base(gameHelper, ((Character) farmer).Name, (string) null, I18n.Type_Player())
  {
    FarmerSubject farmerSubject = this;
    this.Target = farmer;
    this.IsLoadMenu = isLoadMenu;
    this.RawSaveData = isLoadMenu ? new Lazy<XElement>((Func<XElement>) (() => farmerSubject.ReadSaveFile(farmer.slotName))) : (Lazy<XElement>) null;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    FarmerSubject farmerSubject = this;
    Farmer target = farmerSubject.Target;
    yield return (ICustomField) new GenericField(I18n.Player_Gender(), target.IsMale ? I18n.Player_Gender_Male() : I18n.Player_Gender_Female());
    yield return (ICustomField) new GenericField(I18n.Player_FarmName(), ((NetFieldBase<string, NetString>) target.farmName).Value);
    yield return (ICustomField) new GenericField(I18n.Player_FarmMap(), farmerSubject.GetFarmType());
    yield return (ICustomField) new GenericField(I18n.Player_FavoriteThing(), ((NetFieldBase<string, NetString>) target.favoriteThing).Value);
    yield return (ICustomField) new GenericField(Game1.player.spouse == "Krobus" ? I18n.Player_Housemate() : I18n.Player_Spouse(), farmerSubject.GetSpouseName());
    if (Utility.doesMasterPlayerHaveMailReceivedButNotMailForTomorrow("ccMovieTheater"))
      yield return (ICustomField) new GenericField(I18n.Player_WatchedMovieThisWeek(), farmerSubject.Stringify((object) (((NetFieldBase<int, NetInt>) target.lastSeenMovieWeek).Value >= Game1.Date.TotalSundayWeeks)));
    int maxSkillPoints = farmerSubject.Constants.PlayerMaxSkillPoints;
    int[] skillPointsPerLevel = farmerSubject.Constants.PlayerSkillPointsPerLevel;
    yield return (ICustomField) new SkillBarField(I18n.Player_FarmingSkill(), target.experiencePoints[0], maxSkillPoints, skillPointsPerLevel);
    yield return (ICustomField) new SkillBarField(I18n.Player_MiningSkill(), target.experiencePoints[3], maxSkillPoints, skillPointsPerLevel);
    yield return (ICustomField) new SkillBarField(I18n.Player_ForagingSkill(), target.experiencePoints[2], maxSkillPoints, skillPointsPerLevel);
    yield return (ICustomField) new SkillBarField(I18n.Player_FishingSkill(), target.experiencePoints[1], maxSkillPoints, skillPointsPerLevel);
    yield return (ICustomField) new SkillBarField(I18n.Player_CombatSkill(), target.experiencePoints[4], maxSkillPoints, skillPointsPerLevel);
    SpaceCoreIntegration spaceCore = farmerSubject.GameHelper.SpaceCore;
    if (spaceCore.IsLoaded)
    {
      string[] strArray = spaceCore.ModApi.GetCustomSkills();
      for (int index = 0; index < strArray.Length; ++index)
      {
        string skill = strArray[index];
        yield return (ICustomField) new SkillBarField(spaceCore.ModApi.GetDisplayNameOfCustomSkill(skill), spaceCore.ModApi.GetExperienceForCustomSkill(target, skill), maxSkillPoints, skillPointsPerLevel);
      }
      strArray = (string[]) null;
    }
    string str = I18n.Player_Luck_Summary((object) ((Game1.player.DailyLuck >= 0.0 ? "+" : "") + Math.Round(Game1.player.DailyLuck * 100.0, 2).ToString()));
    yield return (ICustomField) new GenericField(I18n.Player_Luck(), $"{farmerSubject.GetSpiritLuckMessage()}{Environment.NewLine}({str})");
    if (farmerSubject.IsLoadMenu)
      yield return (ICustomField) new GenericField(I18n.Player_SaveFormat(), farmerSubject.GetSaveFormat(farmerSubject.RawSaveData?.Value));
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    FarmerSubject farmerSubject = this;
    Farmer target = farmerSubject.Target;
    yield return (IDebugField) new GenericDebugField("immunity", target.Immunity, pinned: true);
    yield return (IDebugField) new GenericDebugField("defense", target.buffs.Defense, pinned: true);
    yield return (IDebugField) new GenericDebugField("magnetic radius", target.MagneticRadius, pinned: true);
    foreach (IDebugField debugField in farmerSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    Farmer target = this.Target;
    if (this.IsLoadMenu)
    {
      target.FarmerRenderer.draw(spriteBatch, new FarmerSprite.AnimationFrame(0, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), 0, new Rectangle(0, 0, 16 /*0x10*/, 32 /*0x20*/), position, Vector2.Zero, 0.8f, 2, Color.White, 0.0f, 1f, target);
    }
    else
    {
      FarmerSprite farmerSprite = target.FarmerSprite;
      target.FarmerRenderer.draw(spriteBatch, farmerSprite.CurrentAnimationFrame, ((AnimatedSprite) farmerSprite).CurrentFrame, ((AnimatedSprite) farmerSprite).SourceRect, position, Vector2.Zero, 0.8f, Color.White, 0.0f, 1f, target);
    }
    return true;
  }

  private string? GetSpiritLuckMessage()
  {
    if (this.IsLoadMenu)
    {
      string s = this.RawSaveData?.Value?.Element((XName) "dailyLuck")?.Value;
      if (s == null)
        return (string) null;
      ((NetFieldBase<double, NetDouble>) Game1.player.team.sharedDailyLuck).Value = double.Parse(s);
    }
    return new TV().getFortuneForecast(this.Target);
  }

  private string? GetFarmType()
  {
    string s = this.IsLoadMenu ? this.RawSaveData?.Value?.Element((XName) "whichFarm")?.Value ?? Game1.GetFarmTypeID() : Game1.GetFarmTypeID();
    int result;
    if (int.TryParse(s, out result))
    {
      string str1;
      switch (result)
      {
        case 0:
          str1 = "Character_FarmStandard";
          break;
        case 1:
          str1 = "Character_FarmFishing";
          break;
        case 2:
          str1 = "Character_FarmForaging";
          break;
        case 3:
          str1 = "Character_FarmMining";
          break;
        case 4:
          str1 = "Character_FarmCombat";
          break;
        case 5:
          str1 = "Character_FarmFourCorners";
          break;
        case 6:
          str1 = "Character_FarmBeach";
          break;
        default:
          str1 = (string) null;
          break;
      }
      string str2 = str1;
      if (str2 != null)
        return GameI18n.GetString("Strings\\UI:" + str2).Replace("_", Environment.NewLine);
    }
    foreach (ModFarmType additionalFarm in DataLoader.AdditionalFarms(Game1.content))
    {
      if (additionalFarm?.Id == s && additionalFarm.TooltipStringPath != null)
        return GameI18n.GetString(additionalFarm.TooltipStringPath).Replace("_", Environment.NewLine);
    }
    return I18n.Player_FarmMap_Custom();
  }

  private string? GetSpouseName()
  {
    if (this.IsLoadMenu)
      return this.Target.spouse;
    long? spouse = this.Target.team.GetSpouse(this.Target.UniqueMultiplayerID);
    string displayName = (spouse.HasValue ? (Character) Game1.GetPlayer(spouse.Value, false) : (Character) null)?.displayName;
    if (displayName != null)
      return displayName;
    return ((Character) Game1.player.getSpouse())?.displayName;
  }

  private XElement? ReadSaveFile(string? slotName)
  {
    if (slotName == null)
      return (XElement) null;
    FileInfo fileInfo = new FileInfo(Path.Combine(StardewModdingAPI.Constants.SavesPath, slotName, slotName));
    return !fileInfo.Exists ? (XElement) null : XElement.Parse(File.ReadAllText(fileInfo.FullName));
  }

  private string GetSaveFormat(XElement? saveData)
  {
    if (saveData == null)
      return "???";
    string saveFormat = saveData.Element((XName) "gameVersion")?.Value;
    if (!string.IsNullOrWhiteSpace(saveFormat))
      return saveFormat;
    if (saveData.Element((XName) "hasApplied1_4_UpdateChanges") != null)
      return "1.4";
    if (saveData.Element((XName) "hasApplied1_3_UpdateChanges") != null)
      return "1.3";
    return saveData.Element((XName) "whichFarm") != null ? "1.1 - 1.2" : "1.0";
  }
}
