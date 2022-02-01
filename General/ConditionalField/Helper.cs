using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.General.ConditionalField
{
  public static class ReflectionHelper
  {
    public static MethodInfo GetMethod(object target, string methodName)
    {
      return ReflectionHelper.GetAllMethods(target, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
    }

    public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
    {
      return target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(predicate);
    }

    public static FieldInfo GetField(object target, string fieldName)
    {
      return ReflectionHelper.GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
    }

    public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
    {
      List<Type> types = new List<Type>() { target.GetType() };
      while (types.Last().BaseType != null)
      {
        types.Add(types.Last().BaseType);
      }

      for (int i = types.Count - 1; i >= 0; i--)
      {
        IEnumerable<FieldInfo> fieldInfos = types[i].GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(predicate);
        foreach (var fieldInfo in fieldInfos)
        {
          yield return fieldInfo;
        }
      }
    }
  }

  public static class ConditionChecker
  {
    public static bool Check(string attribute, SerializedProperty property)
    {
      var target = property.serializedObject.targetObject;

      // if the condition is a field
      FieldInfo conditionField = ReflectionHelper.GetField(target, attribute);
      if (conditionField != null && conditionField.FieldType == typeof(bool))
      {
        return (bool)conditionField.GetValue(target);
      }

      // if the condition is a method
      MethodInfo conditionMethod = ReflectionHelper.GetMethod(target, attribute);
      if (conditionMethod != null && conditionMethod.ReturnType == typeof(bool) && conditionMethod.GetParameters().Length == 0)
      {
        return (bool)conditionMethod.Invoke(target, null);
      }

      Debug.LogError("Invalid condition!");
      return true;
    }
  }
}
