// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.ObjectExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class ObjectExtensions
{
  public static T AssertNotNull<T>(this T? value, string? paramName = null) where T : class
  {
    return (object) value != null ? value : throw new ArgumentNullException(paramName);
  }
}
