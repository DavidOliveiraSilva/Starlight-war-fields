
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public float speed;
    private bool canMove = true;
    private Rigidbody2D rb;
    private float mira;
    public GameObject mainCamera;
    //gun
    public GameObject Bullet;
    //end gun

    private float fireCounter;
    private bool canFire = false;

    private ParticleSystem plasma;
    private bool dashing;
    public float dashDuration;
    private float dashTime;
    public float dashSpeed;

    public GameObject skill;
    // Use this for initialization
    private void Start() {
        GameObject playerCamera = Instantiate(mainCamera);
        if (isLocalPlayer) {
            playerCamera.GetComponent<FollowPlayer>().target = gameObject;
            playerCamera.SetActive(true);
        } else {
            playerCamera.SetActive(false);
        }
        
    }
    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        plasma = transform.Find("Plasma").gameObject.GetComponent<ParticleSystem>();
        var emission = plasma.emission;
        emission.rateOverTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (fireCounter <= 0) {
            canFire = true;
        } else {
            fireCounter -= Time.deltaTime;
        }
        
        
    }
    private void FixedUpdate() {
        if (isLocalPlayer) {
            //Movimento
            if (canMove) {

                float hor = Input.GetAxis("Horizontal");
                float ver = Input.GetAxis("Vertical");
                float angle = Mathf.Atan2(ver, hor);
                if (Mathf.Abs(hor) > 0 || Mathf.Abs(ver) > 0) {
                    rb.velocity = new Vector2(speed * Mathf.Cos(angle) * Mathf.Abs(hor), speed * Mathf.Sin(angle) * Mathf.Abs(ver));
                } else {
                    rb.velocity = new Vector2(0, 0);
                }
            }
            //Mira
            Vector3 mPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            mira = Mathf.Atan2(transform.position.y - mPosition.y, transform.position.x - mPosition.x);
            mira = mira * Mathf.Rad2Deg + 180;
            transform.eulerAngles = new Vector3(0, 0, mira);
            if (Input.GetMouseButton(0) && canFire) {
                CmdFire();
                fireCounter = Bullet.gameObject.GetComponent<Bullet>().delay;
                canFire = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                Dash();
            }

            if (dashing) {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0) {
                    var emission = plasma.emission;
                    emission.rateOverTime = 0;
                    canMove = true;
                    dashing = false;
                }
            }
        }
    }
    [Command]
    void CmdFire() {
        print(mira);
        GameObject b = Instantiate(Bullet);
        b.transform.position = transform.Find("BulletExitPoint").position;
        b.GetComponent<Bullet>().angle = (mira) * Mathf.Deg2Rad;
        //NetworkServer.SpawnWithClientAuthority(b, connectionToClient);
        NetworkServer.Spawn(b);
    }
    void Dash() {
        var emission = plasma.emission;
        rb.velocity = new Vector2(dashSpeed * Mathf.Cos(mira * Mathf.Deg2Rad), dashSpeed * Mathf.Sin(mira * Mathf.Deg2Rad));
        dashing = true;
        emission.rateOverTime = 2000;
        dashTime = dashDuration;
        canMove = false;
    }
    public float GetAim() {

        return mira;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Soul") {
            skill = collision.gameObject.GetComponent<Soul>().skill;
            collision.gameObject.GetComponent<Soul>().AutoDestroy();
        }
    }
}
