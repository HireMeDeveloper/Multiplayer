using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerMovement : MonoBehaviour, IPunObservable
{
    private PlayerEvents playerEvents;
    private PlayerStatus status;
    private PlayerData data;

    private Vector2 networkPosition;
    private bool networkIsFlipped = false;
    private float smoothing = 5f;

    private bool isFlipped = false;
    [SerializeField] private MovementState movementState = MovementState.NORMAL;
    private Vector3 slideDir;
    private float slideSpeed;

    private Rigidbody2D rb;
    [SerializeField] private new SpriteRenderer renderer;

    private Vector2 movement;

    private float currentSpeed;

    private KeyCode dodgeButton = KeyCode.Mouse1;
    private const float DODGE_COOLDOWN = 3f;
    private float remainingDodgeCooldown;

    [Header("Stats")]
    [SerializeField] private float defaultSpeed = 6f;

    [Header("Events")]
    public UnityEvent onMove;
    public UnityEvent onNotMove;

    private void Awake()
    {
        data = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody2D>();
        playerEvents = GetComponent<PlayerEvents>();
        currentSpeed = defaultSpeed;
        status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        networkPosition = rb.transform.position;
        movementState = MovementState.NORMAL;
    }

    private void Update()
    {
        if (!PhotonView.Get(this).IsMine) return;
        if (movementState == MovementState.NORMAL)
        {
            handleMovement();
            handleDodgeRoll();
        }
        else
        {
            handleDodgeSliding();
        }
    }


    private void FixedUpdate()
    {
        if (!PhotonView.Get(this).IsMine)
        {
            rb.position = Vector3.Lerp(rb.position, networkPosition, Time.deltaTime * smoothing);

            if (networkIsFlipped) transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            return;
        }

        currentSpeed = status.getSpeed();
        rb.position += movement * (currentSpeed * status.speedMultiplier) * Time.deltaTime;
    }

    private IEnumerator dodgeCooldownTimer()
    {
        remainingDodgeCooldown = DODGE_COOLDOWN;
        var segment = DODGE_COOLDOWN / 6;
        var index = 0;

        CursorManager.instance.setCursorIndex(index);

        while (remainingDodgeCooldown > 0)
        {
            yield return new WaitForSeconds(segment);
            remainingDodgeCooldown -= segment;
            index++;
            CursorManager.instance.setCursorIndex(index);
        }
    }

    private void handleDodgeRoll()
    {
        if (Input.GetKeyDown(dodgeButton) && remainingDodgeCooldown <= 0)
        {
            playerEvents?.onDodgeRoll.Invoke();
            movementState = MovementState.DODGE_ROLLING;
            var preSlideDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            slideDir = new Vector3(preSlideDir.x, preSlideDir.y, 0);
            slideSpeed = 80f;
            StartCoroutine("dodgeCooldownTimer");
        }
    }

    private void handleDodgeSliding()
    {
        rb.transform.position += (slideDir * slideSpeed * Time.deltaTime);

        slideSpeed -= slideSpeed * 10f * Time.deltaTime;

        if (slideSpeed < 5) movementState = MovementState.NORMAL;
    }

    private void handleMovement()
    {
        var playerPoint = Camera.main.WorldToScreenPoint(transform.position);
        var mouse = Input.mousePosition;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (mouse.x < playerPoint.x)
        {
            isFlipped = true;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            isFlipped = false;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (movement.x == 0 && movement.y == 0) onNotMove.Invoke();
        else onMove.Invoke();
    }

    public void setSpeed(float speed)
    {
        this.currentSpeed = speed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.transform.position);
            stream.SendNext(isFlipped);
        } else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkIsFlipped = (bool)stream.ReceiveNext();
        }
    }
}
