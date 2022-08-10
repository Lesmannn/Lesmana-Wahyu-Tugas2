using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

	public delegate void PlayerDelegate ();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;


	public float tapForce=10;
	public float tiltSmooth=5;
	public Vector3 startPos;

	public AudioSource tapAudio;
	public AudioSource scoreAudio;
	public AudioSource dieAudio;



	Rigidbody2D rigid;
	Quaternion downrotation;
	Quaternion forwardrotation;

	GameManager game;


	void Start(){
		rigid = GetComponent<Rigidbody2D> ();
		downrotation = Quaternion.Euler (0, 0, -90);
		forwardrotation = Quaternion.Euler (0, 0, 35);
		game = GameManager.Instance;
		rigid.simulated = false;
	}

	void OnEnable(){
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable(){
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	
	}

	void OnGameStarted(){
		rigid.velocity = Vector3.zero;
		rigid.simulated = true;

	}
	void OnGameOverConfirmed(){
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
	}
	void Update(){
		if (game.GameOver) {
			rigid.simulated = false;
			return;
		}
			
		if(Input.GetMouseButtonDown(0))
			{
			rigid.simulated = true;
			tapAudio.Play ();
			transform.rotation = forwardrotation;
			rigid.velocity = Vector3.zero;
			rigid.AddForce (Vector2.up * tapForce, ForceMode2D.Force);

			}

		transform.rotation = Quaternion.Lerp (transform.rotation, downrotation, tiltSmooth * Time.deltaTime);
	
	
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "ScoreZone") {
		
			OnPlayerScored ();
			scoreAudio.Play();
		}

		if (col.gameObject.tag == "DeadZone") {
		
			rigid.simulated = false;
			OnPlayerDied ();
			dieAudio.Play();
		}
	
	
	
	}

}
