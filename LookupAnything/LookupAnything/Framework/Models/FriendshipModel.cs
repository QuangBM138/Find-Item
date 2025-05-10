// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.FriendshipModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using StardewValley;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models;

internal record FriendshipModel
{
  public bool CanDate { get; }

  public bool IsDating { get; }

  public bool IsSpouse { get; }

  public bool IsHousemate { get; }

  public bool IsDivorced { get; }

  public bool HasStardrop { get; }

  public bool TalkedToday { get; }

  public int GiftsToday { get; }

  public int GiftsThisWeek { get; }

  public FriendshipStatus Status { get; }

  public int Points { get; }

  public int? StardropPoints { get; }

  public int MaxPoints { get; }

  public int PointsPerLevel { get; }

  public int FilledHearts { get; }

  public int EmptyHearts { get; }

  public int LockedHearts { get; }

  public int TotalHearts => this.FilledHearts + this.EmptyHearts + this.LockedHearts;

  public FriendshipModel(Farmer player, NPC npc, Friendship friendship, ConstantData constants)
  {
    bool flag1 = friendship.IsMarried();
    bool flag2 = friendship.IsRoommate();
    this.CanDate = ((NetFieldBase<bool, NetBool>) npc.datable).Value;
    this.IsDating = friendship.IsDating();
    this.IsSpouse = flag1 && !flag2;
    this.IsHousemate = flag1 & flag2;
    this.IsDivorced = friendship.IsDivorced();
    this.Status = friendship.Status;
    this.TalkedToday = friendship.TalkedToToday;
    this.GiftsToday = friendship.GiftsToday;
    this.GiftsThisWeek = friendship.GiftsThisWeek;
    this.MaxPoints = this.IsSpouse || this.IsHousemate ? constants.SpouseMaxFriendship : 2500;
    this.Points = friendship.Points;
    this.PointsPerLevel = 250;
    this.FilledHearts = this.Points / 250;
    this.LockedHearts = !this.CanDate || this.IsDating ? 0 : constants.DatingHearts;
    this.EmptyHearts = this.MaxPoints / 250 - this.FilledHearts - this.LockedHearts;
    if (!this.IsSpouse && !this.IsHousemate)
      return;
    this.StardropPoints = new int?(constants.SpouseFriendshipForStardrop);
    this.HasStardrop = !((NetHashSet<string>) player.mailReceived).Contains("CF_Spouse");
  }

  public FriendshipModel(int points, int pointsPerLevel, int maxPoints)
  {
    this.Points = points;
    this.PointsPerLevel = pointsPerLevel;
    this.MaxPoints = maxPoints;
    this.FilledHearts = this.Points / pointsPerLevel;
    this.EmptyHearts = this.MaxPoints / pointsPerLevel - this.FilledHearts;
  }

  public int GetPointsToNext()
  {
    if (this.Points < this.MaxPoints)
      return this.PointsPerLevel - this.Points % this.PointsPerLevel;
    if (this.StardropPoints.HasValue)
    {
      int points = this.Points;
      int? stardropPoints = this.StardropPoints;
      int valueOrDefault = stardropPoints.GetValueOrDefault();
      if (points < valueOrDefault & stardropPoints.HasValue)
      {
        stardropPoints = this.StardropPoints;
        return stardropPoints.Value - this.Points;
      }
    }
    return 0;
  }

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("CanDate = ");
    builder.Append(this.CanDate.ToString());
    builder.Append(", IsDating = ");
    builder.Append(this.IsDating.ToString());
    builder.Append(", IsSpouse = ");
    builder.Append(this.IsSpouse.ToString());
    builder.Append(", IsHousemate = ");
    builder.Append(this.IsHousemate.ToString());
    builder.Append(", IsDivorced = ");
    builder.Append(this.IsDivorced.ToString());
    builder.Append(", HasStardrop = ");
    builder.Append(this.HasStardrop.ToString());
    builder.Append(", TalkedToday = ");
    builder.Append(this.TalkedToday.ToString());
    builder.Append(", GiftsToday = ");
    builder.Append(this.GiftsToday.ToString());
    builder.Append(", GiftsThisWeek = ");
    builder.Append(this.GiftsThisWeek.ToString());
    builder.Append(", Status = ");
    builder.Append(this.Status.ToString());
    builder.Append(", Points = ");
    builder.Append(this.Points.ToString());
    builder.Append(", StardropPoints = ");
    builder.Append(this.StardropPoints.ToString());
    builder.Append(", MaxPoints = ");
    builder.Append(this.MaxPoints.ToString());
    builder.Append(", PointsPerLevel = ");
    builder.Append(this.PointsPerLevel.ToString());
    builder.Append(", FilledHearts = ");
    builder.Append(this.FilledHearts.ToString());
    builder.Append(", EmptyHearts = ");
    builder.Append(this.EmptyHearts.ToString());
    builder.Append(", LockedHearts = ");
    builder.Append(this.LockedHearts.ToString());
    builder.Append(", TotalHearts = ");
    builder.Append(this.TotalHearts.ToString());
    return true;
  }

  [CompilerGenerated]
  protected FriendshipModel(FriendshipModel original)
  {
    this.CanDate = original.CanDate;
    this.IsDating = original.IsDating;
    this.IsSpouse = original.IsSpouse;
    this.IsHousemate = original.IsHousemate;
    this.IsDivorced = original.IsDivorced;
    this.HasStardrop = original.HasStardrop;
    this.TalkedToday = original.TalkedToday;
    this.GiftsToday = original.GiftsToday;
    this.GiftsThisWeek = original.GiftsThisWeek;
    this.Status = original.Status;
    this.Points = original.Points;
    this.StardropPoints = original.StardropPoints;
    this.MaxPoints = original.MaxPoints;
    this.PointsPerLevel = original.PointsPerLevel;
    this.FilledHearts = original.FilledHearts;
    this.EmptyHearts = original.EmptyHearts;
    this.LockedHearts = original.LockedHearts;
  }
}
