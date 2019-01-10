using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D _rigid;
    [SerializeField]
    private float _jumpForce = 5.0f;
    [SerializeField]
    private LayerMask _groundLayer;
    private bool _resetJump = false;
    private bool _grounded = false;
    [SerializeField]
    private float _moveSpeed = 2.5f;
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;
    private SpriteRenderer _swordArcSprite;

    // Use this for initialization
    void Start () {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _swordArcSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Movement();

        if (Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            _playerAnim.Attack();
        }
	}

    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        _grounded = IsGrounded();
                
        if (move > 0)
        {
            _playerSprite.flipX = false;
            _swordArcSprite.flipX = false;
            _swordArcSprite.flipY = false;
            Vector3 newPos = _swordArcSprite.transform.localPosition;
            newPos.x = 0.531f;
            newPos.y = 0.118f;
            newPos.z = -0.25f;
            _swordArcSprite.transform.localPosition = newPos;
        }
        else if (move < 0)
        {
            _playerSprite.flipX = true;
            _swordArcSprite.flipX = false;
            _swordArcSprite.flipY = true;
            Vector3 newPos = _swordArcSprite.transform.localPosition;
            newPos.x = -0.257f;
            newPos.y = 0.069f;
            newPos.z = 0.25f;
            _swordArcSprite.transform.localPosition = newPos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _playerAnim.Jump(true);
        }

        _rigid.velocity = new Vector2(move * _moveSpeed, _rigid.velocity.y);

        _playerAnim.Move(move);
    }

    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, _groundLayer.value);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        if (hitInfo.collider != null)
        {
            if (_resetJump == false)
            {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }

    //funcion con la que esperamos un valor determinado para que se vuelva a ejecutar algo
    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
}
