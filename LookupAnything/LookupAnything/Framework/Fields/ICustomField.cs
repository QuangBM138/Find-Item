// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ICustomField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal interface ICustomField
{
  string Label { get; }

  IFormattedText[]? Value { get; }

  bool HasValue { get; }

  bool MayHaveLinks { get; }

  LinkField? ExpandLink { get; }

  Vector2? DrawValue(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, float wrapWidth);

  bool TryGetLinkAt(int x, int y, [NotNullWhen(true)] out ISubject? subject);
}
