// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ISubjectRegistry
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewValley;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal interface ISubjectRegistry
{
  ISubject? GetByEntity(object entity, GameLocation? location);
}
