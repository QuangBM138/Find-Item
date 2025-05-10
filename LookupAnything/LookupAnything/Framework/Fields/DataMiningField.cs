// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.DataMiningField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class DataMiningField : GenericField
{
  public DataMiningField(string label, IEnumerable<IDebugField>? fields)
    : base(label)
  {
    IDebugField[] debugFieldArray = (fields != null ? fields.ToArray<IDebugField>() : (IDebugField[]) null) ?? Array.Empty<IDebugField>();
    this.HasValue = ((IEnumerable<IDebugField>) debugFieldArray).Any<IDebugField>();
    if (!this.HasValue)
      return;
    this.Value = this.GetFormattedText(debugFieldArray).ToArray<IFormattedText>();
  }

  private IEnumerable<IFormattedText> GetFormattedText(IDebugField[] fields)
  {
    int i = 0;
    for (int last = fields.Length - 1; i <= last; ++i)
    {
      IDebugField field = fields[i];
      yield return (IFormattedText) new FormattedText("*", new Color?(Color.Red), true);
      yield return (IFormattedText) new FormattedText(field.Label + ":");
      yield return (IFormattedText) (i != last ? new FormattedText(field.Value + Environment.NewLine) : new FormattedText(field.Value));
      field = (IDebugField) null;
    }
  }
}
