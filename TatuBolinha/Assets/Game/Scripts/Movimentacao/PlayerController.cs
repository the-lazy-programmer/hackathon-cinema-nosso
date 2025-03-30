using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Character;

[RequireComponent(typeof(CharacterMovement2D))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    CharacterMovement2D playerMovement;
    SpriteRenderer spriteRenderer;
    //PlayerInputs playerInput;
    InputManager playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<CharacterMovement2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //playerInput = GetComponent<PlayerInputs>();
        playerInput = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimentacao
        Vector2 movementInput = InputManager.Movement;
        playerMovement.ProcessMovementInput(movementInput);

        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        //Pulo
        if (InputManager.jumpPressed)
        {
            playerMovement.Jump();
        }
        if (InputManager.jumpReleased == false)
        {
            playerMovement.AbortJump();
        }

        //Agachar
        /*if (playerInput.IsCrouchButtonDown())
        {
            playerMovement.Crouch();
            
        }
        else if (playerInput.IsCrouchButtonUp())
        {
            playerMovement.UnCrouch();
          
        }*/
    }
}
