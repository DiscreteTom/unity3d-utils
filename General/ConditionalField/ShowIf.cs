using System;
using UnityEngine;
using DT.General.ConditionalField;

namespace DT.General
{
  [AttributeUsage(AttributeTargets.Field)]
  public class ShowIfAttribute : PropertyAttribute
  {
    public string condition { get; private set; }

    public ShowIfAttribute(string fieldOrMethod)
    {
      this.condition = fieldOrMethod;
    }
  }
}
