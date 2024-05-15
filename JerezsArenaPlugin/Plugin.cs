using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HarmonyLib.Tools;
using System;

namespace JerezsArenaPlugin;

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
        [HarmonyPatch(typeof(GameDataManager), "CheckAP")]
        static public void CheckAP_Hook(ref int iGirl,ref int iApCost, ref PlayingErrCode __result)
        {
            if(__result == PlayingErrCode.NOTHING)
            {
                return;
            }
            gLog.LogInfo(String.Format("CheckAP {0},{1},{2}", iGirl, iApCost, __result));
            switch (__result)
            {
                case PlayingErrCode.LACK_AP:
                    {
                        __result = PlayingErrCode.NOTHING;
                        break;
                    }
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameDataManager), "CheckSP")]
        static public void CheckSP_Hook(ref int iGirl,ref float iSpCost, ref PlayingErrCode __result)
        {
            if (__result == PlayingErrCode.NOTHING)
            {
                return;
            }
            gLog.LogInfo(String.Format("CheckHP {0},{1},{2}", iGirl, iSpCost, __result));
            if (__result != PlayingErrCode.NOTHING)
            {
                __result = PlayingErrCode.NOTHING;
            }

        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameDataManager), "CheckHP")]
        static public void CheckHP_Hook(ref int iGirl, ref float iHpCost, ref PlayingErrCode __result)
        {
            if (__result == PlayingErrCode.NOTHING)
            {
                return;
            }
            gLog.LogInfo(String.Format("CheckHP {0},{1},{2}", iGirl, iHpCost, __result));
            if (__result != PlayingErrCode.NOTHING)
            {
                __result = PlayingErrCode.NOTHING;
            }

        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameDataManager), "CheckGold")]
        static public void CheckGold_Hook(ref int iGoldCost, ref PlayingErrCode __result)
        {
            //if (__result == PlayingErrCode.NOTHING)
            //{
            //    return;
            //}
            gLog.LogInfo(String.Format("CheckGold {0},{1}", iGoldCost, __result));
            if (__result != PlayingErrCode.NOTHING)
            {
                __result = PlayingErrCode.NOTHING;
            }

        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameDataManager), "PayGold")]
        static public void GameDataManager_PayGold_Hook(ref int iG, ref PlayingErrCode __result)
        {
            gLog.LogInfo(String.Format("GameDataManager.PayGold {0}},{1}", iG, __result));
            iG = -Math.Abs(iG);
            if (__result != PlayingErrCode.NOTHING)
            {
                __result = PlayingErrCode.NOTHING;
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerDataRunTime), "PayGold")]
        static public void PlayerDataRunTime_PayGold_Hook(ref int iGold,ref PlayerDataRunTime __instance, ref PlayingErrCode __result)
        {
            gLog.LogInfo(String.Format("PlayerDataRunTime.PayGold {0},{1}", iGold, __result));
            iGold = -Math.Abs(iGold);
            if (__result != PlayingErrCode.NOTHING)
            {
                __result = PlayingErrCode.NOTHING;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerDataRunTime), "AddGold")]
        static public void PlayerDataRunTime_AddGold_Hook(ref int iGold, ref PlayerDataRunTime __instance)
        {
            gLog.LogInfo(String.Format("PlayerDataRunTime.AddGold {0}", iGold));
            if (iGold < 0)
            {
                iGold = Math.Abs(iGold);
            }
        }
    }
}
