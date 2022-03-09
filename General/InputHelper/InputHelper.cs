using System.Collections.Generic;
using UnityEngine;

namespace DT.General
{
  public class InputHelper
  {
    static public bool GetAnyButton(IEnumerable<string> names)
    {
      foreach (var btn in names)
        if (Input.GetButton(btn)) return true;
      return false;
    }

    static public bool GetAnyButtonDown(IEnumerable<string> names)
    {
      foreach (var btn in names)
        if (Input.GetButtonDown(btn)) return true;
      return false;
    }

    static public bool GetAnyButtonUp(IEnumerable<string> names)
    {
      foreach (var btn in names)
        if (Input.GetButtonUp(btn)) return true;
      return false;
    }

    static public float GetAnyAxis(IEnumerable<string> names)
    {
      foreach (var btn in names)
      {
        var value = Input.GetAxis(btn);
        if (value != 0) return value;
      }
      return 0;
    }

    static public float GetAnyAxisRaw(IEnumerable<string> names)
    {
      foreach (var btn in names)
      {
        var value = Input.GetAxisRaw(btn);
        if (value != 0) return value;
      }
      return 0;
    }
  }
}
