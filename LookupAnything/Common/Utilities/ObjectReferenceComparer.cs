// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.ObjectReferenceComparer`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class ObjectReferenceComparer<T> : IEqualityComparer<T>
{
  public bool Equals(T? x, T? y) => (object) x == (object) y;

  public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode((object) obj);
}
