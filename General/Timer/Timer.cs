using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DT.General {
  public class TimeoutHandle {
    Func<bool> finished;
    UnityAction cancel;
    public float startTime { get; private set; }
    public float ttlMs { get; private set; }
    public bool canceled { get; private set; }

    public TimeoutHandle(float startTime, float ttlMs, Func<bool> finished, UnityAction cancel) {
      this.startTime = startTime;
      this.ttlMs = ttlMs;
      this.finished = finished;
      this.cancel = cancel;
      this.canceled = false;
    }

    public void Cancel() {
      this.cancel.Invoke();
      this.canceled = true;
    }

    // just calculate time
    // this might be inaccurate, do NOT use this to check whether task is finished
    public float Progress() {
      return (Time.time - this.startTime) * 1000 / this.ttlMs;
    }

    // not cancel and not finished
    public bool Pending() {
      return !this.canceled && !this.Finished();
    }

    public bool Finished() {
      return this.finished();
    }

    // canceled or finished
    public bool Stopped() {
      return !this.Pending();
    }
  }

  public static class MonoBehaviourExtension {
    // leverage coroutine
    // efficiently call async func
    public static TimeoutHandle SetTimeout(this MonoBehaviour mb, float ms, UnityAction callback) {
      bool run = true;
      bool finished = false;

      IEnumerator GetCoroutine() {
        yield return new WaitForSeconds(ms / 1000);
        if (run) {
          callback.Invoke();
          finished = true;
        }
      }
      mb.StartCoroutine(GetCoroutine());

      var handle = new TimeoutHandle(Time.time, ms, () => finished, () => run = false);
      return handle;
    }
  }

  public delegate TimeoutHandle SetTimeoutFunc(float ms, UnityAction cb);

  // lazy evaluation
  // useful to calculate progress
  public class Timer {
    public float startTime { get; private set; }
    public float ttlMs { get; private set; }

    public Timer(float ttlMs) {
      this.startTime = Time.time;
      this.ttlMs = ttlMs;
    }

    public float Progress() {
      return (Time.time - this.startTime) * 1000 / this.ttlMs;
    }

    public bool Pending() {
      return this.Progress() < 1;
    }

    public bool Finished() {
      return !this.Pending();
    }
  }
}

