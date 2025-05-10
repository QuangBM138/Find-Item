// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Constants.Constant
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Constants;

internal static class Constant
{
  public static readonly int MaxStackSizeForPricing = 999;
  public static readonly Vector2 MaxTargetSpriteSize = new Vector2(3f, 5f);
  public static readonly Vector2 MaxBuildingTargetSpriteSize = new Vector2(10f, 10f);

  public static bool AllowBold => Game1.content.GetCurrentLanguage() != 3;

  public static class MailLetters
  {
    public const string ReceivedSpouseStardrop = "CF_Spouse";
    public const string JojaMember = "JojaMember";
  }

  public static class SeasonNames
  {
    public const string Spring = "spring";
    public const string Summer = "summer";
    public const string Fall = "fall";
    public const string Winter = "winter";
  }

  public static class ItemNames
  {
    public static string Heater = nameof (Heater);
  }

  public static class BuildingNames
  {
    public static string GoldClock = "Gold Clock";
  }

  public static class ObjectIndexes
  {
    public static int AutoGrabber = 165;
  }
}
