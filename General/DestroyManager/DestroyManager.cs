using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace DT.General
{
  public class DestroyManager : MonoBehaviour
  {
    public bool destroyOnInvisible = false;
    public bool destroyOnTime = false;
    [ShowIf("destroyOnTime")]
    [SerializeField] float TTL = 5;
    [SerializeField] UnityEvent onDestroy;

    void Update()
    {
      if (this.destroyOnTime)
      {
        this.TTL -= Time.deltaTime;
        if (this.TTL <= 0) Destroy(this.gameObject);
      }
    }

    void OnDestroy()
    {
      this.onDestroy?.Invoke();
    }

    public void DestroyGameObject()
    {
      Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
      if (this.destroyOnInvisible)
      {
        this.DestroyGameObject();
      }
    }
  }

}
