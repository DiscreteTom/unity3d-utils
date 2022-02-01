using UnityEditor;
using UnityEngine;

namespace DT.General.ConditionalField
{
  [CustomPropertyDrawer(typeof(EnableIfAttribute))]
  public class EnableIfAttributeDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (ConditionChecker.Check(((EnableIfAttribute)this.attribute).condition, property))
      {
        EditorGUI.PropertyField(position, property, label, true);
      }
      else
      {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
      }
    }
  }
}
