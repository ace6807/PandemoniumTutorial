using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayerMask, wallLayerMask;
    [SerializeField] float playerHorizontalSpeed, stopFactor, jumpForce;


    float horizontalInput;
    bool isGrounded, jumpPressed, didJump, didWallJump, isCrouching, canAttack;
    Vector2 playerColliderSize;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    Collider2D lastWallCollider;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerColliderSize = playerCollider.size;
    }

    private void GetInput(){
        isCrouching = Input.GetAxisRaw("Vertical") == -1;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
    }

    private void GroundCheck(){
        isGrounded = IsGrounded();
        if (isGrounded){
            lastWallCollider = null;
        }
    }

    private void JumpCheck(){
        if (jumpPressed && (isGrounded || !isGrounded && IsTouchingWall())){
            didJump = true;
        }
    }

    private void CheckCanAttack(){
        canAttack = CanAttack();
        print(canAttack);
    }

    private void Update()
    {
        CheckCanAttack();
        GetInput();
        GroundCheck();
        JumpCheck();
    }

    private void MoveCharacter(){
        body.velocity = new Vector2(horizontalInput * playerHorizontalSpeed, body.velocity.y);
    }

    private void SetCharacterDirection(){
        if (horizontalInput < 0){
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0){
            spriteRenderer.flipX = false;
        }
    }

    private void Jump(){
        if (didJump){
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            didJump = false;
        }
    }

    private void SetMovementAnimation()
    {
        playerAnimator.SetBool("walking", horizontalInput != 0);
        playerAnimator.SetBool("grounded", isGrounded);
        playerAnimator.SetBool("crouching", isCrouching);
    }

    private void UpdateCollider(){
        if (isCrouching){
            playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y/2);
        }
        else{
            playerCollider.size = playerColliderSize;
        }
    }

    private void FixedUpdate(){
        MoveCharacter();
        SetMovementAnimation();
        SetCharacterDirection();
        // UpdateCollider(); // Offset between size of sprite and collider
        Jump();
    }

    private bool IsTouchingWall(){
        var castSize = new Vector2(playerCollider.size.x + .01f, playerCollider.size.y);
        var castDirection = new Vector2(horizontalInput, 0);
        var hit = Physics2D.BoxCast(playerCollider.bounds.center, castSize, 0, castDirection, 0.1f, wallLayerMask);

        var newWallJumped = hit.collider != null && lastWallCollider != hit.collider;
        lastWallCollider = hit.collider;

        return newWallJumped;
    }

    private bool IsGrounded(){
        var hit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayerMask);
        return hit.collider != null;
    }

    public bool CanAttack(){
        return horizontalInput == 0 && isGrounded && !IsTouchingWall();
    }
}
