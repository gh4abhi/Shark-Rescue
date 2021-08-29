using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket_Controller : MonoBehaviour
{
    // todo - We have to fix a lighting bug. 
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 100f;
    [SerializeField]
    private AudioClip mainEngine;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip successSound;
    [SerializeField]
    private ParticleSystem mainEngineParticles;
    [SerializeField]
    private ParticleSystem hitParticles;
    [SerializeField]
    private ParticleSystem successParticles;
    [SerializeField]
    float levelLoadDelay = 2f;

    enum State { Alive, Dying, Transcending };

    State state = State.Alive;

    public bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            if (Debug.isDebugBuild)
            {
                RespondToDebugKeys();
            }
        }
    }


    // todo fix sound bug.

    void OnCollisionEnter(Collision collision)
    {
        if(state!=State.Alive || collisionsDisabled) // Ignore collsions when dead.
        {
            return;
        }
         switch(collision.gameObject.tag)
        {
            case "Safe":
                print("Ok");
                break;
            case "Obstacles":
                StartDeathSequence();
                break;
            case "Fuel" :
                print("Fuel Up");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                print("Not Known");
                break;
        }
    }

    void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(hitSound);
        print("No Fuel");
        hitParticles.Play();
        Invoke("LoadLoseScene", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        print("You Win");
        state = State.Transcending;
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex==SceneManager.sceneCountInBuildSettings-2)
        {
            nextSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        SceneManager.LoadScene(nextSceneIndex); // todo for more than 1 level.
    }
    void LoadLoseScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-2); // todo for more than 1 level.
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))      // We can thrust while rotating.                
        {
            mainEngineParticles.Play();
            ApplyThrust();
        }
        else
        {
            mainEngineParticles.Stop();
            audioSource.Stop();
        }
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        //rigidBody.freezeRotation = true;
        rigidBody.angularVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationThisFrame);
        }
        //rigidBody.freezeRotation = false;
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

}
