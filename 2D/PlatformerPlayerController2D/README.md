# PlatformerPlayerController2D

## Installation

```bash
yarn add "https://gitpkg.now.sh/DiscreteTom/unity3d-utils/2D/PlatformerPlayerController2D?platformer-player-controller-2d-0.3.0"
```

## Usage

![](img/inspector.png)

```cs
using DT._2D;
using UnityEngine;

public class Player : MonoBehaviour {
  PlatformerPlayerController2D controller;
  Rigidbody2D body;

  void Start() {
    this.controller = this.GetComponent<PlatformerPlayerController2D>();
    this.body = this.GetComponent<Rigidbody2D>();

    this.controller.Init(new PlatformerPlayerController2D.InitInput() {
      velocity = this.body.velocity
    }).Apply(this.body);
  }

  void Update() {
    this.controller.Move(new PlatformerPlayerController2D.MoveInput {
      horizontal = Input.GetAxisRaw("Horizontal"),
      jumpBtnDown = Input.GetButtonDown("Jump"),
      jumpBtnUp = Input.GetButtonUp("Jump"),
      jumpBtnHeld = Input.GetButton("Jump"),
      grounded = Physics2D.Raycast(this.transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Default")).collider != null,
      velocity = this.body.velocity,
      gravityScale = this.body.gravityScale,
      deltaTime = Time.deltaTime
    }).Apply(this.body);
  }
}
```
