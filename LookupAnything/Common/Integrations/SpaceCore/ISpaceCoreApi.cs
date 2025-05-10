// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.SpaceCore.ISpaceCoreApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.SpaceCore;

public interface ISpaceCoreApi
{
  string[] GetCustomSkills();

  int GetExperienceForCustomSkill(Farmer farmer, string skill);

  string GetDisplayNameOfCustomSkill(string skill);
}
