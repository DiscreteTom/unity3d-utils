using UnityEngine;

namespace DT._2D {
  public class PlatformerPlayerController2D : MonoBehaviour {
    [Header("Horizontal Move")]
    [Min(0)]
    public float maxHorizontalSpeed = 13;
    [Min(0)]
    public float acceleration = 90;
    [Min(0)]
    public float deceleration = 60;

    [Header("Jump")]
    [Min(0)]
    public int maxAirJumpCount = 1;
    public Vector2 jumpForce = new Vector2(0, 20);
    [Min(0)]
    public float normalGravityScale = 5;
    [Min(0)]
    public float jumpEarlyEndGravityScale = 20;
    [Min(0)]
    public float maxFallSpeedAbs = 40;
    [Min(0)]
    public float apexBonusTimeMs = 20;
    public bool holdButtonAutoJump = false;

    [Header("Ground Checker")]
    [Min(0)]
    public float coyoteTimeMs = 200;
    [Min(0)]
    public float coolDownAfterJumpMs = 50;

    // members to calculate velocity y
    float lastY;
    [HideInInspector] public int airJumpCountLeft;
    float groundCheckerCDLeftMs;
    bool shouldCoyoteTime;
    float coyoteCDLeftMs;
    float apexBonusCDLeftMs;

    public struct MoveInput {
      public float horizontal;
      public bool jumpBtnDown;
      public bool jumpBtnUp;
      public bool jumpBtnHeld;
      public bool grounded;
      public Vector2 velocity;
      public float gravityScale;
      public float deltaTime;
    }
    public struct MoveResult {
      public bool jumped;
      public bool airJumped;
      public float gravityScale;
      public Vector2 velocity;
    }

    public MoveResult Init(MoveInput input) {
      this.lastY = input.velocity.y;
      this.airJumpCountLeft = this.maxAirJumpCount;
      this.groundCheckerCDLeftMs = 0;
      this.shouldCoyoteTime = false;
      this.coyoteCDLeftMs = 0;
      this.apexBonusCDLeftMs = 0;

      return new MoveResult {
        jumped = false,
        airJumped = false,
        gravityScale = this.normalGravityScale,
        velocity = input.velocity,
      };
    }

    public MoveResult Move(MoveInput input) {
      // init result
      var result = new MoveResult() {
        jumped = false,
        airJumped = false,
        velocity = input.velocity,
        gravityScale = input.gravityScale,
      };

      // update timers
      if (this.groundCheckerCDLeftMs > 0) this.groundCheckerCDLeftMs -= input.deltaTime * 1000;
      if (this.coyoteCDLeftMs > 0) this.coyoteCDLeftMs -= input.deltaTime * 1000;
      if (this.apexBonusCDLeftMs > 0) {
        this.apexBonusCDLeftMs -= input.deltaTime * 1000;
        if (this.apexBonusCDLeftMs <= 0) {
          // restore gravity scale
          result.gravityScale = this.normalGravityScale;
        }
      }

      // calculate velocity x
      if (input.horizontal != 0) {
        result.velocity.x += input.horizontal * this.acceleration * input.deltaTime;
        if (Mathf.Abs(result.velocity.x) > this.maxHorizontalSpeed) { // too fast
          if (result.velocity.x > 0) {
            result.velocity.x = Mathf.MoveTowards(result.velocity.x, this.maxHorizontalSpeed, this.deceleration * input.deltaTime);
          } else {
            result.velocity.x = Mathf.MoveTowards(result.velocity.x, -this.maxHorizontalSpeed, this.deceleration * input.deltaTime);
          }
        }
      } else {
        // no input, slow player down
        result.velocity.x = Mathf.MoveTowards(result.velocity.x, 0, this.deceleration * input.deltaTime);
      }

      // ground checker
      if (input.grounded) {
        if (this.groundCheckerCDLeftMs <= 0) {
          // CD is over, reset air jump count
          this.airJumpCountLeft = this.maxAirJumpCount;
        }
        this.shouldCoyoteTime = true;
      } else { // not grounded
        if (this.shouldCoyoteTime) {
          this.coyoteCDLeftMs = this.coyoteTimeMs;
          this.shouldCoyoteTime = false;
        }
      }

      // jump
      if (
        (input.jumpBtnDown &&
          (input.grounded ||
            this.coyoteCDLeftMs > 0 ||
            this.airJumpCountLeft > 0)) ||
        (this.holdButtonAutoJump &&
          input.jumpBtnHeld &&
          (input.grounded || this.coyoteCDLeftMs > 0))
      ) { // should jump
        result.jumped = true;
        this.apexBonusCDLeftMs = 0; // not in apex bonus
        result.velocity.y = 0; // clear velocity y to apply jump force
        result.gravityScale = this.normalGravityScale; // restore gravity scale
        // is air jump?
        if (!input.grounded && this.coyoteCDLeftMs <= 0) {
          this.airJumpCountLeft--;
          result.airJumped = true;
        }
        // prevent next coyote jump
        this.shouldCoyoteTime = false;
        this.coyoteCDLeftMs = 0;
        // start ground checker CD
        this.groundCheckerCDLeftMs = this.coolDownAfterJumpMs;
      } else { // not jump
        var currentY = input.velocity.y;
        if (currentY > 0 && input.jumpBtnUp) { // moving up, and jump btn early released
          result.gravityScale = this.jumpEarlyEndGravityScale; // apply early end gravity scale
        } else if (this.lastY > 0 && currentY <= 0) { // touch apex
          // let player slide for a distance
          result.gravityScale = 0;
          result.velocity.y = 0;
          this.apexBonusCDLeftMs = this.apexBonusTimeMs;
        } else if (currentY < 0) { // fall
          // prevent fall too fast
          result.velocity.y = Mathf.Max(currentY, -maxFallSpeedAbs);
        }
      }
      // update lastY
      this.lastY = result.velocity.y;

      return result;
    }
  }
}
