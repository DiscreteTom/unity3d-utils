using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DT.General.CollisionChecker {
  [CustomEditor(typeof(BoxChecker)), CanEditMultipleObjects]
  public class BoxCheckerEditor : Editor {
    BoxBoundsHandle handle = new BoxBoundsHandle();

    void OnSceneGUI() {
      BoxChecker checker = this.target as BoxChecker;

      if (checker.showBounds) {
        // copy the target object's data to the handle
        handle.center = checker.bounds.center + checker.transform.position;
        handle.size = checker.bounds.size;

        // draw the handle
        EditorGUI.BeginChangeCheck();
        handle.SetColor(checker.boundsColor);
        handle.DrawHandle();
        if (EditorGUI.EndChangeCheck()) {
          // record the target object before setting new values so changes can be undone/redone
          Undo.RecordObject(checker, "Change Bounds");

          // copy the handle's updated data back to the target object
          checker.bounds = new Bounds(handle.center - checker.transform.position, handle.size);
        }
      }
    }
  }
}
