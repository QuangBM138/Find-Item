// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.CharacterLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.Common;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class CharacterLookupProvider : BaseLookupProvider
{
  private readonly Func<ModConfig> Config;
  private readonly ISubjectRegistry Codex;

  public CharacterLookupProvider(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    Func<ModConfig> config,
    ISubjectRegistry codex)
    : base(reflection, gameHelper)
  {
    this.Config = config;
    this.Codex = codex;
  }

  public override IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    CharacterLookupProvider characterLookupProvider1 = this;
    if (location is IslandFarmCave islandFarmCave && islandFarmCave.gourmand != null)
    {
      CharacterLookupProvider characterLookupProvider = characterLookupProvider1;
      NPC gourmand = islandFarmCave.gourmand;
      yield return (ITarget) new CharacterTarget(characterLookupProvider1.GameHelper, characterLookupProvider1.GetSubjectType(gourmand), gourmand, ((Character) gourmand).Tile, (Func<ISubject>) (() => characterLookupProvider.BuildSubject(gourmand)));
    }
    foreach (NPC npc1 in (IEnumerable<NPC>) ((object) Game1.CurrentEvent?.actors ?? (object) location.characters))
    {
      CharacterLookupProvider characterLookupProvider = characterLookupProvider1;
      NPC npc = npc1;
      Vector2 tile = ((Character) npc).Tile;
      if (characterLookupProvider1.GameHelper.CouldSpriteOccludeTile(tile, lookupTile))
        yield return (ITarget) new CharacterTarget(characterLookupProvider1.GameHelper, characterLookupProvider1.GetSubjectType(npc), npc, tile, (Func<ISubject>) (() => characterLookupProvider.BuildSubject(npc)));
    }
    foreach (FarmAnimal farmAnimal in ((NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>) location.Animals).Values)
    {
      CharacterLookupProvider characterLookupProvider = characterLookupProvider1;
      FarmAnimal animal = farmAnimal;
      Vector2 tile = ((Character) animal).Tile;
      if (characterLookupProvider1.GameHelper.CouldSpriteOccludeTile(tile, lookupTile))
        yield return (ITarget) new FarmAnimalTarget(characterLookupProvider1.GameHelper, animal, tile, (Func<ISubject>) (() => characterLookupProvider.BuildSubject(animal)));
    }
    foreach (Farmer farmer1 in location.farmers)
    {
      CharacterLookupProvider characterLookupProvider = characterLookupProvider1;
      Farmer farmer = farmer1;
      Vector2 tile = ((Character) farmer).Tile;
      if (characterLookupProvider1.GameHelper.CouldSpriteOccludeTile(tile, lookupTile))
        yield return (ITarget) new FarmerTarget(characterLookupProvider1.GameHelper, farmer, (Func<ISubject>) (() => characterLookupProvider.BuildSubject(farmer)));
    }
  }

  public override ISubject? GetSubject(IClickableMenu menu, int cursorX, int cursorY)
  {
    IClickableMenu iclickableMenu = this.GameHelper.GetGameMenuPage(menu) ?? menu;
    switch (iclickableMenu)
    {
      case SkillsPage _:
        return this.BuildSubject(Game1.player);
      case ProfileMenu profileMenu:
        if (profileMenu.hoveredItem == null)
        {
          Character character = profileMenu.Current.Character;
          if (character != null)
            return this.Codex.GetByEntity((object) character, character.currentLocation);
          goto case null;
        }
        goto case null;
      case SocialPage socialPage:
        using (List<ClickableTextureComponent>.Enumerator enumerator = socialPage.characterSlots.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ClickableTextureComponent current = enumerator.Current;
            if (((ClickableComponent) current).containsPoint(cursorX, cursorY))
            {
              Character character = socialPage.SocialEntries[((ClickableComponent) current).myID].Character;
              if (character is Farmer player)
                return this.BuildSubject(player);
              if (character is NPC npc)
                return this.BuildSubject(npc);
            }
          }
          goto case null;
        }
      case Billboard billboard:
        if (billboard.calendarDays != null)
        {
          int selectedDay = -1;
          for (int index = 0; index < billboard.calendarDays.Count; ++index)
          {
            if (((ClickableComponent) billboard.calendarDays[index]).containsPoint(cursorX, cursorY))
            {
              selectedDay = index + 1;
              break;
            }
          }
          if (selectedDay == -1)
            return (ISubject) null;
          NPC npc = this.GameHelper.GetAllCharacters().Where<NPC>((Func<NPC, bool>) (p => p.Birthday_Season == Game1.currentSeason && p.Birthday_Day == selectedDay)).MaxBy<NPC, bool>((Func<NPC, bool>) (p => p.CanSocialize));
          if (npc != null)
            return this.BuildSubject(npc);
          goto case null;
        }
        goto default;
      case TitleMenu _:
        if (TitleMenu.subMenu is LoadGameMenu subMenu)
        {
          ClickableComponent clickableComponent = subMenu.slotButtons.FirstOrDefault<ClickableComponent>((Func<ClickableComponent, bool>) (p => p.containsPoint(cursorX, cursorY)));
          if (clickableComponent != null)
          {
            int index = subMenu.currentItemIndex + int.Parse(clickableComponent.name);
            if ((subMenu.MenuSlots[index] is LoadGameMenu.SaveFileSlot menuSlot ? menuSlot.Farmer : (Farmer) null) != null)
              return (ISubject) new FarmerSubject(this.GameHelper, menuSlot.Farmer, true);
            goto case null;
          }
          goto case null;
        }
        goto default;
      case null:
        return (ISubject) null;
      default:
        if (iclickableMenu.GetType().FullName == "AnimalSocialMenu.Framework.AnimalSocialPage")
        {
          int num1 = this.Reflection.GetField<int>((object) iclickableMenu, "SlotPosition", true).GetValue();
          List<ClickableTextureComponent> textureComponentList = this.Reflection.GetField<List<ClickableTextureComponent>>((object) iclickableMenu, "Sprites", true).GetValue();
          List<object> list = this.Reflection.GetField<List<object>>((object) iclickableMenu, "Names", true).GetValue();
          for (int index = num1; index < textureComponentList.Count; ++index)
          {
            if (((ClickableComponent) textureComponentList[index]).containsPoint(cursorX, cursorY))
            {
              object obj;
              if (list.TryGetIndex<object>(index, out obj) && obj is long num2)
              {
                long id = num2;
                FarmAnimal animal = ((GameLocation) Game1.getFarm()).getAllFarmAnimals().FirstOrDefault<FarmAnimal>((Func<FarmAnimal, bool>) (p => ((NetFieldBase<long, NetLong>) p.myID).Value == id));
                if (animal != null)
                  return this.BuildSubject(animal);
                break;
              }
              break;
            }
          }
          goto case null;
        }
        NPC npc1 = this.Reflection.GetField<NPC>((object) iclickableMenu, "hoveredNpc", false)?.GetValue() ?? this.Reflection.GetField<NPC>((object) iclickableMenu, "HoveredNpc", false)?.GetValue();
        if (npc1 != null)
          return this.BuildSubject(npc1);
        goto case null;
    }
  }

  public override ISubject? GetSubjectFor(object entity, GameLocation? location)
  {
    ISubject subjectFor;
    switch (entity)
    {
      case FarmAnimal animal:
        subjectFor = this.BuildSubject(animal);
        break;
      case Farmer player:
        subjectFor = this.BuildSubject(player);
        break;
      case NPC npc:
        subjectFor = this.BuildSubject(npc);
        break;
      default:
        subjectFor = (ISubject) null;
        break;
    }
    return subjectFor;
  }

  public override IEnumerable<ISubject> GetSearchSubjects()
  {
    HashSet<string> seen = new HashSet<string>();
    foreach (ISubject searchSubject in GetAll())
    {
      if (seen.Add($"{searchSubject.GetType().FullName}::{searchSubject.Type}::{searchSubject.Name}"))
        yield return searchSubject;
    }

    IEnumerable<ISubject> GetAll()
    {
      List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator();
      while (enumerator.MoveNext())
        yield return this.BuildSubject(enumerator.Current);
      enumerator = new List<NPC>.Enumerator();
      foreach (GameLocation location in CommonHelper.GetLocations())
      {
        foreach (FarmAnimal animal in ((NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>) location.Animals).Values)
          yield return this.BuildSubject(animal);
      }
      foreach (Farmer allFarmer in Game1.getAllFarmers())
        yield return this.BuildSubject(allFarmer);
    }
  }

  private ISubject BuildSubject(Farmer player)
  {
    return (ISubject) new FarmerSubject(this.GameHelper, player);
  }

  private ISubject BuildSubject(FarmAnimal animal)
  {
    return (ISubject) new FarmAnimalSubject(this.Codex, this.GameHelper, animal);
  }

  private ISubject BuildSubject(NPC npc)
  {
    ModConfig modConfig = this.Config();
    return (ISubject) new CharacterSubject(this.Codex, this.GameHelper, npc, this.GetSubjectType(npc), this.GameHelper.Metadata, modConfig.ShowUnknownGiftTastes, modConfig.HighlightUnrevealedGiftTastes, modConfig.ShowGiftTastes, modConfig.CollapseLargeFields, modConfig.EnableTargetRedirection, modConfig.ShowUnownedGifts);
  }

  private SubjectType GetSubjectType(NPC npc)
  {
    SubjectType subjectType;
    switch (npc)
    {
      case Horse _:
        subjectType = SubjectType.Horse;
        break;
      case Junimo _:
        subjectType = SubjectType.Junimo;
        break;
      case Pet _:
        subjectType = SubjectType.Pet;
        break;
      case Monster _:
        subjectType = SubjectType.Monster;
        break;
      default:
        subjectType = SubjectType.Villager;
        break;
    }
    return subjectType;
  }
}
