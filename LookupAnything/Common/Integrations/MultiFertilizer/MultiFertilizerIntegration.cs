// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.MultiFertilizer.MultiFertilizerIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.MultiFertilizer;

internal class MultiFertilizerIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration("MultiFertilizer", "spacechase0.MultiFertilizer", "1.0.2", modRegistry, monitor)
{
  public IEnumerable<string> GetAppliedFertilizers(HoeDirt dirt)
  {
    if (this.IsLoaded)
    {
      if (CommonHelper.IsItemId(((NetFieldBase<string, NetString>) dirt.fertilizer).Value, false))
        yield return ((NetFieldBase<string, NetString>) dirt.fertilizer).Value;
      string[] strArray = new string[3]
      {
        "FertilizerLevel",
        "SpeedGrowLevel",
        "WaterRetainLevel"
      };
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str1 = strArray[index];
        string s;
        int result;
        if (((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) ((TerrainFeature) dirt).modData).TryGetValue("spacechase0.MultiFertilizer/" + str1, ref s) && int.TryParse(s, out result))
        {
          string str2 = $"{str1}:{result}";
          string str3;
          if (str2 != null)
          {
            switch (str2.Length)
            {
              case 16 /*0x10*/:
                switch (str2[15])
                {
                  case '1':
                    if (str2 == "SpeedGrowLevel:1")
                    {
                      str3 = "(O)465";
                      goto label_30;
                    }
                    break;
                  case '2':
                    if (str2 == "SpeedGrowLevel:2")
                    {
                      str3 = "(O)466";
                      goto label_30;
                    }
                    break;
                  case '3':
                    if (str2 == "SpeedGrowLevel:3")
                    {
                      str3 = "(O)918";
                      goto label_30;
                    }
                    break;
                }
                break;
              case 17:
                switch (str2[16 /*0x10*/])
                {
                  case '1':
                    if (str2 == "FertilizerLevel:1")
                    {
                      str3 = "(O)368";
                      goto label_30;
                    }
                    break;
                  case '2':
                    if (str2 == "FertilizerLevel:2")
                    {
                      str3 = "(O)369";
                      goto label_30;
                    }
                    break;
                  case '3':
                    if (str2 == "FertilizerLevel:3")
                    {
                      str3 = "(O)919";
                      goto label_30;
                    }
                    break;
                }
                break;
              case 18:
                switch (str2[17])
                {
                  case '1':
                    if (str2 == "WaterRetainLevel:1")
                    {
                      str3 = "(O)370";
                      goto label_30;
                    }
                    break;
                  case '2':
                    if (str2 == "WaterRetainLevel:2")
                    {
                      str3 = "(O)371";
                      goto label_30;
                    }
                    break;
                  case '3':
                    if (str2 == "WaterRetainLevel:3")
                    {
                      str3 = "(O)920";
                      goto label_30;
                    }
                    break;
                }
                break;
            }
          }
          str3 = (string) null;
label_30:
          string appliedFertilizer = str3;
          if (appliedFertilizer != null)
            yield return appliedFertilizer;
        }
      }
      strArray = (string[]) null;
    }
  }
}
