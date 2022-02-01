using System;
using UnityEngine;
using DT.General.ConditionalField;

namespace DT.General
{
  [AttributeUsage(AttributeTargets.Field)]
  public class EnableIfAttribute : PropertyAttribute
  {
    public string condition { get; private set; }

    public EnableIfAttribute(string fieldOrMethod)
    {
      this.condition = fieldOrMethod;
    }
  }
}
