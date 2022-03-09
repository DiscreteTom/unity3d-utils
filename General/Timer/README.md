# Timer

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Timer?timer-0.1.0"
```

## Usage

### SetTimeout

```cs
using UnityEngine;
using DT.General;

public class MyClass : MonoBehaviour
{
  void Start()
  {
    var handle = this.SetTimeout(100, () => { Debug.Log(123); });
    handle.Cancel(); // cancel callback function
  }
}
```

### Timer

```cs
using DT.General;

void f()
{
  var timer = new Timer(500);
  timer.Progress(); // get progress
}
```
