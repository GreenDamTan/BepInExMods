using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HarmonyLib.Tools;
using OHD.Card;
using OHD.Dungeons.Player;
using OHD.GameActions;
using OHD.Players;
using System;

namespace Take_Me_To_The_Dungeon_Plugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    static ManualLogSource gLog = new ManualLogSource("glog");
    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        HarmonyFileLog.Enabled = true;
        BepInEx.Logging.Logger.Sources.Add(gLog);
        gLog.LogInfo("i am g glog");
        Harmony _pluginTriggers = Harmony.CreateAndPatchAll(
                typeof(Triggers)
            );
    }
    private class Triggers
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(OHD.Dungeons.Power.PowerBase), "get_Amount")]
        static public void get_Amount_Hook(ref OHD.Dungeons.Power.PowerBase __instance, ref int __result)
        {
            if (__result == 0) { return; }
            gLog.LogMessage("\n");
            //var stackInfo = new System.Diagnostics.StackTrace(true);
            //gLog.LogMessage(stackInfo.ToString());
            gLog.LogMessage(__instance._Owner_k__BackingField.ToString());
            gLog.LogMessage(__instance._PowerType_k__BackingField.ToString());
            //gLog.LogMessage(__instance._ActiveDescription_k__BackingField.ToString());
            gLog.LogMessage(__instance.ToString());
            gLog.LogMessage(__result.ToString());
            //foreach (var field in Traverse.Create(__instance).Properties())
            //{
            //    gLog.LogMessage(field.ToString());
            //}
            if (__instance._Owner_k__BackingField.ToString().Contains("BattleRoomPlayer"))
            {
                if (__instance._PowerType_k__BackingField.ToString().Contains("Debuff"))
                {
                    return;
                }
                __result = 999;
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(OHD.Card.CardBase), "get_CrystalCost")]
        static public void get_CrystalCost_Hook(ref int __result)
        {
            //gLog.LogInfo("get_CrystalCost "+__result);
            __result = 0;
        }
    }
}
