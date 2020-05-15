﻿using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.ModLib.Settings.SettingsContainer
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IModLibDefinitionsSettingsContainer : IGlobalSettingsContainer
    {

    }
}