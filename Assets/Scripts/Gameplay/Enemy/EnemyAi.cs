using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class EnemyAi : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector2 networkPosition;
    private bool networkIsFlipped;
    private float smoothing = 5f;

    public bool isInMap { get; private set; } = false;

    private bool isFlipped;

    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private EnemyState state = EnemyState.ENTERING;

    private List<GameObject> players = new List<GameObject>();

    private PhotonView view;

    [Header("Stats")]
    private bool canAttack = true;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private bool canFindTargets = true;
    [SerializeField] private float targetChangeTime;

    private float remainingDelay = 0f;

    private Vector2 lastPos;

    public SpawnPoint spawnPoint { get; set; }
    private Window window;

    [Header("Events")]
    public UnityEvent onAttack;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        networkPosition = transform.position;
        lastPos = transform.position;
        remainingDelay = attackDelay;
    }

    public void setSpawnPoint(SpawnPoint spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        this.window = spawnPoint.window;
        isInMap = false;
        window.onBreak.AddListener(() => moveOntoPlayer());
    }

    private void moveBackToWindow()
    {

    }
    private void moveOntoPlayer()
    {
        isInMap = true;
    }

    private IEnumerator attackTimer()
    {
        while(canAttack)
        {
            if(target != null)
            {
                if (Vector2.Distance(transform.position, target.transform.position) <= attackRange) attack();
            }
            
            yield return new WaitForSeconds(attackDelay + Random.Range(0, 1.5f));
        }
    }

    private IEnumerator findTargetTimer()
    {
        while (canFindTargets)
        {
            

            yield return new WaitForSeconds(targetChangeTime);
        }
    }

    private void attack()
    {
        onAttack.Invoke();
    }

    public void setSpeed(float speed)
    {
        agent.speed = speed;
    }

    private void calculatedFlip(Vector2 newPos)
    {
        if (newPos.x > lastPos.x)
        {
            flip(false);
        }
        else if (newPos.x < lastPos.x)
        {
            flip(true);
        }
        lastPos = newPos;
    }

    private void Update()
    {
        calculatedFlip(transform.position);
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            agent.enabled = false;
            transform.position = Vector2.Lerp(transform.position, networkPosition, Time.deltaTime * smoothing);
            agent.enabled = false;
            return;
        }

        findTarget();

        if (target != null ) agent.SetDestination(target.position);

        if (target == null) return;
        if (target.position.x < transform.position.x)
        {
            flip(true);
        }
        else if (target.position.x > transform.position.x)
        {
            flip(false);
        }

        if ( Vector2.Distance(transform.position, target.transform.position) <= attackRange && remainingDelay <= 0)
        {
            remainingDelay = attackDelay;
            attack();
        } else
        {
            remainingDelay = Mathf.Clamp(remainingDelay - Time.deltaTime, 0, attackDelay);
        }

    }

    private void findTarget()
    {
        switch (state)
        {
            case EnemyState.ENTERING:
                if (!window.isBroken) target = window.transform;
                else
                {
                    target = spawnPoint.insideGoal;
                    if(Vector2.Distance(transform.position, target.transform.position) <= agent.stoppingDistance)
                    {
                        state = EnemyState.HUNTING;
                    }
                }
                break;
            case EnemyState.HUNTING:
                players = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
                var orderedPlayers = players.OrderBy(player => Vector2.Distance(player.transform.position, transform.position)).ToList();
                if (players.Count > 0) target = orderedPlayers[0].transform;
                else target = null;

                break;
            case EnemyState.DANCING:
                target = null;
                break;
            default:
                break;
        }
    }

    private void flip(bool flipped)
    {
        isFlipped = flipped;

        if (flipped) transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }

    public void remove()
    {
        view.RPC("removeRPC", RpcTarget.All);
    }

    [PunRPC]
    public void removeRPC()
    {
        Destroy(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

    public bool getIsFlipped()
    {
        return isFlipped;
    }
}
