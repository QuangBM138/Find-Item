// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.ModDataDictionaryExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using StardewValley;
using StardewValley.Mods;
using StardewValley.Network;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class ModDataDictionaryExtensions
{
  [return: NotNullIfNotNull("defaultValue")]
  public static T? ReadField<T>(
    this ModDataDictionary data,
    string key,
    Func<string, T> parse,
    T? defaultValue = null)
  {
    string str;
    if (((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) data).TryGetValue(key, ref str))
    {
      try
      {
        return parse(str);
      }
      catch
      {
      }
    }
    return defaultValue;
  }

  [return: NotNullIfNotNull("defaultValue")]
  public static string? ReadField(this ModDataDictionary data, string key, string? defaultValue = null)
  {
    string str;
    return !((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) data).TryGetValue(key, ref str) ? defaultValue : str;
  }

  public static ModDataDictionary WriteField(this ModDataDictionary data, string key, string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      ((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) data).Remove(key);
    else
      ((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) data)[key] = value;
    return data;
  }
}
