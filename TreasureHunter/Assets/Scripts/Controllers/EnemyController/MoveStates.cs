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
    private Color sightColor = Color.green;
	
	private void Update () 
    {
		if (!enemyHealth.is_dead) 
        {
            CheckIsOnTheFloor();
			var _distance = Vector3.Distance (target.position, transform.position);
			var angle = Vector3.Angle(-transform.forward, target.position - transform.position);
			if (!IsPlayerVisible()) 
            {
                //Do nothing
				movingState = (int)MoveState.idle;
				sightColor = Color.green;
			}
			else
            {
				if (Vector3.Distance(target.position, transform.position) > Vector3.Distance(SightLeftCorner.position, transform.position))
                {
                    //Observe player
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (transform.position - target.position), ROTATION_SPEED * Time.deltaTime);
                    transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                    movingState = MoveState.aggressive_idle;
					sightColor = Color.yellow;
				}
                else if (_distance > Vector3.Distance(AttackLimit.position, transform.position)) 
                {
                    //Run to player
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (transform.position - target.position), ROTATION_SPEED * Time.deltaTime);
                    transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
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
        Debug.DrawRay(transform.position, target.position - transform.position);
        return !Physics.Raycast(transform.position, target.position - transform.position, out hit, LayerMask.NameToLayer("Wall"));
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

	private void DrawSight() 
    {
        Debug.DrawLine(transform.position, SightLeftCorner.position, sightColor, 0.01f);
        Debug.DrawLine(transform.position, SightRightCorner.position, sightColor, 0.01f);
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + Vector3.up*4, Vector3.Distance(AttackLimit.position, transform.position));
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(AttackPosition.position, Vector3.Distance(transform.position, AttackLimit.position));
        DrawSight();
    }
}
