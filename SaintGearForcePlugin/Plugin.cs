using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaintGearForcePlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        static ManualLogSource gLog = new ManualLogSource("glog");
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            HarmonyFileLog.Enabled = true;
            BepInEx.Logging.Logger.Sources.Add(gLog);
            gLog.LogInfo("i am g glog");
            Harmony _pluginTriggers = Harmony.CreateAndPatchAll(
                typeof(Triggers)
            );
        }
        private class Triggers
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(PlayerStatusManeger), nameof(PlayerStatusManeger.Damage_HP))]
            static IEnumerable<CodeInstruction> Damage_HP(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                gLog.LogInfo("codes[63].opcode " + codes[63].opcode);
                codes[63].opcode = System.Reflection.Emit.OpCodes.Ret;
                return codes.AsEnumerable();
            }
            [HarmonyPostfix]
            [HarmonyPatch(typeof(PlayerGrowUpSystem), "GetStatusGrowUpData")]
            static public void GetStatusGrowUpData_Hook(ref StatusGrowUpData __result)
            {
                __result.consume_sp = 1;
            }
            [HarmonyPostfix]
            [HarmonyPatch(typeof(PlayerGrowUpSystem), "GetGrowUpSkillData")]
            static public void GetGrowUpSkillData_Hook(ref GrowUpSkillData __result)
            {
                __result.amount = 1;
            }
            [HarmonyPrefix]
            [HarmonyPatch(typeof(PlayerStatusManeger), "Consume_EP")]
            [HarmonyPatch(typeof(PlayerStatusManeger), "Consume_SP")]
            static public void Consume_SP_EP(ref int consume_amount)
            {
                gLog.LogInfo("consume_amount" + consume_amount);
                consume_amount = 1;
            }
            //[HarmonyPrefix]
            //[HarmonyPatch(typeof(PlayerStatusManeger), nameof(PlayerStatusManeger.Damage_HP))]
            //static public void Damage_HP_Hook(ref int damage_amount, ref bool acme)
            //{
            //    damage_amount = 1;
            //}
        }
    }
}
