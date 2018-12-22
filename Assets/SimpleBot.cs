using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Action {
    public ActionType type;
    public float amount;
}
public class SimpleBot : NetworkBehaviour {

    [SerializeField]
    Action[] actions;
    private float aim;
    private float bodyAngle;
    public float speed;
    public bool loop;
    private int currentAction = 0;
    private Rigidbody2D rb;
    public float LookVelocity;
    public GameObject Bullet;
    private float fireCounter;
    private bool canFire;
    private float fireDuration;
    private float moveDuration;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        fireDuration = 0;
        moveDuration = 0;

    }
    private void Update() {
        if (fireCounter <= 0) {
            canFire = true;
        } else {
            fireCounter -= Time.deltaTime;
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (isServer) {
            if (actions[currentAction].type == ActionType.Look) {
                if (CompareAngle(aim, actions[currentAction].amount)) {
                    aim = actions[currentAction].amount;
                    currentAction = (currentAction + 1) % actions.Length;
                } else {
                    if (aim < actions[currentAction].amount) {
                        aim += LookVelocity;
                    }
                    if (aim > actions[currentAction].amount) {
                        aim -= LookVelocity;
                    }
                }
            }
            if (actions[currentAction].type == ActionType.Shoot) {
                if (fireDuration < actions[currentAction].amount) {
                    if (canFire) {
                        CmdFire();
                        fireCounter = Bullet.gameObject.GetComponent<Bullet>().delay;
                        canFire = false;
                    }
                    fireDuration += Time.fixedDeltaTime;
                } else {
                    fireDuration = 0;
                    currentAction = (currentAction + 1) % actions.Length;
                }
            }
            if (actions[currentAction].type == ActionType.Move) {
                if (moveDuration < actions[currentAction].amount) {
                    rb.velocity = new Vector2(speed * Mathf.Cos(bodyAngle * Mathf.Deg2Rad), speed * Mathf.Sin(bodyAngle * Mathf.Deg2Rad));
                    moveDuration += Time.fixedDeltaTime;
                } else {
                    rb.velocity = new Vector2(0, 0);
                    moveDuration = 0;
                    currentAction = (currentAction + 1) % actions.Length;
                }
            }
            if (actions[currentAction].type == ActionType.Turn) {
                bodyAngle = actions[currentAction].amount;
                currentAction = (currentAction + 1) % actions.Length;
            }
            transform.eulerAngles = new Vector3(0, 0, aim);
        }
    }

    [Command]
    void CmdFire() {
        GameObject b = Instantiate(Bullet);
        b.transform.position = transform.Find("BulletExitPoint").position;
        b.GetComponent<Bullet>().angle = (aim) * Mathf.Deg2Rad;
        //NetworkServer.SpawnWithClientAuthority(b, connectionToClient);
        NetworkServer.Spawn(b);
    }

    bool CompareAngle(float a, float b) {
        return Mathf.Abs(a % 360 - b % 360) < LookVelocity;
    }
}
