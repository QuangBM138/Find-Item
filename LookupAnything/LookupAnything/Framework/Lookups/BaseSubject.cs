// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.BaseSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups;

internal abstract class BaseSubject : ISubject
{
  protected GameHelper GameHelper { get; }

  protected Metadata Metadata => this.GameHelper.Metadata;

  protected ConstantData Constants => this.Metadata.Constants;

  public string Name { get; protected set; }

  public string? Description { get; protected set; }

  public string? Type { get; protected set; }

  public abstract IEnumerable<ICustomField> GetData();

  public abstract IEnumerable<IDebugField> GetDebugFields();

  public abstract bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size);

  protected BaseSubject(GameHelper gameHelper)
  {
    this.GameHelper = gameHelper;
    this.Name = string.Empty;
  }

  protected BaseSubject(GameHelper gameHelper, string name, string? description, string? type)
    : this(gameHelper)
  {
    this.Initialize(name, description, type);
  }

  [MemberNotNull("Name")]
  protected void Initialize(string name, string? description, string? type)
  {
    this.Name = name;
    this.Description = description;
    this.Type = type;
  }

  protected IEnumerable<IDebugField> GetDebugFieldsFrom(object? obj)
  {
    if (obj != null)
    {
      Dictionary<string, string> seenValues = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      System.Type type;
      for (type = obj.GetType(); type != (System.Type) null; type = type.BaseType)
      {
        foreach (var data in ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (field => !field.IsLiteral && !field.Name.EndsWith(">k__BackingField"))).Select(field => new
        {
          Name = field.Name,
          Type = field.FieldType,
          Value = this.GetDebugValue(obj, field),
          IsProperty = false
        }).Concat(((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (property => property.CanRead)).Select(property => new
        {
          Name = property.Name,
          Type = property.PropertyType,
          Value = this.GetDebugValue(obj, property),
          IsProperty = true
        })).OrderBy(field => field.Name, (IComparer<string>) StringComparer.OrdinalIgnoreCase).ThenByDescending(field => field.IsProperty))
        {
          string str;
          if ((!seenValues.TryGetValue(data.Name, out str) || !(str == data.Value)) && (!(data.Name == "modDataForSerialization") || !seenValues.ContainsKey("modData")) && !(data.Value == data.Type.ToString()))
          {
            seenValues[data.Name] = data.Value;
            yield return (IDebugField) new GenericDebugField($"{type.Name}::{data.Name}", data.Value);
          }
        }
      }
      type = (System.Type) null;
    }
  }

  protected string Stringify(object? value) => I18n.Stringify(value);

  protected string GetRelativeDateStr(SDate date)
  {
    return this.GetRelativeDateStr(date.DaysSinceStart - SDate.Now().DaysSinceStart);
  }

  protected string GetRelativeDateStr(int days)
  {
    switch (days - -1)
    {
      case 0:
        return I18n.Generic_Yesterday();
      case 1:
        return I18n.Generic_Now();
      case 2:
        return I18n.Generic_Tomorrow();
      default:
        return days <= 0 ? I18n.Generic_XDaysAgo((object) -days) : I18n.Generic_InXDays((object) days);
    }
  }

  private string GetDebugValue(object obj, FieldInfo field)
  {
    try
    {
      return this.Stringify(field.GetValue(obj));
    }
    catch (Exception ex)
    {
      return "error reading field: " + ex.Message;
    }
  }

  private string GetDebugValue(object obj, PropertyInfo property)
  {
    try
    {
      return this.Stringify(property.GetValue(obj));
    }
    catch (Exception ex)
    {
      return "error reading property: " + ex.Message;
    }
  }
}
