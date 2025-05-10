// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.LinkField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class LinkField : GenericField
{
  private readonly Func<ISubject?> Subject;

  public override bool MayHaveLinks => true;

  public LinkField(string label, string text, Func<ISubject?> subject)
    : base(label, (IFormattedText) new FormattedText(text, new Color?(Color.Blue)))
  {
    this.Subject = subject;
  }

  public override bool TryGetLinkAt(int x, int y, [NotNullWhen(true)] out ISubject? subject)
  {
    if (base.TryGetLinkAt(x, y, out subject))
      return true;
    subject = this.Subject();
    return subject != null;
  }
}
