using UnityEngine;

namespace DT.General
{
  public static class LayerMaskExtension
  {
    public static bool Contains(this LayerMask mask, int target)
    {
      return (mask.value & (1 << target)) != 0;
    }
  }
}

