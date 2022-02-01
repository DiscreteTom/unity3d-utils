using UnityEngine;
using UnityEngine.Events;

public class DestroyManager : MonoBehaviour
{
  [SerializeField]
  private UnityEvent onDestroy;

  void OnDestroy()
  {
    this.onDestroy?.Invoke();
  }

  public void DestroyGameObject()
  {
    Destroy(this.gameObject);
  }
}
