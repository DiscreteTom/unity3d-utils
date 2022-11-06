using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Events;

namespace DT.General {
  public interface IWatchable {
    void AddListener(UnityAction f);
    void RemoveListener(UnityAction f);
  }

  /// <summary>
  /// Watch a **value** type for changes.
  /// This class should be used for immutable types (int, float, bool, string, etc) only.
  /// </summary>
  public class Watch<T> : IWatchable {
    protected T value;
    UnityEvent onChange0;
    UnityEvent<T> onChange1;
    UnityEvent<T, T> onChange2;

    public Watch(T value) {
      this.value = value;
      this.onChange0 = new UnityEvent();
      this.onChange1 = new UnityEvent<T>();
      this.onChange2 = new UnityEvent<T, T>();
    }

    public T Value {
      get => this.value;
      set {
        var previous = this.value;
        this.value = value;
        this.onChange0.Invoke();
        this.onChange1.Invoke(value);
        this.onChange2.Invoke(value, previous);
      }
    }

    /// <summary>
    /// Add a listener that will be called when the value changes.
    /// </summary>
    public void AddListener(UnityAction f) => this.onChange0.AddListener(f);
    public void RemoveListener(UnityAction f) => this.onChange0.RemoveListener(f);
    /// <summary>
    /// Add a listener that will be called when the value changes.
    /// The parameter is the new value.
    /// </summary>
    public void AddListener(UnityAction<T> f) => this.onChange1.AddListener(f);
    public void RemoveListener(UnityAction<T> f) => this.onChange1.RemoveListener(f);
    /// <summary>
    /// Add a listener that will be called when the value changes.
    /// The parameter is the new value and the previous value.
    /// </summary>
    public void AddListener(UnityAction<T, T> f) => this.onChange2.AddListener(f);
    public void RemoveListener(UnityAction<T, T> f) => this.onChange2.RemoveListener(f);
  }

  /// <summary>
  /// Watch a **reference** type for changes.
  /// </summary>
  public class WatchRef<T> : IWatchable {
    protected T value;
    UnityEvent onChange0;
    UnityEvent<WatchRef<T>> onChange;

    public WatchRef(T value) {
      this.value = value;
      this.onChange0 = new UnityEvent();
      this.onChange = new UnityEvent<WatchRef<T>>();
    }

    /// <summary>
    /// Set the value and trigger the onChange event.
    /// </summary>
    public void SetValue(T value) {
      this.value = value;
      this.InvokeEvent();
    }

    /// <summary>
    /// Apply a function to the value and trigger the onChange event.
    /// </summary>
    public void Apply(UnityAction<T> f) {
      f(this.value);
      this.InvokeEvent();
    }

    /// <summary>
    /// Apply a function to the value and trigger the onChange event.
    /// </summary>
    public R Apply<R>(Func<T, R> f) {
      var result = f(this.value);
      this.InvokeEvent();
      return result;
    }

    /// <summary>
    /// Apply a function to the value without trigger the onChange event.
    /// </summary>
    public void ReadOnlyApply(UnityAction<T> f) {
      f(this.value);
    }

    /// <summary>
    /// Apply a function to the value without trigger the onChange event.
    /// </summary>
    public R ReadOnlyApply<R>(Func<T, R> f) {
      return f(this.value);
    }

    /// <summary>
    /// Add a listener that will be called when the value changes.
    /// </summary>
    public void AddListener(UnityAction<WatchRef<T>> f) => this.onChange.AddListener(f);
    public void RemoveListener(UnityAction<WatchRef<T>> f) => this.onChange.RemoveListener(f);
    /// <summary>
    /// Add a listener that will be called when the value changes.
    /// </summary>
    public void AddListener(UnityAction f) => this.onChange0.AddListener(f);
    public void RemoveListener(UnityAction f) => this.onChange0.RemoveListener(f);

    /// <summary>
    /// Invoke all events.
    /// </summary>
    void InvokeEvent() {
      this.onChange0.Invoke();
      this.onChange.Invoke(this);
    }
  }

  /// <summary>
  /// Immediately calculate a value when a watchable changes.
  /// The result should be immutable.
  /// </summary>
  public class Computed<T> : Watch<T> {
    Func<T> compute;

    public new T Value {
      get => this.value;
    }

    public Computed(Func<T> compute) : base(compute()) {
      this.compute = compute;
    }

    public Computed<T> Watch(IWatchable target) {
      target.AddListener(this.Update);
      return this;
    }

    void Update() {
      this.value = this.compute();
    }
  }

  /// <summary>
  /// Auto calculate a value when the value is used after a watchable changes.
  /// The result should be immutable.
  /// </summary>
  public class LazyComputed<T> {
    T value;
    bool needUpdate = true;
    Func<T> compute { get; set; }

    public T Value {
      get {
        if (this.needUpdate) {
          this.value = this.compute();
          this.needUpdate = false;
        }
        return this.value;
      }
    }

    public LazyComputed(Func<T> compute) {
      this.compute = compute;
      this.needUpdate = true;
    }

    public LazyComputed<T> Watch(IWatchable target) {
      target.AddListener(this.Update);
      return this;
    }

    void Update() {
      this.needUpdate = true;
    }
  }

  /// <summary>
  /// Watch a list-like type for changes.
  /// </summary>
  public class WatchIList<L, T> : WatchRef<L> where L : IList<T> {
    LazyComputed<ReadOnlyCollection<T>> readOnlyList;

    public WatchIList(L value) : base(value) {
      this.readOnlyList = new LazyComputed<ReadOnlyCollection<T>>(() => new ReadOnlyCollection<T>(this.value)).Watch(this);
    }

    /// <summary>
    /// Get the list as a read-only list and cache it for future calls.
    /// </summary>
    public ReadOnlyCollection<T> Value => this.readOnlyList.Value;
  }
  /// <summary>
  /// Watch a dictionary-like type for changes.
  /// </summary>
  public class WatchIDictionary<D, K, V> : WatchRef<D> where D : IDictionary<K, V> {
    LazyComputed<ReadOnlyDictionary<K, V>> readOnlyDictionary;

    public WatchIDictionary(D value) : base(value) {
      this.readOnlyDictionary = new LazyComputed<ReadOnlyDictionary<K, V>>(() => new ReadOnlyDictionary<K, V>(this.value)).Watch(this);
    }

    /// <summary>
    /// Get the dictionary as a read-only dictionary and cache it for future calls.
    /// </summary>
    public ReadOnlyDictionary<K, V> Value => this.readOnlyDictionary.Value;
  }

  /// <summary>
  /// Watch a list for changes.
  /// </summary>
  public class WatchList<T> : WatchIList<List<T>, T> {
    public WatchList() : base(new List<T>()) { }
    public WatchList(List<T> value) : base(value) { }
  }
  /// <summary>
  /// Watch an array for changes.
  /// </summary>
  public class WatchArray<T> : WatchIList<T[], T> {
    public WatchArray(int n) : base(new T[n]) { }
    public WatchArray(T[] value) : base(value) { }
  }
  /// <summary>
  /// Watch a dictionary for changes.
  /// </summary>
  public class WatchDictionary<K, V> : WatchIDictionary<Dictionary<K, V>, K, V> {
    public WatchDictionary() : base(new Dictionary<K, V>()) { }
    public WatchDictionary(Dictionary<K, V> value) : base(value) { }
  }
}