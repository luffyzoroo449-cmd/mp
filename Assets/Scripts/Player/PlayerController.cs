using UnityEngine;

namespace ShadowRace.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Animator))] // Added Animator dependency
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D RB { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public PlayerStats Stats { get; private set; }
        public Animator anim { get; private set; } // Reference to Animator

        [Header("Movement Options")]
        public float movementVelocity = 10f;
        public float jumpVelocity = 15f;
        public int amountOfJumps = 2;
        private int amountOfJumpsLeft;
        private bool isFacingRight = true;

        [Header("AAA Momentum Settings")]
        public float acceleration = 13f;
        public float deceleration = 16f;
        public float velocityPower = 0.96f;
        public float frictionAmount = 0.2f;

        [Header("Dash Options")]
        public float dashTime = 0.2f;
        public float dashSpeed = 20f;
        public float dashCooldown = 1f;
        private float dashTimeLeft;
        private float lastDash = -100f;
        public bool isDashing;

        [Header("Wall Slide & Jump")]
        public float wallSlideVelocity = 3f;
        public float wallJumpVelocity = 15f;
        public Vector2 wallJumpAngle = new Vector2(1, 2);
        private bool isWallSliding;

        [Header("Collision Check")]
        public Transform groundCheck;
        public Transform wallCheck;
        public float groundCheckRadius = 0.3f;
        public float wallCheckDistance = 0.5f;
        public LayerMask whatIsGround;

        private bool isGrounded;
        private bool isTouchingWall;

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Stats = GetComponent<PlayerStats>();
            anim = GetComponent<Animator>(); // Cache Animator
            
            amountOfJumpsLeft = amountOfJumps;
            wallJumpAngle.Normalize();
        }

        private void Update()
        {
            CheckSurroundings();
            CheckInput();
            CheckMovementDirection();
            CheckDash();
            CheckWallSliding();
            UpdateAnimations(); // Added Animation Update Call
        }

        private void FixedUpdate()
        {
            if (!isDashing) ApplyMovement();
        }

        private void CheckSurroundings()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

            if (isGrounded)
            {
                amountOfJumpsLeft = amountOfJumps;
            }
        }

        private void CheckInput()
        {
            if (InputHandler.JumpInput)
            {
                Jump();
            }

            if (InputHandler.DashInput)
            {
                AttemptToDash();
            }
        }

        private void ApplyMovement()
        {
            if (isWallSliding)
            {
                RB.velocity = new Vector2(RB.velocity.x, -wallSlideVelocity);
            }
            else
            {
                // Original movement calculation
                float currentMovementVelocity = movementVelocity;
                float currentInputX = InputHandler.NormalizedInputX;

                // --- AAA WEATHER PHYSICS MODIFIERS ---
                if (ShadowRace.World.WeatherManager.Instance != null)
                {
                    if (ShadowRace.World.WeatherManager.Instance.CurrentWeather == ShadowRace.World.WeatherManager.WeatherType.Snow)
                    {
                        // Snow slows you down by 25%
                        currentMovementVelocity *= 0.75f;
                    }
                }
                // -------------------------------------

                // AAA MOMENTUM BASED MOVEMENT
                float targetSpeed = currentInputX * currentMovementVelocity;
                float speedDif = targetSpeed - RB.velocity.x;

                float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
                
                // Weather Friction Modifier overrides
                if (ShadowRace.World.WeatherManager.Instance != null && ShadowRace.World.WeatherManager.Instance.CurrentWeather == ShadowRace.World.WeatherManager.WeatherType.Rain)
                {
                    if (Mathf.Abs(targetSpeed) < 0.01f) accelRate *= 0.5f; // Slide more in rain
                }

                float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);

                RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

                // Apply Friction if not moving
                if (Mathf.Abs(currentInputX) < 0.01f && isGrounded && !isDashing) {
                    float amount = Mathf.Min(Mathf.Abs(RB.velocity.x), Mathf.Abs(frictionAmount));
                    amount *= Mathf.Sign(RB.velocity.x);
                    RB.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }
            }
        }

        public void ApplyRecoil(Vector2 force)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0f); // Reset Y velocity for punchy vertical recoil effect
            RB.AddForce(force, ForceMode2D.Impulse);
        }

        private void Jump()
        {
            if (isWallSliding)
            {
                WallJump();
                InputHandler.UseJumpInput();
            }
            else if (isGrounded || amountOfJumpsLeft > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, jumpVelocity);
                amountOfJumpsLeft--;
                InputHandler.UseJumpInput();
            }
        }

        private void WallJump()
        {
            int wallJumpDirection = isFacingRight ? -1 : 1;
            Vector2 forceToAdd = new Vector2(wallJumpAngle.x * wallJumpVelocity * wallJumpDirection, wallJumpAngle.y * wallJumpVelocity);
            RB.velocity = Vector2.zero; // Reset velocity before adding new force
            RB.velocity = forceToAdd;
            amountOfJumpsLeft = amountOfJumps - 1; // Used one jump
            Flip(); // Turn away from wall
        }

        private void AttemptToDash()
        {
            if (Time.time >= lastDash + dashCooldown && Stats.currentStamina >= 20f)
            {
                isDashing = true;
                dashTimeLeft = dashTime;
                lastDash = Time.time;
                Stats.currentStamina -= 20f;
            }
            InputHandler.UseDashInput();
        }

        private void CheckDash()
        {
            if (isDashing)
            {
                if (dashTimeLeft > 0)
                {
                    RB.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, 0); // Straight horizontal dash
                    dashTimeLeft -= Time.deltaTime;
                    // Optional: Disable gravity/collision during dash
                }
                else
                {
                    isDashing = false;
                }
            }
        }

        private void CheckWallSliding()
        {
            if (isTouchingWall && !isGrounded && RB.velocity.y < 0 && InputHandler.NormalizedInputX != 0)
            {
                isWallSliding = true;
            }
            else
            {
                isWallSliding = false;
            }
        }

        private void CheckMovementDirection()
        {
            if (isFacingRight && InputHandler.NormalizedInputX < 0)
            {
                Flip();
            }
            else if (!isFacingRight && InputHandler.NormalizedInputX > 0)
            {
                Flip();
            }
        }

        private void UpdateAnimations()
        {
            if (anim == null) return;
            
            anim.SetBool("isWalking", Mathf.Abs(RB.velocity.x) > 0.1f && !isWallSliding);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", RB.velocity.y);
            anim.SetBool("isWallSliding", isWallSliding);
            anim.SetBool("isDashing", isDashing);
        }

        private void Flip()
        {
            if (isWallSliding) return; // Don't flip while on wall

            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        private void OnDrawGizmos()
        {
            if (groundCheck != null)
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            
            if (wallCheck != null)
                Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        }
    }
}
