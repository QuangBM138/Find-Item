// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.IInvariantSet
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal interface IInvariantSet : 
  IReadOnlySet<string>,
  IEnumerable<string>,
  IEnumerable,
  IReadOnlyCollection<string>
{
  IInvariantSet GetWith(string other);

  IInvariantSet GetWith(ICollection<string> other);

  IInvariantSet GetWithout(string other);

  IInvariantSet GetWithout(IEnumerable<string> other);
}
