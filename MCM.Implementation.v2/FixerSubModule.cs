﻿using HarmonyLib;

using TaleWorlds.MountAndBlade;

namespace MCM.Implementation.v2
{
    // Do not provide assembly substitutes
    public sealed class FixerSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            var harmony = new Harmony("bannerlord.mcm.v2.loaderpreventer");
            foreach (var method in SettingsDatabasePatch1.TargetMethods())
            {
                harmony.Patch(
                    original: method,
                    prefix: new HarmonyMethod(typeof(SettingsDatabasePatch1), nameof(SettingsDatabasePatch1.Prefix)));
            }
            foreach (var method in MBOptionScreenSubModulePatch1.TargetMethods())
            {
                harmony.Patch(
                    original: method,
                    prefix: new HarmonyMethod(typeof(MBOptionScreenSubModulePatch1), nameof(MBOptionScreenSubModulePatch1.Prefix)));
            }
            foreach (var method in MBOptionScreenSubModulePatch2.TargetMethods())
            {
                harmony.Patch(
                    original: method,
                    prefix: new HarmonyMethod(typeof(MBOptionScreenSubModulePatch2), nameof(MBOptionScreenSubModulePatch2.Prefix)));
            }
        }
    }
}