using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class fly : MonoBehaviour
{
	
	public float speed { get; set; }
	public float acceleration { get; set; }
	public float minSpeed = 0;
	public float maxSpeed = 20;
	public float fallingSpeed;
	
	public float score = 0;
    public string time;

    private bool engine = false;


    public ParticleSystem Proba;
    
    

	//updated
	public GUIStyle lebelStyle;
	public RaycastHit hit;
	public float altitude;

	
	//timer

	float timeLimit= 300.0f;// ovo je trajanje nivoa igre, otprilike kao r.kelly - i believe 
//	float timeTaken = 0;
//	float timeReduce = 0;
//	float timeShow = 0;
//	float startTime= 0;

	//pause
	public GUIText pauseText;

	public GUIStyle pauseStyle;


	//gameOver
	private bool gameOver;
	public GUIStyle gameOverStyle;
	public GUIText gameOverText;


	//finish
	private bool finish;
	public GUIText finishText;
	public GUIStyle finishStyle;

	//restart
	private bool restart;
	public GUIText restartText;
	public GUIStyle restartStyle;
	
	
	public GUIText resultText;
	public GUIStyle resultStyle;
	
	
	public GUIText timeText;
	public GUIStyle timeStyle;


    private Propeller propeller;

	 

	// Initialization
	void Start()
	{
		
		speed = 0;
		acceleration = 0;
		fallingSpeed = 0;

	    Proba.Pause();

        propeller = GameObject.Find("propeler").gameObject.GetComponent<Propeller>();
	    
		//update
		gameOver=false;
		gameOverText.text = "";

		finish = false;
		finishText.text = "";

		restart = false;
		restartText.text = "";
		
		
		resultText.text = "";
		
		timeText.text="";

		pauseText.text = "";
	}

	// Called once per frame
	void Update()
	{
	    time = Time.timeSinceLevelLoad.ToString("##.## 'sec'");

		if(restart)
		{
			gameOverText.text = "";

			if(Input.GetKeyDown(KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevel); 
			}
		}

		if (Time.timeSinceLevelLoad >= timeLimit){
			GameOver();
			//Application.Quit();
		}
			
		
		if (Input.GetAxis("Horizontal") < 0)
		{
			if (acceleration > 5)
				transform.Rotate(0, 0, 20 * Time.deltaTime);
			
		}
		
		
		if (Input.GetAxis("Horizontal") > 0)
		{
			if (acceleration > 5)
				transform.Rotate(0, 0, -20 * Time.deltaTime);
			
		}
		
		if (Input.GetAxis("Vertical") < 0)
		{
			if (acceleration > 7)
				transform.Rotate(-20 * Time.deltaTime, 0, 0);
			
		}

        if (Input.GetKey(KeyCode.A) == true)
		{
			transform.Rotate(0, -20 * Time.deltaTime, 0);
			transform.Rotate(0, 0, 10 * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.S) == true)
		{
			transform.Rotate(0, 20 * Time.deltaTime, 0);
			transform.Rotate(0, 0, -10 * Time.deltaTime);
		}
		
		if (Input.GetAxis("Vertical") > 0)
		{
			if (acceleration > 7)
				transform.Rotate(20 * Time.deltaTime, 0, 0);
		}

        if (speed < 3)
        {

            if (transform.position.y > 1)
            {
                fallingSpeed = fallingSpeed + (float)0.5;
                transform.Rotate(fallingSpeed * Time.deltaTime, 0, 0);
                speed = speed + (float)0.2;
            }
        }

	    if (Input.GetKeyDown("r"))
	    {
	        if (engine)
	        {
                engine = false;
	            while (propeller.GetPropelerSpeed() > 0)
	            {
                    propeller.TurnOfEngine();
	            }
	            
	        }
	        else
	        {
	           propeller.TurnOnEngine();
               Proba.Play();
               engine = true; 
	        }
            
	    }

	    if (engine)
	    {
            if (Input.GetKey("="))
            {
                if (speed < maxSpeed)
                {
                    speed += (float)0.2;
                    Proba.Play();
                    propeller.SpeedUp();
                }
            }
	    }

	    if (engine)
	    {
            if (Input.GetKey("-"))
            {
                if (speed > minSpeed)
                {
                    speed -= (float)0.2;
                    propeller.SpeedDown();
                }
            }
	    }
		
		
		transform.Translate(0, 0, acceleration * Time.deltaTime);

        if (speed > 0)
        {
            if (acceleration <= speed)
                acceleration = acceleration + (float)0.05;
                
           
                
        }
		
		if (acceleration > speed)
		{
			acceleration = acceleration - (float)0.05;
			if (acceleration <= 2)
			{
				acceleration = acceleration - (float)0.005;
				if (acceleration <= 0)
				{
					acceleration = 0;
				}
			}
		}

	    

		//updated update
		if (Physics.Raycast (transform.localPosition, transform.TransformDirection (Vector3.down), out hit))
		{ 
			altitude= hit.distance; 
		}
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
			gameOver = true;
			//UnityEditor.EditorApplication.isPlaying = false;
		}

		if(finish || gameOver)
		{
			Restart();
		}


		//this is to pause a game
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Time.timeScale == 1)
			{
				Time.timeScale = 0;
				pauseText.text = "Paused!";

			}
			else
			{
				Time.timeScale = 1;
				pauseText.text = "";
			}
		}


			
	}
	public GameObject explosion;



	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "sky_fire_ring")
		{
			Destroy(other.gameObject);
			score = score + 10;
		}

		if (other.gameObject.name == "sky_ring")
		{
			Destroy(other.gameObject);
			score = score + 5;
			//			yield return new WaitForSeconds(0.4);   

		} 
		if (other.gameObject.name == "sky_ring_on_fire")
		{
			Destroy(other.gameObject);
			score = score + 20;
		}

	    if (other.gameObject.name == "DestroyRing")
	    {
			GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			//Destroy(GameObject.FindWithTag("Player"));
			//Destroy(gameObject); //
			//Destroy(expl, 3); 

	        Restart();
			//Destroy(GameObject.FindWithTag("Player"));

	    }

        if (other.gameObject.name == "Terrain")
        {
			GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			//Destroy(gameObject); 
			//Destroy(expl, 3); 
            Restart();
        }

        

        
		if (other.gameObject.name == "race_gate")
		{
			Destroy(other.gameObject);
			score = score + 30;	
			Finish();
		}


	}

	//updated update
	void OnGUI()
	{
		
		GUI.Box(new Rect(10,10,150,120),"Flying data");
		GUI.Label(new Rect(15, 30, 100, 20), "Speed: " + (acceleration * 20).ToString("##.## 'km/h'"));
		GUI.Label(new Rect(15, 50, 100, 20), "Score: " + score.ToString("0000"));
        GUI.Label(new Rect(15, 70, 100, 20), "Altitude: "+((rigidbody.position.y)*5).ToString("##.## 'm'"));
		GUI.Label(new Rect(15, 90, 100, 20), "Time: " + time);
		// xMin - left, xMax - rite, yMin - top,  yMax - bottom
		/*GUI.Label(new Rect(30, 40, 50, 25), "Score: ", lebelStyle);
		GUI.Label(new Rect(65, 70, 50, 25), score.ToString(), lebelStyle);

		GUI.Label(new Rect(95, 100, 50, 25), "Speed: ", lebelStyle);
		GUI.Label(new Rect(135, 140, 50, 25), (speed*acceleration).ToString(), lebelStyle);

		GUI.Label(new Rect(175, 180, 50, 25), "Altitude: ", lebelStyle);
		GUI.Label(new Rect(205, 220, 50, 25), (altitude-0.42).ToString(), lebelStyle);

		GUI.Label(new Rect(230, 250, 50, 25), "Time: ", lebelStyle);
		GUI.Label(new Rect(270, 280, 50, 25), Time.timeSinceLevelLoad.ToString(), lebelStyle);*/
		//

		//Ending text options
		GUI.Label(new Rect(550, 220, 100, 50), gameOverText.text, gameOverStyle);
		GUI.Label(new Rect(550,220,100,50), finishText.text, finishStyle);
		GUI.Label(new Rect(450, 110, 50, 70), restartText.text, restartStyle);
		GUI.Label(new Rect(520, 10, 100, 50), resultText.text, resultStyle);
		GUI.Label(new Rect(500, 50, 100, 50), timeText.text, timeStyle);
		//Pause text option
		GUI.Label(new Rect(550, 220, 100, 50), pauseText.text, pauseStyle);
	}
	public void GameOver()
	{
		gameOver = true;
		//if it has finished but player still didn't react
		if(finish)
			gameOverText.text = "";
		else 
			gameOverText.text = "Game over!";
		//Application.Quit();
		//UnityEditor.EditorApplication.isPlaying = false;
		speed = 0;
		acceleration = 0;
		maxSpeed = 0;
	}
	public void Finish()
	{
		finish = true;
		if(restart)
			finishText.text = "";
		else
		{
			finishText.text = "Finished!";
			resultText.text = "Your score: "+score.ToString("0000");
			timeText.text = "Your time: "+Time.timeSinceLevelLoad.ToString("##.## 'sec'");
			speed = 0;
			acceleration = 0;
			maxSpeed = 0;
            time = Time.timeSinceLevelLoad.ToString("##.## 'sec'");
		}
		
	}
	public void Restart(){
//		yield return new WaitForSeconds(2);
		restart = true;
        speed = 0;
        acceleration = 0;
        maxSpeed = 0;
        while (propeller.GetPropelerSpeed() > 0)
        {
            propeller.TurnOfEngine();
        }
		//if(finish)
		if(finish)
			restartText.text="Press 'R' to restart the game!";

		else
			restartText.text = "Press 'R' to restart the game!";
		//else if(gameOver)
		//	restartText.text = "Press twice 'Ctrl + P' for restart!";

	}


	
}
