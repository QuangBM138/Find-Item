// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ItemIconField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewValley;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ItemIconField : GenericField
{
  private readonly SpriteInfo? Sprite;
  private readonly ISubject? LinkSubject;

  public override bool MayHaveLinks => this.LinkSubject != null || base.MayHaveLinks;

  public ItemIconField(
    GameHelper gameHelper,
    string label,
    Item? item,
    ISubjectRegistry? codex,
    string? text = null)
    : base(label, item != null)
  {
    this.Sprite = gameHelper.GetSprite(item);
    if (item == null)
      return;
    this.LinkSubject = codex?.GetByEntity((object) item, (GameLocation) null);
    text = !string.IsNullOrWhiteSpace(text) ? text : item.DisplayName;
    Color? color = this.LinkSubject != null ? new Color?(Color.Blue) : new Color?();
    this.Value = new IFormattedText[1]
    {
      (IFormattedText) new FormattedText(text, color)
    };
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    float y = font.MeasureString("ABC").Y;
    Vector2 size;
    // ISSUE: explicit constructor call
    ((Vector2) ref size).\u002Ector(y);
    spriteBatch.DrawSpriteWithin(this.Sprite, position.X, position.Y, size);
    Vector2 vector2 = spriteBatch.DrawTextBlock(font, (IEnumerable<IFormattedText>) this.Value, Vector2.op_Addition(position, new Vector2(size.X + 5f, 5f)), wrapWidth);
    return new Vector2?(new Vector2(wrapWidth, vector2.Y + 5f));
  }

  public override bool TryGetLinkAt(int x, int y, [NotNullWhen(true)] out ISubject? subject)
  {
    if (base.TryGetLinkAt(x, y, out subject))
      return true;
    subject = this.LinkSubject;
    return subject != null;
  }
}
