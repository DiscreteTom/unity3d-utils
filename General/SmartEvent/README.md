# SmartEvent

Event that can auto remove listeners.

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/SmartEvent?smart-event-0.1.0"
```

## Usage

```cs
var a = new SmartEvent();
a.AddListener(() => ...)
  .When(() => b > c) // Remove the listener from the event when the condition is not met.
  .Require(someGameObject); // Remove the listener from the event if obj is null.
```
