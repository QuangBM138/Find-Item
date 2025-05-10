// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ScheduleField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.Common;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ScheduleField(NPC npc, GameHelper gameHelper) : GenericField(I18n.Npc_Schedule(), ScheduleField.GetText(npc, gameHelper))
{
  private static IEnumerable<IFormattedText> GetText(NPC npc, GameHelper gameHelper)
  {
    bool isFarmhand = !Context.IsMainPlayer;
    GameLocation currentLocation = ((Character) npc).currentLocation;
    if (isFarmhand)
    {
      bool? nullable = currentLocation?.IsActiveLocation();
      if (!nullable.HasValue || !nullable.GetValueOrDefault())
      {
        yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_Farmhand_UnknownPosition());
        goto label_4;
      }
    }
    yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_CurrentPosition(currentLocation != null ? (object) gameHelper.GetLocationDisplayName(currentLocation.Name, currentLocation.GetData()) : (object) "???", (object) ((Character) npc).TilePoint.X, (object) ((Character) npc).TilePoint.Y));
label_4:
    yield return (IFormattedText) new FormattedText(Environment.NewLine + Environment.NewLine);
    if (isFarmhand)
    {
      yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_Farmhand_UnknownSchedule());
    }
    else
    {
      ScheduleField.ScheduleEntry[] schedule = ScheduleField.FormatSchedule(npc.Schedule).ToArray<ScheduleField.ScheduleEntry>();
      if (schedule.Length == 0)
        yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_NoEntries());
      else if (npc.ignoreScheduleToday || !npc.followSchedule)
      {
        yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_NotFollowingSchedule());
      }
      else
      {
        int Time;
        for (int i = 0; i < schedule.Length; Time = i++)
        {
          SchedulePathDescription Description;
          (Time, Description) = schedule[i];
          int time = Time;
          SchedulePathDescription entry = Description;
          string locationName = gameHelper.GetLocationDisplayName(entry.targetLocationName, Game1.getLocationFromName(entry.targetLocationName)?.GetData());
          bool flag1 = Game1.timeOfDay >= time;
          bool flag2 = i < schedule.Length - 1 && Game1.timeOfDay >= schedule[i + 1].Time;
          Color textColor = flag1 ? (flag2 ? Color.Gray : Color.Green) : Color.Black;
          if (i > 0)
            yield return (IFormattedText) new FormattedText(Environment.NewLine);
          yield return (IFormattedText) new FormattedText(I18n.Npc_Schedule_Entry((object) CommonHelper.FormatTime(time), (object) locationName, (object) entry.targetTile.X, (object) entry.targetTile.Y), new Color?(textColor));
          entry = (SchedulePathDescription) null;
          locationName = (string) null;
        }
        schedule = (ScheduleField.ScheduleEntry[]) null;
      }
    }
  }

  private static IEnumerable<ScheduleField.ScheduleEntry> FormatSchedule(
    Dictionary<int, SchedulePathDescription>? schedule)
  {
    if (schedule != null)
    {
      List<int> list = schedule.Keys.OrderBy<int, int>((Func<int, int>) (key => key)).ToList<int>();
      string prevTargetLocationName = string.Empty;
      foreach (int num in list)
      {
        SchedulePathDescription Description;
        if (schedule.TryGetValue(num, out Description) && !(Description.targetLocationName == prevTargetLocationName))
        {
          prevTargetLocationName = Description.targetLocationName;
          yield return new ScheduleField.ScheduleEntry(num, Description);
        }
      }
    }
  }

  private record ScheduleEntry(int Time, SchedulePathDescription Description);
}
