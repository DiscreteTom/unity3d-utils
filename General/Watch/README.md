# Watch

Create responsive variables.

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Watch?watch-0.1.0"
```

## Usage

```cs
var a = new Watch<int>(0);

// access value
a.Value;

// watch for change
a.AddListener((value, old) => ...);

// assignment
a.Value = 1;
```
