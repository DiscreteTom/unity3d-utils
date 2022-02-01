using UnityEngine;

namespace DT.General
{
  public static class LayerMaskExtension
  {
    // ref: http://answers.unity.com/answers/1332280/view.html
    public static bool Contains(this LayerMask mask, int target)
    {
      return (mask.value & (1 << target)) != 0;
    }
  }
}

