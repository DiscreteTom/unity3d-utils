# Command

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/General/Command?command-0.1.0"
```

## Usage

```cs
ICommandBus cBus = new CommandBus();

public struct MyCommand : ICommand {
  public void Exec(IIoCC core) {
    // do something
  }
}

cBus.Exec<MyCommand>();
cBus.Exec(new MyCommand());
```
