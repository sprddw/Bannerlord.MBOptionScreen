﻿using MCM.Abstractions.Attributes;
using MCM.Abstractions.ResourceInjection;
using MCM.UI.ResourceInjection.Loaders;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.ResourceInjection
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    internal class DefaultResourceLoader : IResourceLoader
    {
        public WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_v3" => PrefabsLoader.LoadModOptionsView(),
            "EditValueView_v3" => PrefabsLoader.LoadEditValueView(),
            "OptionsWithModOptionsView_v3" => PrefabsLoader.LoadOptionsWithModOptionsView(),
            _ => null
        };
    }
}