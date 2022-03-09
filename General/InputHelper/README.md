# InputHelper

Allow you to check a list of axis/button names.

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/InputHelper?input-helper-0.1.1"
```

## Usage

```cs
using DT.General;

public string[] horizontalInputNames;

InputHelper.GetAnyButton(horizontalInputNames);
InputHelper.GetAnyButtonDown(horizontalInputNames);
InputHelper.GetAnyButtonUp(horizontalInputNames);
InputHelper.GetAnyAxis(horizontalInputNames);
InputHelper.GetAnyAxisRaw(horizontalInputNames);
```
