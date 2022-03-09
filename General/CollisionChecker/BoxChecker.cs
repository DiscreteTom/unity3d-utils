using NaughtyAttributes;
using UnityEngine;

namespace DT.General {
  public class BoxChecker : BaseCollisionChecker {
    public bool _3D;
    public Bounds bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(1, 1, 0));
    public bool showBounds = false;
    [ShowIf("showBounds")]
    public Color boundsColor = Color.red;
    public LayerMask checkLayers;

    public override bool Check() {
      if (this._3D) {
        return Physics.CheckBox(this.bounds.center + this.transform.position, this.bounds.extents, this.transform.rotation, this.checkLayers.value);
      } else {
        return Physics2D.OverlapBox(this.bounds.center + this.transform.position, this.bounds.size, 0, this.checkLayers.value);
      }
    }
  }
}

