using UnityEditor;
using UnityEngine;

namespace DT.General.ConditionalField
{
  [CustomPropertyDrawer(typeof(ShowIfAttribute))]
  public class ShowIfAttributeDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var attr = (ShowIfAttribute)this.attribute;
      if (ConditionChecker.Check(attr.condition, property))
      {
        if (attr.enableCondition == "" || ConditionChecker.Check(attr.enableCondition, property))
          EditorGUI.PropertyField(position, property, label, true);
        else
        {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUI.PropertyField(position, property, label, true);
          EditorGUI.EndDisabledGroup();
        }
      }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (ConditionChecker.Check(((ShowIfAttribute)this.attribute).condition, property))
        return base.GetPropertyHeight(property, label);
      return 0;
    }
  }
}
