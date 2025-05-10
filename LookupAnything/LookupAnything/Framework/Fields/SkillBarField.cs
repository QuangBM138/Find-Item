// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.SkillBarField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class SkillBarField : PercentageBarField
{
  private readonly int[] SkillPointsPerLevel;

  public SkillBarField(
    string label,
    int experience,
    int maxSkillPoints,
    int[] skillPointsPerLevel)
    : base(label, experience, maxSkillPoints, Color.Green, Color.Gray, (string) null)
  {
    this.SkillPointsPerLevel = skillPointsPerLevel;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    int[] skillPointsPerLevel = this.SkillPointsPerLevel;
    int num1 = ((IEnumerable<int>) skillPointsPerLevel).FirstOrDefault<int>((Func<int, bool>) (p => p - this.CurrentValue > 0));
    int expNeeded = num1 > 0 ? num1 - this.CurrentValue : 0;
    int level = num1 > 0 ? Array.IndexOf<int>(skillPointsPerLevel, num1) : skillPointsPerLevel.Length;
    string text = expNeeded > 0 ? I18n.Player_Skill_Progress((object) level, (object) expNeeded) : I18n.Player_Skill_ProgressLast((object) level);
    float num2 = 0.0f;
    int val1 = 0;
    for (int index = 0; index < skillPointsPerLevel.Length; ++index)
    {
      float ratio = index >= level ? (index <= level ? Math.Min(1f, (float) this.CurrentValue / ((float) skillPointsPerLevel[index] * 1f)) : 0.0f) : 1f;
      Vector2 vector2 = this.DrawBar(spriteBatch, Vector2.op_Addition(position, new Vector2(num2, 0.0f)), ratio, this.FilledColor, this.EmptyColor, 25f);
      val1 = (int) vector2.Y;
      num2 += vector2.X + 2f;
    }
    Vector2 vector2_1 = spriteBatch.DrawTextBlock(font, text, Vector2.op_Addition(position, new Vector2(num2, 0.0f)), wrapWidth - num2);
    return new Vector2?(new Vector2(num2 + vector2_1.X, Math.Max((float) val1, vector2_1.Y)));
  }
}
