using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowRace.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 RawMovementInput { get; private set; }
        public int NormalizedInputX { get; private set; }
        public int NormalizedInputY { get; private set; }
        
        public bool JumpInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool SpecialSkillInput { get; private set; }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();
            NormalizedInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormalizedInputY = Mathf.RoundToInt(RawMovementInput.y);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
            }
            if (context.canceled)
            {
                JumpInput = false;
            }
        }

        public void UseJumpInput() => JumpInput = false;

        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.started) DashInput = true;
            if (context.canceled) DashInput = false;
        }

        public void UseDashInput() => DashInput = false;

        public void OnAttackInput(InputAction.CallbackContext context)
        {
            if (context.started) AttackInput = true;
            if (context.canceled) AttackInput = false;
        }

        public void UseAttackInput() => AttackInput = false;
        
        public void OnSpecialSkillInput(InputAction.CallbackContext context)
        {
            if (context.started) SpecialSkillInput = true;
            if (context.canceled) SpecialSkillInput = false;
        }

        public void UseSpecialSkillInput() => SpecialSkillInput = false;
    }
}
