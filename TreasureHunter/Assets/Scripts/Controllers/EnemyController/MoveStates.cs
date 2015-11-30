using UnityEngine;
using System.Collections;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Interfaces;
using System.Collections.Generic;

public class MoveStates : MonoBehaviour
{
    private Transform target { get { return SceneManager.Instance.MazeManager.Player.transform; } }

    #region CLASS SETTINGS

    private const float ROTATION_SPEED = 5;

    #endregion

    #region SCENE REFERENCES

    public EnemyHealth enemyHealth;
    //NavigationTransforms
    public Transform SightLeftCorner;
    public Transform SightRightCorner;
    public Transform AttackLimit;
    public Transform AttackPosition;

    #endregion

    public int Speed = 5;
    [HideInInspector]
    public MoveState movingState = MoveState.idle;
    public float targetingTime = 5.0f;
    public float idleAfterRunTime = 5.0f;

    private int currentPathCornerIndex = 0;
    private bool followingPath = false;
    private bool backToStart = true;
    private float followingTimer = 0;
    private NavMeshPath navPath;
    private Vector3 startingPosition = Vector3.zero;
    private Vector3 targetPosition;
    private Color sightColor = Color.green;

    private void Start()
    {
        navPath = new NavMeshPath();
        SetStartPosition(transform.position);
    }

	private void Update () 
    {
		if (!enemyHealth.is_dead)
        {
            CheckIsOnTheFloor();
			if (!IsPlayerVisible()) 
            {
                //Follow the path
                if (followingPath)
                {
                    followingTimer+=Time.deltaTime;
                    if (navPath.corners.Length - 1 > currentPathCornerIndex && (followingTimer < targetingTime||backToStart))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - navPath.corners[currentPathCornerIndex + 1]), ROTATION_SPEED * Time.deltaTime);
                        transform.position += -transform.forward * Speed * Time.deltaTime;
                        movingState = MoveState.running;
                        sightColor = Color.green;
                        if (Vector3.Distance(transform.position, targetPosition) > 1)
                        {
                            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, navPath);
                        }
                        else if (targetPosition == startingPosition)
                        {
                            followingPath = false;
                        }
                    }
                    else
                    {
                        if (!backToStart)
                        {
                            NavMesh.CalculatePath(transform.position, startingPosition, NavMesh.AllAreas, navPath);
                            backToStart = true;
                            currentPathCornerIndex = 0;
                            targetPosition = startingPosition;
                        }
                        else
                        {
                            followingPath = false;
                        }
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - target.position), ROTATION_SPEED * Time.deltaTime);
                    movingState = MoveState.idle;
                    sightColor = Color.green;
                }
			}
			else
            {
                followingTimer = 0;
                followingPath = true;
                backToStart = false;
                currentPathCornerIndex = 0;
                float distanceToPlayer = Vector3.Distance(target.position, transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - target.position), ROTATION_SPEED * Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                targetPosition = new Vector3(target.position.x, 5, target.position.z);
                NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, navPath);
                if (distanceToPlayer > Vector3.Distance(SightLeftCorner.position, transform.position))
                {
                    //Observe player
                    movingState = MoveState.aggressive_idle;
					sightColor = Color.yellow;
				}
                else if (distanceToPlayer > Vector3.Distance(AttackLimit.position, transform.position)) 
                {
                    //Run to player
                    transform.position += -transform.forward * Speed * Time.deltaTime;
					movingState = MoveState.running;
					sightColor = Color.red;
				}
				else
                {
                    //make hit
					movingState = MoveState.attack;
                }
			}
        }
    }

    private void CheckIsOnTheFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (hit.collider.tag == "terrain")
            {
                transform.Translate(0, -hit.distance, 0);
            }
        }
    }

    private bool IsPlayerVisible()
    {
        RaycastHit hit;
        Vector3 start = transform.position + new Vector3(0, 2, 0);
        Vector3 direction = (target.position - transform.position).normalized;
        return !Physics.Raycast(start, direction, out hit, Vector3.Distance(start, target.position), 1<<LayerMask.NameToLayer("Wall"));
    }

    public void SetStartPosition(Vector3 start)
    {
        startingPosition = new Vector3(start.y, 5, start.z);
        Debug.DrawLine(start, start+ new Vector3(0,50,0), Color.red, 30);
    }

    public void MakeHit()
    {
        Collider[] colliders = Physics.OverlapSphere(AttackPosition.position, Vector3.Distance(transform.position, AttackLimit.position));
        List<IDamageable> hittedMonsters = new List<IDamageable>();
        foreach (Collider col in colliders)
        {
            PlayerAttack eh = col.GetComponentInParent<PlayerAttack>();
            if (eh != null)
            {
                if (!hittedMonsters.Exists(id => id == eh))
                {
                    hittedMonsters.Add(eh);
                    eh.TakeDamage(0.05f);
                }
            }
        }
    }

	private void DrawAdditionalGizmos() 
    {
        Debug.DrawLine(transform.position, SightLeftCorner.position, sightColor, 0.01f);
        Debug.DrawLine(transform.position, SightRightCorner.position, sightColor, 0.01f);
        for (int i = 0; navPath!=null&& i < navPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(navPath.corners[i], navPath.corners[i + 1], Color.magenta, 0.01f, false);
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + Vector3.up*4, Vector3.Distance(AttackLimit.position, transform.position));
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(AttackPosition.position, Vector3.Distance(transform.position, AttackLimit.position));
        DrawAdditionalGizmos();
    }
}