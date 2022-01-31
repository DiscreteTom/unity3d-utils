namespace DT.General
{
  public static class LayerExtension
  {
    public static bool Contains(this LayerMask mask, int target)
    {
      return (mask.value & (1 << target)) != 0;
    }
  }
}

