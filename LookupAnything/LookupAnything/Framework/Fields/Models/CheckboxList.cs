// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.CheckboxList
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal class CheckboxList
{
  public Checkbox[] Checkboxes;
  public CheckboxList.IntroData? Intro;
  public bool IsHidden;

  public CheckboxList(Checkbox[] checkboxes, bool isHidden = false)
  {
    this.Checkboxes = checkboxes;
    this.IsHidden = isHidden;
  }

  public CheckboxList(IEnumerable<Checkbox> checkboxes, bool isHidden = false)
    : this(checkboxes.ToArray<Checkbox>(), isHidden)
  {
  }

  public CheckboxList AddIntro(string text, SpriteInfo? icon = null)
  {
    this.Intro = new CheckboxList.IntroData(text, icon);
    return this;
  }

  internal record IntroData(string Text, SpriteInfo? Icon);
}
