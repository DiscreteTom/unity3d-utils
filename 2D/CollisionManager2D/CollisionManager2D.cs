using UnityEngine;
using UnityEngine.Events;
using DT.General;

namespace DT.CollisionManager2D
{
  [System.Serializable]
  struct CollisionHandler2D
  {
    public LayerMask layers;
    public UnityEvent<Collision2D> events;
  }

  [System.Serializable]
  struct TriggerHandler2D
  {
    public LayerMask layers;
    public UnityEvent<Collider2D> events;
  }

  public class CollisionManager2D : MonoBehaviour
  {
    [Header("Collision")]
    [SerializeField] CollisionHandler2D[] onCollisionEnter;
    [SerializeField] CollisionHandler2D[] onCollisionStay;
    [SerializeField] CollisionHandler2D[] onCollisionExit;

    [Header("Trigger")]
    [SerializeField] TriggerHandler2D[] onTriggerEnter;
    [SerializeField] TriggerHandler2D[] onTriggerStay;
    [SerializeField] TriggerHandler2D[] onTriggerExit;

    void OnCollisionEnter2D(Collision2D other)
    {
      foreach (var handler in this.onCollisionEnter)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }
    void OnCollisionStay2D(Collision2D other)
    {
      foreach (var handler in this.onCollisionStay)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }
    void OnCollisionExit2D(Collision2D other)
    {
      foreach (var handler in this.onCollisionExit)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      foreach (var handler in this.onTriggerEnter)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }

    void OnTriggerExit2D(Collider2D other)
    {
      foreach (var handler in this.onTriggerExit)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }

    void OnTriggerStay2D(Collider2D other)
    {
      foreach (var handler in this.onTriggerStay)
      {
        if (handler.layers.Contains(other.gameObject.layer))
        {
          handler.events.Invoke(other);
        }
      }
    }
  }
}
