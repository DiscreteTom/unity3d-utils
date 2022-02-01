using UnityEditor;
using UnityEngine;

namespace DT.General.ConditionalField
{
  [CustomPropertyDrawer(typeof(ShowIfAttribute))]
  public class ShowIfAttributeDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (ConditionChecker.Check(this.attribute as ShowIfAttribute, property))
      {
        EditorGUI.PropertyField(position, property, label, true);
      }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (ConditionChecker.Check(this.attribute as ShowIfAttribute, property))
        return base.GetPropertyHeight(property, label);
      return 0;
    }
  }
}
