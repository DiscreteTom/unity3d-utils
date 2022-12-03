# Watch

Create responsive variables.

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Watch?watch-0.2.4"
```

## Usage

### For Value Type

```cs
var a = new Watch<int>(0);

// access value
a.Value;

// watch for changes
a.AddListener((value, old) => ...);

// assignment
a.Value = 1;
```

### For Reference Type

```cs
var a = new WatchRef<SomeClass>(null);

// watch for changes
a.AddListener(value => ...);

// set value will trigger change events
a.SetValue(new SomeClass());
// access value will trigger change events
a.Apply(v => v.a = 1);
// to access value without trigger change events
a.ReadOnlyApply(v => v.a = 1);
```

### For Collections

```cs
var list = new WatchList<int>();
var dict = new WatchDictionary<string, int>();
var array = new WatchArray<int>(10);

// get readonly collection
list.Value;
dict.Value;
array.Value;

// auto trigger change events
list.Add(1);
list[0] = 1;

// readonly methods won't trigger change events
list.IndexOf(1);

// watch for changes & access value to trigger change events
// same as WatchRef
```

### Computed

```cs
var v = new Watch<int>(0);

// every time `v` is changed, calculate `c` again.
var c = new Computed<int>(() => v.Value + 1).Watch(v);
```
