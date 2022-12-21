# Framework

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Framework?framework-0.1.0"
```

## Usage

```cs
public class App : Entry {
  void Awake() {
    this.Add<SomeClass>();
  }
}

public class SomeController : CBC {
  void Start() {
    this.Get<SomeClass>();
  }
}
```
