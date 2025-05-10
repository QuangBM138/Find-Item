// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Data.ItemData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Data;

internal record ItemData()
{
  public ObjectContext Context { get; set; }

  public HashSet<string> QualifiedId { get; set; }

  public string? NameKey { get; set; }

  public string? DescriptionKey { get; set; }

  public string? TypeKey { get; set; }

  public bool? ShowInventoryFields { get; set; }

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("Context = ");
    builder.Append(this.Context.ToString());
    builder.Append(", QualifiedId = ");
    builder.Append((object) this.QualifiedId);
    builder.Append(", NameKey = ");
    builder.Append((object) this.NameKey);
    builder.Append(", DescriptionKey = ");
    builder.Append((object) this.DescriptionKey);
    builder.Append(", TypeKey = ");
    builder.Append((object) this.TypeKey);
    builder.Append(", ShowInventoryFields = ");
    builder.Append(this.ShowInventoryFields.ToString());
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ObjectContext>.Default.GetHashCode(this.\u003CContext\u003Ek__BackingField)) * -1521134295 + EqualityComparer<HashSet<string>>.Default.GetHashCode(this.\u003CQualifiedId\u003Ek__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CNameKey\u003Ek__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CDescriptionKey\u003Ek__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.\u003CTypeKey\u003Ek__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.\u003CShowInventoryFields\u003Ek__BackingField);
  }

  [CompilerGenerated]
  public virtual bool Equals(ItemData? other)
  {
    if ((object) this == (object) other)
      return true;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ObjectContext>.Default.Equals(this.\u003CContext\u003Ek__BackingField, other.\u003CContext\u003Ek__BackingField) && EqualityComparer<HashSet<string>>.Default.Equals(this.\u003CQualifiedId\u003Ek__BackingField, other.\u003CQualifiedId\u003Ek__BackingField) && EqualityComparer<string>.Default.Equals(this.\u003CNameKey\u003Ek__BackingField, other.\u003CNameKey\u003Ek__BackingField) && EqualityComparer<string>.Default.Equals(this.\u003CDescriptionKey\u003Ek__BackingField, other.\u003CDescriptionKey\u003Ek__BackingField) && EqualityComparer<string>.Default.Equals(this.\u003CTypeKey\u003Ek__BackingField, other.\u003CTypeKey\u003Ek__BackingField) && EqualityComparer<bool?>.Default.Equals(this.\u003CShowInventoryFields\u003Ek__BackingField, other.\u003CShowInventoryFields\u003Ek__BackingField);
  }

  [CompilerGenerated]
  protected ItemData(ItemData original)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CContext\u003Ek__BackingField = original.\u003CContext\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CQualifiedId\u003Ek__BackingField = original.\u003CQualifiedId\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CNameKey\u003Ek__BackingField = original.\u003CNameKey\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CDescriptionKey\u003Ek__BackingField = original.\u003CDescriptionKey\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CTypeKey\u003Ek__BackingField = original.\u003CTypeKey\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CShowInventoryFields\u003Ek__BackingField = original.\u003CShowInventoryFields\u003Ek__BackingField;
  }
}
