using UnityEngine.Events;

namespace DT.General {
  public class Watch<T> {
    T value;
    UnityEvent<T, T> onChange;

    public Watch(T value) {
      this.value = value;
      this.onChange = new UnityEvent<T, T>();
    }

    public T Value {
      get => this.value;
      set {
        var old = this.value;
        this.value = value;
        this.onChange.Invoke(value, old);
      }
    }

    public void AddListener(UnityAction<T, T> f) => this.onChange.AddListener(f);
    public void RemoveListener(UnityAction<T, T> f) => this.onChange.RemoveListener(f);
  }
}