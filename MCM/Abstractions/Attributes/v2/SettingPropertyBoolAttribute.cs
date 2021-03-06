﻿using MCM.Abstractions.Settings.Definitions;

using System;

namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyBoolAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionBool
    {
        public SettingPropertyBoolAttribute(string displayName) : base(displayName)
        {

        }
    }
}