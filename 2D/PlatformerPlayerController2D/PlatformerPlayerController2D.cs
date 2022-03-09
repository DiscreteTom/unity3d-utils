using DT.General;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace DT._2D
{
  [RequireComponent(typeof(Rigidbody2D))]
  [RequireComponent(typeof(SpriteRenderer))]
  public class PlatformerPlayerController2D : MonoBehaviour
  {
    [Header("Input")]
    [InputAxis]
    public string[] horizontalAxisNames = new string[1] { "Horizontal" };
    [InputAxis]
    public string[] jumpButtonNames = new string[1] { "Jump" };

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
    [SerializeField] UnityEvent onJump = new UnityEvent();
    public void OnJump(UnityAction f) => this.onJump.AddListener(f);
    [SerializeField] UnityEvent onAirJump = new UnityEvent();
    public void OnAirJump(UnityAction f) => this.onAirJump.AddListener(f);

    [Header("Ground Checker")]
    public BaseCollisionChecker groundChecker;
    [Min(0)]
    public float coyoteTimeMs = 200;
    [Min(0)]
    public float coolDownAfterJumpMs = 50;

    // expose some state
    public float horizontalInput { get; private set; }
    public bool grounded { get; private set; }

    // members to calculate velocity y
    float lastY;
    [HideInInspector] public int airJumpCountLeft;
    TimeoutHandle apexBonusHandle;
    Timer groundCheckerTimer;
    Timer coyoteTimer;

    // other components
    Rigidbody2D body;
    SpriteRenderer sprite;

    void Start()
    {
      this.sprite = this.GetComponent<SpriteRenderer>();
      this.body = this.GetComponent<Rigidbody2D>();
      this.body.gravityScale = this.normalGravityScale;
      this.lastY = this.body.velocity.y;
      this.airJumpCountLeft = this.maxAirJumpCount;
      this.groundCheckerTimer = new Timer(0);
    }

    void Update()
    {
      // check collision
      this.grounded = this.groundChecker.Check();

      // gather input
      var jumpBtnDown = InputHelper.GetAnyButtonDown(this.jumpButtonNames);
      var jumpBtnUp = InputHelper.GetAnyButtonUp(this.jumpButtonNames);
      var jumpBtnHold = InputHelper.GetAnyButton(this.jumpButtonNames);
      this.horizontalInput = InputHelper.GetAnyAxisRaw(this.horizontalAxisNames);

      // calculate result
      var x = this.CalculateX(this.body.velocity.x, this.horizontalInput);
      var result = this.CalculateY(this.body.velocity.y, jumpBtnDown, jumpBtnUp, jumpBtnHold);

      // apply result
      this.body.velocity = new Vector2(x, result.y);
      this.body.gravityScale = result.gravityScale;
      if (result.jump) this.body.AddForce(this.jumpForce, ForceMode2D.Impulse);

      // update sprite direction
      if (this.horizontalInput > 0)
      {
        this.sprite.flipX = false;
      }
      else if (this.horizontalInput < 0)
      {
        this.sprite.flipX = true;
      }
    }

    float CalculateX(float x, float input)
    {
      if (input != 0)
      {
        x += input * this.acceleration * Time.deltaTime;
        x = Mathf.Clamp(x, -this.maxHorizontalSpeed, this.maxHorizontalSpeed);
      }
      else
      {
        // no input, slow player down
        x = Mathf.MoveTowards(x, 0, this.deceleration * Time.deltaTime);
      }
      return x;
    }

    (float y, float gravityScale, bool jump) CalculateY(float oldY, bool jumpBtnDown, bool jumpBtnUp, bool jumpBtnHold)
    {
      (float y, float gravityScale, bool jump) result = (oldY, this.body.gravityScale, false);

      if (this.grounded)
      {
        if (this.groundCheckerTimer.Finished())
        {
          // restore jump count
          this.airJumpCountLeft = this.maxAirJumpCount;
        }
        this.coyoteTimer = null;
      }
      else
      {
        if (this.coyoteTimer == null)
        {
          this.coyoteTimer = new Timer(this.coyoteTimeMs);
        }
      }

      if ((jumpBtnDown && (this.grounded || !this.coyoteTimer.Finished() || this.airJumpCountLeft > 0)) || (this.holdButtonAutoJump && jumpBtnHold && (this.grounded || !this.coyoteTimer.Finished())))
      { // apply jump
        this.onJump.Invoke();
        this.ClearApexBonusHandle();
        result.y = 0; // clear velocity y to apply jump force
        result.gravityScale = this.normalGravityScale;
        if (!this.grounded && this.coyoteTimer.Finished())
        {
          this.airJumpCountLeft--;
          this.onAirJump.Invoke();
        }
        this.coyoteTimer = new Timer(0); // prevent next coyote jump
        this.groundCheckerTimer = new Timer(this.coolDownAfterJumpMs);
        result.jump = true;
      }
      else
      {
        if (oldY > 0 && jumpBtnUp)
        { // moving up, and jump early stopped
          result.gravityScale = this.jumpEarlyEndGravityScale;
        }
        else if (this.lastY > 0 && oldY <= 0)
        { // touch apex
          result.gravityScale = 0;
          result.y = 0; // let player slide for a distance
          this.apexBonusHandle = this.SetTimeout(apexBonusTimeMs, () => this.body.gravityScale = this.normalGravityScale);
        }
        else if (oldY < 0)
        { // fall
          // prevent fall too fast
          result.y = Mathf.Max(oldY, -maxFallSpeedAbs);
        }
      }

      this.lastY = result.y;
      return result;
    }

    void ClearApexBonusHandle()
    {
      this.apexBonusHandle?.Cancel();
      this.apexBonusHandle = null;
    }
  }
}
