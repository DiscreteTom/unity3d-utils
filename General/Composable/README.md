# Composable

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Composable?composable-0.1.0"
```

## Usage

```cs
public class MyController : ComposableBehaviour {
  void Start() {
    this.OnUpdate.AddListener(() => {
      // do something
    });
  }
}
```
