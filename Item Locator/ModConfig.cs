// Decompiled with JetBrains decompiler
// Type: ModConfig
// Assembly: Item Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BFE121E-49FA-41A1-80AC-34270D5A3C38
// Assembly location: D:\game indi\Item Locator\Item Locator.dll

using StardewModdingAPI;
using System.Collections.Generic;

#nullable enable
public sealed class ModConfig
{
  public SButton openMenuKey { get; set; }

  public List<string> locateHistory { get; set; }

  public float pathTransparency { get; set; }

  public ModConfig()
  {
    this.openMenuKey = (SButton) 79;
    this.locateHistory = new List<string>()
    {
      "None",
      "None",
      "None",
      "None",
      "None"
    };
    this.pathTransparency = 0.15f;
  }
}
