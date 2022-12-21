# IoC

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/IoC?ioc-0.1.0"
```

## Usage

```cs
var core = new IoCC();

core.Add<SomeClass>();
core.Add(new SomeClass());
core.Get<SomeClass>();
```
