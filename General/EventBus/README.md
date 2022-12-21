# EventBus

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/EventBus?event-bus-0.1.0"
```

## Usage

```cs
var eBus = new EventBus();

eBus.AddListener("my-event", () => { ... });
eBus.Invoke("my-event");
```
