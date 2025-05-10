// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.CharacterSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.GameData;
using StardewValley.GameData.Pets;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.TokenizableStrings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class CharacterSubject : BaseSubject
{
  private readonly SubjectType TargetType;
  private readonly NPC Target;
  private readonly ISubjectRegistry Codex;
  private readonly bool ShowUnknownGiftTastes;
  private readonly bool HighlightUnrevealedGiftTastes;
  private readonly ModGiftTasteConfig ShowGiftTastes;
  private readonly ModCollapseLargeFieldsConfig CollapseFieldsConfig;
  private readonly bool EnableTargetRedirection;
  private readonly bool ShowUnownedGifts;
  private readonly bool IsGourmand;
  private readonly bool IsHauntedSkull;
  private readonly bool IsMagmaSprite;
  private readonly bool DisablePortraits;

  public CharacterSubject(
    ISubjectRegistry codex,
    GameHelper gameHelper,
    NPC npc,
    SubjectType type,
    Metadata metadata,
    bool showUnknownGiftTastes,
    bool highlightUnrevealedGiftTastes,
    ModGiftTasteConfig showGiftTastes,
    ModCollapseLargeFieldsConfig collapseFieldsConfig,
    bool enableTargetRedirection,
    bool showUnownedGifts)
    : base(gameHelper)
  {
    this.Codex = codex;
    this.ShowUnknownGiftTastes = showUnknownGiftTastes;
    this.HighlightUnrevealedGiftTastes = highlightUnrevealedGiftTastes;
    this.ShowGiftTastes = showGiftTastes;
    this.CollapseFieldsConfig = collapseFieldsConfig;
    this.EnableTargetRedirection = enableTargetRedirection;
    this.ShowUnownedGifts = showUnownedGifts;
    this.Target = npc;
    this.TargetType = type;
    CharacterData character = metadata.GetCharacter(npc, type);
    this.Initialize(npc.getName(), (object) character == null || character.DescriptionKey == null ? (string) null : Translation.op_Implicit(I18n.GetByKey(character.DescriptionKey)), CharacterSubject.GetTypeName((Character) npc, type));
    if (npc is Bat bat)
    {
      this.IsHauntedSkull = ((NetFieldBase<bool, NetBool>) bat.hauntedSkull).Value;
      this.IsMagmaSprite = ((NetFieldBase<bool, NetBool>) bat.magmaSprite).Value;
    }
    else
      this.IsGourmand = type == SubjectType.Villager && ((Character) npc).Name == "Gourmand" && ((Character) npc).currentLocation.Name == "IslandFarmCave";
    this.DisablePortraits = CharacterSubject.ShouldDisablePortraits(npc, this.IsGourmand);
  }

  public override IEnumerable<ICustomField> GetData()
  {
    CharacterSubject characterSubject = this;
    NPC npc = characterSubject.Target;
    IModInfo modFromStringId = characterSubject.GameHelper.TryGetModFromStringId(((Character) npc).Name);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    IEnumerable<ICustomField> customFields;
    switch (characterSubject.TargetType)
    {
      case SubjectType.Monster:
        customFields = characterSubject.GetDataForMonster((Monster) npc);
        break;
      case SubjectType.Pet:
        customFields = characterSubject.GetDataForPet((Pet) npc);
        break;
      case SubjectType.Villager:
        NPC npc1 = npc;
        customFields = npc1 is Child child ? characterSubject.GetDataForChild(child) : (npc1 is TrashBear trashBear ? characterSubject.GetDataForTrashBear(trashBear) : (!characterSubject.IsGourmand ? characterSubject.GetDataForVillager(npc) : characterSubject.GetDataForGourmand()));
        break;
      default:
        customFields = (IEnumerable<ICustomField>) Array.Empty<ICustomField>();
        break;
    }
    foreach (ICustomField customField in customFields)
      yield return customField;
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    CharacterSubject characterSubject = this;
    NPC target = characterSubject.Target;
    Pet pet = target as Pet;
    yield return (IDebugField) new GenericDebugField("facing direction", characterSubject.Stringify((object) (FacingDirection) ((Character) target).FacingDirection), pinned: true);
    yield return (IDebugField) new GenericDebugField("walking towards player", characterSubject.Stringify((object) target.IsWalkingTowardPlayer), pinned: true);
    if (((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData).ContainsKey(((Character) target).Name))
    {
      FriendshipModel friendshipForVillager = characterSubject.GameHelper.GetFriendshipForVillager(Game1.player, target, ((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData)[((Character) target).Name]);
      yield return (IDebugField) new GenericDebugField("friendship", $"{friendshipForVillager.Points} (max {friendshipForVillager.MaxPoints})", pinned: true);
    }
    if (pet != null)
      yield return (IDebugField) new GenericDebugField("friendship", $"{pet.friendshipTowardFarmer} of {1000})", pinned: true);
    foreach (IDebugField debugField in characterSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    NPC target = this.Target;
    if (this.IsHauntedSkull || this.IsMagmaSprite)
    {
      Rectangle standardTileSheet = Game1.getSourceRectForStandardTileSheet(((Character) target).Sprite.Texture, 4, 16 /*0x10*/, 16 /*0x10*/);
      spriteBatch.Draw(((Character) target).Sprite.Texture, position, new Rectangle?(standardTileSheet), Color.White, 0.0f, Vector2.Zero, new Vector2(size.X / 16f), (SpriteEffects) 0, 1f);
      return true;
    }
    if (((Character) target).IsVillager && !this.DisablePortraits)
    {
      spriteBatch.DrawSprite(target.Portrait, new Rectangle(0, 0, 64 /*0x40*/, 64 /*0x40*/), position.X, position.Y, new Point(64 /*0x40*/, 64 /*0x40*/), new Color?(Color.White), size.X / 64f);
      return true;
    }
    ((Character) target).Sprite.draw(spriteBatch, position, 1f, 0, 0, Color.White, false, size.X / (float) ((Character) target).Sprite.getWidth(), 0.0f, false);
    return true;
  }

  private IEnumerable<ICustomField> GetDataForChild(Child child)
  {
    CharacterSubject characterSubject = this;
    yield return (ICustomField) new GenericField(I18n.Npc_Birthday(), characterSubject.GetChildBirthdayString(child));
    ChildAge age = (ChildAge) ((NPC) child).Age;
    int daysOld = ((NetFieldBase<int, NetInt>) child.daysOld).Value;
    int toNextChildGrowth = characterSubject.GetDaysToNextChildGrowth(age, daysOld);
    bool flag = toNextChildGrowth == -1;
    int maxValue = daysOld + (flag ? 0 : toNextChildGrowth);
    string text = flag ? I18n.Npc_Child_Age_DescriptionGrown((object) I18n.For(age)) : I18n.Npc_Child_Age_DescriptionPartial((object) I18n.For(age), (object) toNextChildGrowth, (object) I18n.For(age + 1));
    yield return (ICustomField) new PercentageBarField(I18n.Npc_Child_Age(), ((NetFieldBase<int, NetInt>) child.daysOld).Value, maxValue, Color.Green, Color.Gray, text);
    if (((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData).ContainsKey(((Character) child).Name))
    {
      FriendshipModel friendshipForVillager = characterSubject.GameHelper.GetFriendshipForVillager(Game1.player, (NPC) child, ((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData)[((Character) child).Name]);
      yield return (ICustomField) new CharacterFriendshipField(I18n.Npc_Friendship(), friendshipForVillager);
      yield return (ICustomField) new GenericField(I18n.Npc_TalkedToday(), characterSubject.Stringify((object) ((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData)[((Character) child).Name].TalkedToToday));
    }
  }

  private IEnumerable<ICustomField> GetDataForGourmand()
  {
    IslandFarmCave locationFromName = (IslandFarmCave) Game1.getLocationFromName("IslandFarmCave");
    if (locationFromName != null)
    {
      int questsDone = ((NetFieldBase<int, NetInt>) locationFromName.gourmandRequestsFulfilled).Value;
      int maxQuests = IslandFarmCave.TOTAL_GOURMAND_REQUESTS;
      if (questsDone <= maxQuests)
      {
        List<Checkbox> source = new List<Checkbox>();
        for (int index = 0; index < maxQuests; ++index)
        {
          string itemId = locationFromName.IndexForRequest(index);
          if (CommonHelper.IsItemId(itemId))
          {
            List<Checkbox> checkboxList = source;
            string displayName = ItemRegistry.GetDataOrErrorItem(itemId).DisplayName;
            Checkbox checkbox = new Checkbox(questsDone > index, displayName);
            checkboxList.Add(checkbox);
          }
        }
        if (source.Any<Checkbox>())
          yield return (ICustomField) new CheckboxListField(I18n.TrashBearOrGourmand_ItemWanted(), new CheckboxList[1]
          {
            new CheckboxList((IEnumerable<Checkbox>) source)
          });
      }
      yield return (ICustomField) new GenericField(I18n.TrashBearOrGourmand_QuestProgress(), I18n.Generic_Ratio((object) questsDone, (object) maxQuests));
    }
  }

  private IEnumerable<ICustomField> GetDataForMonster(Monster monster)
  {
    CharacterSubject characterSubject = this;
    bool canRerollDrops = Game1.player.isWearingRing("526");
    yield return (ICustomField) new GenericField(I18n.Monster_Invincible(), I18n.Generic_Seconds((object) monster.invincibleCountdown), new bool?(monster.isInvincible()));
    yield return (ICustomField) new PercentageBarField(I18n.Monster_Health(), monster.Health, monster.MaxHealth, Color.Green, Color.Gray, I18n.Generic_PercentRatio((object) (int) Math.Round((double) monster.Health / ((double) monster.MaxHealth * 1.0) * 100.0), (object) monster.Health, (object) monster.MaxHealth));
    yield return (ICustomField) new ItemDropListField(characterSubject.GameHelper, characterSubject.Codex, I18n.Monster_Drops(), characterSubject.GetMonsterDrops(monster), fadeNonGuaranteed: true, crossOutNonGuaranteed: !canRerollDrops, defaultText: I18n.Monster_Drops_Nothing());
    yield return (ICustomField) new GenericField(I18n.Monster_Experience(), characterSubject.Stringify((object) monster.ExperienceGained));
    yield return (ICustomField) new GenericField(I18n.Monster_Defense(), characterSubject.Stringify((object) ((NetFieldBase<int, NetInt>) monster.resilience).Value));
    yield return (ICustomField) new GenericField(I18n.Monster_Attack(), characterSubject.Stringify((object) monster.DamageToFarmer));
    foreach (MonsterSlayerQuestData monsterSlayerQuestData in DataLoader.MonsterSlayerQuests(Game1.content).Values)
    {
      bool? nullable = monsterSlayerQuestData.Targets?.Contains(((Character) monster).Name);
      if (nullable.HasValue && nullable.GetValueOrDefault())
      {
        int count = monsterSlayerQuestData.Targets.Sum<string>(new Func<string, int>(Game1.stats.getMonstersKilled));
        string text = I18n.Monster_AdventureGuild_EradicationGoal((object) TokenParser.ParseText(monsterSlayerQuestData.DisplayName, (Random) null, (TokenParserDelegate) null, (Farmer) null), (object) count, (object) monsterSlayerQuestData.Count);
        Checkbox checkbox = new Checkbox(count >= monsterSlayerQuestData.Count, text);
        yield return (ICustomField) new CheckboxListField(I18n.Monster_AdventureGuild(), new CheckboxList[1]
        {
          new CheckboxList(new Checkbox[1]{ checkbox })
        });
      }
    }
  }

  private IEnumerable<ICustomField> GetDataForPet(Pet pet)
  {
    CharacterSubject characterSubject = this;
    yield return (ICustomField) new CharacterFriendshipField(I18n.Pet_Love(), characterSubject.GameHelper.GetFriendshipForPet(Game1.player, pet));
    int? lastDayPetted = characterSubject.GetLastDayPetted(pet, Game1.player.UniqueMultiplayerID);
    string label = I18n.Pet_PettedToday();
    int? nullable1 = lastDayPetted;
    int totalDays1 = Game1.Date.TotalDays;
    string str = nullable1.GetValueOrDefault() == totalDays1 & nullable1.HasValue ? I18n.Pet_LastPetted_Yes() : characterSubject.Stringify((object) false);
    bool? hasValue = new bool?();
    yield return (ICustomField) new GenericField(label, str, hasValue);
    if (!lastDayPetted.HasValue)
    {
      yield return (ICustomField) new GenericField(I18n.Pet_LastPetted(), I18n.Pet_LastPetted_Never());
    }
    else
    {
      int? nullable2 = lastDayPetted;
      int totalDays2 = Game1.Date.TotalDays;
      if (!(nullable2.GetValueOrDefault() == totalDays2 & nullable2.HasValue))
      {
        int days = Game1.Date.TotalDays - lastDayPetted.Value;
        yield return (ICustomField) new GenericField(I18n.Pet_LastPetted(), days == 1 ? I18n.Generic_Yesterday() : I18n.Pet_LastPetted_DaysAgo((object) days));
      }
    }
    PetBowl petBowl = pet.GetPetBowl();
    if (petBowl != null)
      yield return (ICustomField) new GenericField(I18n.Pet_WaterBowl(), ((NetFieldBase<bool, NetBool>) petBowl.watered).Value ? I18n.Pet_WaterBowl_Filled() : I18n.Pet_WaterBowl_Empty());
  }

  private IEnumerable<ICustomField> GetDataForTrashBear(TrashBear trashBear)
  {
    CharacterSubject characterSubject = this;
    int questsDone = 0;
    if (NetWorldState.checkAnywhereForWorldStateID("trashBear1"))
      questsDone = 1;
    if (NetWorldState.checkAnywhereForWorldStateID("trashBear2"))
      questsDone = 2;
    if (NetWorldState.checkAnywhereForWorldStateID("trashBear3"))
      questsDone = 3;
    if (NetWorldState.checkAnywhereForWorldStateID("trashBearDone"))
      questsDone = 4;
    if (questsDone < 4)
    {
      trashBear.updateItemWanted();
      yield return (ICustomField) new ItemIconField(characterSubject.GameHelper, I18n.TrashBearOrGourmand_ItemWanted(), ItemRegistry.Create(trashBear.itemWantedIndex, 1, 0, false), characterSubject.Codex);
    }
    yield return (ICustomField) new GenericField(I18n.TrashBearOrGourmand_QuestProgress(), I18n.Generic_Ratio((object) questsDone, (object) 4));
  }

  private IEnumerable<ICustomField> GetDataForVillager(NPC npc)
  {
    CharacterSubject characterSubject = this;
    if (characterSubject.EnableTargetRedirection && npc != null && ((Character) npc).Name == "AbigailMine")
    {
      GameLocation currentLocation = ((Character) npc).currentLocation;
      if (currentLocation != null && currentLocation.Name == "UndergroundMine20")
        npc = Game1.getCharacterFromName("Abigail", true, false) ?? npc;
    }
    if (characterSubject.GameHelper.IsSocialVillager(npc))
    {
      SDate date;
      if (characterSubject.GameHelper.TryGetDate(npc.Birthday_Day, npc.Birthday_Season, out date))
        yield return (ICustomField) new GenericField(I18n.Npc_Birthday(), I18n.Stringify((object) date));
      if (((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData).ContainsKey(((Character) npc).Name))
      {
        FriendshipModel friendship = characterSubject.GameHelper.GetFriendshipForVillager(Game1.player, npc, ((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData)[((Character) npc).Name]);
        yield return (ICustomField) new GenericField(I18n.Npc_CanRomance(), friendship.IsSpouse ? I18n.Npc_CanRomance_Married() : (friendship.IsHousemate ? I18n.Npc_CanRomance_Housemate() : characterSubject.Stringify((object) friendship.CanDate)));
        yield return (ICustomField) new CharacterFriendshipField(I18n.Npc_Friendship(), friendship);
        yield return (ICustomField) new GenericField(I18n.Npc_TalkedToday(), characterSubject.Stringify((object) friendship.TalkedToday));
        yield return (ICustomField) new GenericField(I18n.Npc_GiftedToday(), characterSubject.Stringify((object) (friendship.GiftsToday > 0)));
        if (friendship.IsSpouse || friendship.IsHousemate)
          yield return (ICustomField) new GenericField(friendship.IsSpouse ? I18n.Npc_KissedToday() : I18n.Npc_HuggedToday(), characterSubject.Stringify((object) ((NetFieldBase<bool, NetBool>) npc.hasBeenKissedToday).Value));
        if ((object) friendship != null && !friendship.IsSpouse && !friendship.IsHousemate)
          yield return (ICustomField) new GenericField(I18n.Npc_GiftedThisWeek(), I18n.Generic_Ratio((object) friendship.GiftsThisWeek, (object) 2));
        friendship = (FriendshipModel) null;
      }
      else
        yield return (ICustomField) new GenericField(I18n.Npc_Friendship(), I18n.Npc_Friendship_NotMet());
      IDictionary<GiftTaste, GiftTasteModel[]> giftTastes = characterSubject.GetGiftTastes(npc);
      IDictionary<string, bool> ownedItems = CharacterGiftTastesField.GetOwnedItemsCache(characterSubject.GameHelper);
      if (characterSubject.ShowGiftTastes.Loved)
        yield return characterSubject.GetGiftTasteField(I18n.Npc_LovesGifts(), giftTastes, ownedItems, GiftTaste.Love);
      if (characterSubject.ShowGiftTastes.Liked)
        yield return characterSubject.GetGiftTasteField(I18n.Npc_LikesGifts(), giftTastes, ownedItems, GiftTaste.Like);
      if (characterSubject.ShowGiftTastes.Neutral)
        yield return characterSubject.GetGiftTasteField(I18n.Npc_NeutralGifts(), giftTastes, ownedItems, GiftTaste.Neutral);
      if (characterSubject.ShowGiftTastes.Disliked)
        yield return characterSubject.GetGiftTasteField(I18n.Npc_DislikesGifts(), giftTastes, ownedItems, GiftTaste.Dislike);
      if (characterSubject.ShowGiftTastes.Hated)
        yield return characterSubject.GetGiftTasteField(I18n.Npc_HatesGifts(), giftTastes, ownedItems, GiftTaste.Hate);
      giftTastes = (IDictionary<GiftTaste, GiftTasteModel[]>) null;
      ownedItems = (IDictionary<string, bool>) null;
      yield return (ICustomField) new ScheduleField(npc, characterSubject.GameHelper);
    }
  }

  private ICustomField GetGiftTasteField(
    string label,
    IDictionary<GiftTaste, GiftTasteModel[]> giftTastes,
    IDictionary<string, bool> ownedItemsCache,
    GiftTaste taste)
  {
    CharacterGiftTastesField giftTasteField = new CharacterGiftTastesField(label, giftTastes, taste, this.ShowUnknownGiftTastes, this.HighlightUnrevealedGiftTastes, !this.ShowUnownedGifts, ownedItemsCache);
    if (this.CollapseFieldsConfig.Enabled)
      giftTasteField.CollapseIfLengthExceeds(this.CollapseFieldsConfig.NpcGiftTastes, giftTasteField.TotalItems);
    return (ICustomField) giftTasteField;
  }

  private static string GetTypeName(Character npc, SubjectType type)
  {
    switch (type)
    {
      case SubjectType.Horse:
        return GameI18n.GetString("Strings\\StringsFromCSFiles:StrengthGame.cs.11665");
      case SubjectType.Monster:
        return I18n.Type_Monster();
      case SubjectType.Pet:
        PetData petData;
        if (npc is Pet pet && Pet.TryGetData(((NetFieldBase<string, NetString>) pet.petType).Value, ref petData))
        {
          string typeName = TokenParser.ParseText(petData.DisplayName, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? ((NetFieldBase<string, NetString>) pet.petType).Value;
          if (typeName.Length > 1)
            typeName = char.ToUpperInvariant(typeName[0]).ToString() + typeName.Substring(1);
          return typeName;
        }
        break;
      case SubjectType.Villager:
        return I18n.Type_Villager();
    }
    return npc.GetType().Name;
  }

  private static bool ShouldDisablePortraits(NPC npc, bool isGourmand)
  {
    bool flag = Game1.CurrentEvent?.id == "festival_fall27";
    if (flag)
    {
      string name = ((Character) npc).Name;
      flag = name == "Mummy" || name == "Stone Golem" || name == "Wilderness Golem";
    }
    if (flag)
      return true;
    if (isGourmand)
      return true;
    try
    {
      return npc.Portrait == null;
    }
    catch
    {
      return true;
    }
  }

  private IDictionary<GiftTaste, GiftTasteModel[]> GetGiftTastes(NPC npc)
  {
    return (IDictionary<GiftTaste, GiftTasteModel[]>) this.GameHelper.GetGiftTastes(npc).GroupBy<GiftTasteModel, GiftTaste>((Func<GiftTasteModel, GiftTaste>) (entry => entry.Taste)).ToDictionary<IGrouping<GiftTaste, GiftTasteModel>, GiftTaste, GiftTasteModel[]>((Func<IGrouping<GiftTaste, GiftTasteModel>, GiftTaste>) (tasteGroup => tasteGroup.Key), (Func<IGrouping<GiftTaste, GiftTasteModel>, GiftTasteModel[]>) (tasteGroup => tasteGroup.ToArray<GiftTasteModel>()));
  }

  private string GetChildBirthdayString(Child child)
  {
    int num = ((NetFieldBase<int, NetInt>) child.daysOld).Value;
    try
    {
      return SDate.Now().AddDays(-num).ToLocaleString(true);
    }
    catch (ArithmeticException ex)
    {
      return new SDate(Game1.dayOfMonth, Game1.season, 100000000).AddDays(-num).ToLocaleString(false);
    }
  }

  private int GetDaysToNextChildGrowth(ChildAge stage, int daysOld)
  {
    int toNextChildGrowth;
    switch (stage)
    {
      case ChildAge.Newborn:
        toNextChildGrowth = 13 - daysOld;
        break;
      case ChildAge.Baby:
        toNextChildGrowth = 27 - daysOld;
        break;
      case ChildAge.Crawler:
        toNextChildGrowth = 55 - daysOld;
        break;
      default:
        toNextChildGrowth = -1;
        break;
    }
    return toNextChildGrowth;
  }

  private int? GetLastDayPetted(Pet pet, long playerID)
  {
    int num;
    return !((NetDictionary<long, int, NetInt, SerializableDictionary<long, int>, NetLongDictionary<int, NetInt>>) pet.lastPetDay).TryGetValue(playerID, ref num) ? new int?() : new int?(num);
  }

  private IEnumerable<ItemDropData> GetMonsterDrops(Monster monster)
  {
    CharacterSubject characterSubject = this;
    ItemDropData[] source1 = characterSubject.GameHelper.GetMonsterData().FirstOrDefault<MonsterData>((Func<MonsterData, bool>) (p => p.Name == ((Character) monster).Name))?.Drops;
    if (characterSubject.IsHauntedSkull && source1 == null)
      source1 = characterSubject.GameHelper.GetMonsterData().FirstOrDefault<MonsterData>((Func<MonsterData, bool>) (p => p.Name == "Lava Bat"))?.Drops;
    if (source1 == null)
      source1 = Array.Empty<ItemDropData>();
    IDictionary<string, List<ItemDropData>> dropsLeft = (IDictionary<string, List<ItemDropData>>) ((IEnumerable<string>) monster.objectsToDrop).Select<string, ItemDropData>(new Func<string, ItemDropData>(characterSubject.GetActualDrop)).GroupBy<ItemDropData, string>((Func<ItemDropData, string>) (p => p.ItemId)).ToDictionary<IGrouping<string, ItemDropData>, string, List<ItemDropData>>((Func<IGrouping<string, ItemDropData>, string>) (group => group.Key), (Func<IGrouping<string, ItemDropData>, List<ItemDropData>>) (group => group.ToList<ItemDropData>()));
    IEnumerator<ItemDropData> enumerator1 = ((IEnumerable<ItemDropData>) source1).OrderByDescending<ItemDropData, float>((Func<ItemDropData, float>) (p => p.Probability)).GetEnumerator();
    while (enumerator1.MoveNext())
    {
      ItemDropData drop = enumerator1.Current;
      List<ItemDropData> source2;
      bool flag = dropsLeft.TryGetValue(drop.ItemId, out source2) && source2.Any<ItemDropData>();
      if (flag)
      {
        ItemDropData[] array = source2.Where<ItemDropData>((Func<ItemDropData, bool>) (p => p.MinDrop >= drop.MinDrop && p.MaxDrop <= drop.MaxDrop)).ToArray<ItemDropData>();
        ItemDropData itemDropData1 = ((IEnumerable<ItemDropData>) array).FirstOrDefault<ItemDropData>((Func<ItemDropData, bool>) (p => p.MinDrop == drop.MinDrop && p.MaxDrop == drop.MaxDrop));
        if ((object) itemDropData1 == null)
          itemDropData1 = ((IEnumerable<ItemDropData>) array).FirstOrDefault<ItemDropData>();
        ItemDropData itemDropData2 = itemDropData1;
        if ((object) itemDropData2 != null)
          source2.Remove(itemDropData2);
      }
      yield return new ItemDropData(drop.ItemId, 1, drop.MaxDrop, flag ? 1f : drop.Probability);
    }
    enumerator1 = (IEnumerator<ItemDropData>) null;
    foreach (KeyValuePair<string, List<ItemDropData>> keyValuePair in dropsLeft.Where<KeyValuePair<string, List<ItemDropData>>>((Func<KeyValuePair<string, List<ItemDropData>>, bool>) (p => p.Value.Any<ItemDropData>())))
    {
      List<ItemDropData>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator();
      while (enumerator2.MoveNext())
        yield return enumerator2.Current;
      enumerator2 = new List<ItemDropData>.Enumerator();
    }
  }

  private ItemDropData GetActualDrop(string id)
  {
    int MinDrop = 1;
    int MaxDrop = 1;
    int result;
    int num;
    if (int.TryParse(id, out result) && result < 0)
    {
      num = -result;
      id = num.ToString();
      MaxDrop = 3;
    }
    string str;
    if (id != null)
    {
      num = id.Length;
      switch (num)
      {
        case 1:
          switch (id[0])
          {
            case '0':
              num = 378;
              str = num.ToString();
              goto label_17;
            case '2':
              num = 380;
              str = num.ToString();
              goto label_17;
            case '4':
              num = 382;
              str = num.ToString();
              goto label_17;
            case '6':
              num = 384;
              str = num.ToString();
              goto label_17;
          }
          break;
        case 2:
          switch (id[1])
          {
            case '0':
              if (id == "10")
              {
                num = 386;
                str = num.ToString();
                goto label_17;
              }
              break;
            case '2':
              if (id == "12")
              {
                num = 388;
                str = num.ToString();
                goto label_17;
              }
              break;
            case '4':
              if (id == "14")
              {
                num = 390;
                str = num.ToString();
                goto label_17;
              }
              break;
          }
          break;
      }
    }
    str = id;
label_17:
    id = str;
    return new ItemDropData(id, MinDrop, MaxDrop, 1f);
  }
}
