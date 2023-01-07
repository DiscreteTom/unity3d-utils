using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace DT.General {
  public struct AddListenerHelper<ActionType> where ActionType : class {
    List<Func<bool>> conditions;
    TSmartEvent<ActionType> smartEvent;

    internal AddListenerHelper(TSmartEvent<ActionType> smartEvent) {
      this.conditions = new List<Func<bool>>();
      this.smartEvent = smartEvent;
    }

    /// <summary>
    /// Remove the listener from the event when the condition is not met.
    /// </summary>
    public AddListenerHelper<ActionType> When(Func<bool> condition) {
      this.conditions.Add(condition);
      return this;
    }

    /// <summary>
    /// Remove the listener from the event if obj is null.
    /// </summary>
    public AddListenerHelper<ActionType> Require(UnityEngine.Object obj) {
      return this.When(() => obj != null);
    }

    public void AddListener(ActionType action) {
      this.smartEvent.AddListener(action, this.conditions.ToArray());
    }
  }

  /// <summary>
  /// Template of smart event.
  /// </summary>
  public class TSmartEvent<ActionType> where ActionType : class {
    protected class Listener {
      public ActionType action;
      public Func<bool>[] conditions { get; set; }
    }
    protected List<Listener> listeners;

    public TSmartEvent() {
      this.listeners = new List<Listener>();
    }

    /// <summary>
    /// Remove the listener from the event when the condition is not met.
    /// </summary>
    public AddListenerHelper<ActionType> Require(UnityEngine.Object obj) {
      return new AddListenerHelper<ActionType>(this).Require(obj);
    }

    /// <summary>
    /// Remove the listener from the event if obj is null.
    /// </summary>
    public AddListenerHelper<ActionType> When(Func<bool> condition) {
      return new AddListenerHelper<ActionType>(this).When(condition);
    }

    public void AddListener(ActionType action, params Func<bool>[] conditions) {
      var l = new Listener();
      l.action = action;
      l.conditions = conditions;
      this.listeners.Add(l);
    }

    public void RemoveListener(ActionType action) {
      this.listeners.RemoveAll((l) => l.action == action);
    }

    protected void InvokeActions(UnityAction<ActionType> callback) {
      // check conditions and remove listeners
      for (int i = this.listeners.Count - 1; i >= 0; i--) {
        var listener = this.listeners[i];
        if (!listener.conditions.TrueForAll((c) => c())) {
          // Remove listener if any of the conditions are not met.
          this.listeners.RemoveAt(i);
        }
      }

      foreach (var listener in this.listeners) {
        callback.Invoke(listener.action);
      }
    }
  }

  /// <summary>
  /// Event that can auto remove listeners.
  /// </summary>
  public class SmartEvent : TSmartEvent<UnityAction> {
    public void Invoke() {
      this.InvokeActions((action) => action.Invoke());
    }
  }
  /// <summary>
  /// Event that can auto remove listeners.
  /// </summary>
  public class SmartEvent<T0> : TSmartEvent<UnityAction<T0>> {
    public void Invoke(T0 arg) {
      this.InvokeActions((action) => action.Invoke(arg));
    }
  }
  /// <summary>
  /// Event that can auto remove listeners.
  /// </summary>
  public class SmartEvent<T0, T1> : TSmartEvent<UnityAction<T0, T1>> {
    public void Invoke(T0 arg0, T1 arg1) {
      this.InvokeActions((action) => action.Invoke(arg0, arg1));
    }
  }
  /// <summary>
  /// Event that can auto remove listeners.
  /// </summary>
  public class SmartEvent<T0, T1, T2> : TSmartEvent<UnityAction<T0, T1, T2>> {
    public void Invoke(T0 arg0, T1 arg1, T2 arg2) {
      this.InvokeActions((action) => action.Invoke(arg0, arg1, arg2));
    }
  }
  /// <summary>
  /// Event that can auto remove listeners.
  /// </summary>
  public class SmartEvent<T0, T1, T2, T3> : TSmartEvent<UnityAction<T0, T1, T2, T3>> {
    public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3) {
      this.InvokeActions((action) => action.Invoke(arg0, arg1, arg2, arg3));
    }
  }
}
