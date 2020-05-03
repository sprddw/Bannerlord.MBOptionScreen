﻿using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1.Wrapper;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace MCM.Utils
{
    public static class SettingsUtils
    {
        internal static IEnumerable<SettingsPropertyDefinition> GetProperties(object @object, string id)
        {
            foreach (var propertyDefinition in GetPropertiesInternal(@object, id))
            {
                CheckIsValid(propertyDefinition, @object);
                yield return propertyDefinition;
            }
        }

        private static IEnumerable<SettingsPropertyDefinition> GetPropertiesInternal(object @object, string id)
        {
            foreach (var property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = property.GetCustomAttributes();

                object groupAttrObj = attributes.FirstOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyGroupAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyGroupAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyGroupDefinition)));
                var groupDefinition = groupAttrObj != null
                    ? (IPropertyGroupDefinition)new PropertyGroupDefinitionWrapper(groupAttrObj)
                    : (IPropertyGroupDefinition)SettingPropertyGroupAttribute.Default;

                object propAttr;
                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "ModLib.Attributes.SettingPropertyAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.SettingPropertyAttribute")
                    ?? attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v1.SettingPropertyAttribute");
                if (propAttr != null)
                {
                    yield return new SettingsPropertyDefinition(
                        new SettingPropertyAttributeWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyBoolAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionBool)));
                if (propAttr != null)
                {
                    yield return new SettingsPropertyDefinition(
                        new PropertyDefinitionBoolWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyFloatingIntegerAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionFloatingInteger)));
                if (propAttr != null)
                {
                    yield return new SettingsPropertyDefinition(
                        new PropertyDefinitionFloatingIntegerWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyIntegerAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionInteger)));
                if (propAttr != null)
                {
                    yield return new SettingsPropertyDefinition(
                        new PropertyDefinitionIntegerWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyTextAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionText)));
                if (propAttr != null)
                {
                    yield return new SettingsPropertyDefinition(
                        new PropertyDefinitionTextWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }

                propAttr = attributes.SingleOrDefault(a => a.GetType().FullName == "MBOptionScreen.Attributes.v2.SettingPropertyDropdownAttribute")
                    ?? attributes.SingleOrDefault(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyDefinitionDropdown)));
                if (propAttr != null)
                {

                    yield return new SettingsPropertyDefinition(
                        new PropertyDefinitionDropdownWrapper(propAttr),
                        groupDefinition,
                        new ProxyPropertyInfo(property),
                        id);
                    continue;
                }
            }
        }

        private static void CheckIsValid(SettingsPropertyDefinition prop, object settings)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} must have a getter.");
            if (prop.SettingType != SettingType.Dropdown && !prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} must have a setter.");

            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
                if (prop.IsMainToggle)
                    throw new Exception($"Property {prop.Property.Name} in {settings.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
            }
        }

        public static object? UnwrapSettings(object? settings)
        {
            while (settings != null && ReflectionUtils.ImplementsOrImplementsEquivalent(settings.GetType(), typeof(SettingsWrapper)))
            {
                var prop = AccessTools.Field(settings.GetType(), "_object") ??
                           AccessTools.Field(settings.GetType(), nameof(SettingsWrapper.Object));
                settings = prop?.GetValue(settings);
            }
            return settings;
        }
    }
}