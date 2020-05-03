﻿using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MCM.Implementation.v2
{
    public sealed class MBOptionScreenSubModulePatch1
    {
        public static IEnumerable<MethodBase> TargetMethods() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(assembly => Path.GetFileNameWithoutExtension(assembly.Location).StartsWith("MBOptionScreen.v2"))
            .Select(assembly => assembly.GetType("MBOptionScreen.MBOptionScreenSubModule") ??
                                assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2010") ??
                                assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2011"))
            .Where(type => type != null)
            .Select(type => AccessTools.Method(type, "OnSubModuleLoad"))
            .Where(method => method != null);

        public static bool Prefix() => false;
    }

    public sealed class MBOptionScreenSubModulePatch2
    {
        public static IEnumerable<MethodBase> TargetMethods() => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(assembly => Path.GetFileNameWithoutExtension(assembly.Location).StartsWith("MBOptionScreen.v2"))
            .Select(assembly => assembly.GetType("MBOptionScreen.MBOptionScreenSubModule") ??
                                assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2010") ??
                                assembly.GetType("MBOptionScreen.MBOptionScreenSubModuleV2011"))
            .Where(type => type != null)
            .Select(type => AccessTools.Method(type, "OnBeforeInitialModuleScreenSetAsRoot"))
            .Where(method => method != null);

        public static bool Prefix() => false;
    }
}