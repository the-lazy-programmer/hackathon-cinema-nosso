using Platformer2D.Character;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teste : MonoBehaviour
{
    private InputSystem_Actions action;
    private CharacterMovement2D characterMovement;
    private SpriteRenderer spriteRenderer;
    private void Start() {
         action = new InputSystem_Actions();
        action.Enable();
       characterMovement = GetComponent<CharacterMovement2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Jump_performed(InputAction.CallbackContext context) {
        if (context.performed){
            Debug.Log("pular");
        }
    }
    void Update()
    {
        Vector2 Movimento = action.Platformer2D.Move2D.ReadValue<Vector2>();
        characterMovement.ProcessMovementInput(Movimento);
        flip(Movimento.x);
        if (action.Platformer2D.Jump.WasPerformedThisFrame())
        {
            characterMovement.Jump();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) Debug.Log("Espaço");
    }
    private void flip(float Direcao) {
        if (Direcao > 0 )
        {
            spriteRenderer.flipX = false;
        }else if(Direcao < 0)
        {
            spriteRenderer.flipX=true;
        }
    }
    
}
