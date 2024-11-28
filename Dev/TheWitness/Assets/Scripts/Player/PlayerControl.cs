using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;
using System.Data;

// TODO : Swich to new inputs system

public class PlayerControl : MonoBehaviour
{
    #region Properties

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5.2f;
    [SerializeField] private Transform orientation;

    private PlayerInputs playerInputAction;
    private CameraControl cameraControl;
    private Vector2 inputs;
    private Rigidbody rb;
    private float speed;

    private RaycastHit? objectInSight;
    private ICollectable objectInHand;
    private bool interacting = false;

    //Inventory
    private Inventory inventory;

    //Camera
    private HeadBobbing headBobbing;

    //Audio
    private EventInstance PlayerFootSteps;
    private EventInstance PlayerFootStepsSprint;

    #endregion

    #region Getters & Setters

    public Transform Orientation { get => orientation; }
    public PlayerInputs InputAction { get => playerInputAction; }
    public CameraControl CameraControl { get => cameraControl; }
    public Inventory Inventory { get => inventory; }
    public bool Interacting { get => interacting; }

    #endregion

    #region Methods

    private void OnEnable()
    {
        playerInputAction.InGame.Enable();

        playerInputAction.InGame.Sprint.started += (context) => speed = runSpeed;
        playerInputAction.InGame.Sprint.canceled += (context) => speed = walkSpeed;

        playerInputAction.InGame.Interact.started += StartInteract;
        playerInputAction.InGame.Return.started += StopInteract;
    }

    private void OnDisable()
    {
        playerInputAction.InGame.Disable();
    }

    private void Awake()
    {
        playerInputAction = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        cameraControl = GetComponentInChildren<CameraControl>();
        speed = walkSpeed;

        // TODO : Move into gamemanager
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventory = new Inventory();
        headBobbing = GetComponentInChildren<HeadBobbing>();
    }

    private void Start()
    {



        // Vérifier si l'AudioManager est présent
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager is missing in the scene. No SoundPlayed.");
            return;
        }

        //audio
        PlayerFootSteps = AudioManager.instance.CreateInstance(FmodEvents.instance.PlayerFootSteps);
        PlayerFootStepsSprint = AudioManager.instance.CreateInstance(FmodEvents.instance.PlayerFootStepsSprint);

    }

    private void Update()
    {
        if (objectInHand != null)
        {
            objectInHand.Show();
        }

        // Movement & Detection
        if (interacting)
            return;

        SetInputs();

        objectInSight = CheckInSight();
    }

    private void FixedUpdate()
    {
        if (interacting)
            return;
        Move();

        PlayerFootSteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
        PlayerFootStepsSprint.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));

        UpdateSound();
    }


    private void SetInputs()
    {
        inputs = playerInputAction.InGame.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.velocity = (orientation.forward * inputs.y + orientation.right * inputs.x).normalized * speed;
    }

    private RaycastHit? CheckInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f))
        {
            return hit;
        }

        return null;
    }

    private void StartInteract(InputAction.CallbackContext _context)
    {
        // Stop camera + movement
        // Start object interaction

        if (interacting)
            return;

        rb.velocity = Vector3.zero;

        if (objectInSight == null) return;
        GameObject obj = objectInSight.Value.collider.gameObject;
        obj.TryGetComponent(out objectInHand);
        if (objectInHand == null)
        {
            Debug.LogWarning("[Interaction] NULL object");
        }
        else
        {
            objectInHand.Pickup(this);

            interacting = true;

            StopFootstepSounds();
        }

        obj.TryGetComponent(out IInteractable interactable);
        if (interactable == null)
        {
            Debug.LogWarning("[Interaction] NULL object");
        } 
        else
        {
            interactable.Interact();
        }

    }

    private void StopInteract(InputAction.CallbackContext _context)
    {
        // Resume camera + movement
        // Stop object interaction
        // If object is collectable store it

        if (!interacting)
            return;

        if (objectInHand == null)
        {
            Debug.LogWarning("[Interaction] NULL object");
            return;
        }
        objectInHand.Store();
        objectInHand = null;

        interacting = false;
    }

    private void UpdateSound()
    {
        // Si la vitesse du joueur est supérieure à un certain seuil, commencez à jouer le son des pas
        if (rb.velocity.magnitude > 0.1f && speed == walkSpeed)
        {
            // Vérifiez l'état de lecture actuel de l'événement
            PLAYBACK_STATE playBakState;
            PlayerFootSteps.getPlaybackState(out playBakState);

            // Démarre le son uniquement si l'événement est arrêté
            if (playBakState == PLAYBACK_STATE.STOPPED)
            {
                PlayerFootSteps.start();
                PlayerFootStepsSprint.stop(STOP_MODE.ALLOWFADEOUT);
            }

            headBobbing.SetModifier(5f, 0.6f, 4f);
        }
        else if (rb.velocity.magnitude <= 0)
        {
            // Si la vitesse est faible, arrêtez l'événement
            PLAYBACK_STATE playBakState;
            PlayerFootSteps.getPlaybackState(out playBakState);

            // Arrête le son uniquement s'il est en cours
            if (playBakState == PLAYBACK_STATE.PLAYING)
            {
                PlayerFootSteps.stop(STOP_MODE.ALLOWFADEOUT);
            }

            headBobbing.SetModifier(1f, 1f, 1f);
        }

        // Si la vitesse du joueur est supérieure à un certain seuil, commencez à jouer le son des pas
        if (rb.velocity.magnitude > 0.1f && speed == runSpeed)
        {
            // Vérifiez l'état de lecture actuel de l'événement
            PLAYBACK_STATE playBakStateSprint;
            PlayerFootStepsSprint.getPlaybackState(out playBakStateSprint);

            // Démarre le son uniquement si l'événement est arrêté
            if (playBakStateSprint == PLAYBACK_STATE.STOPPED)
            {
                PlayerFootStepsSprint.start();
                PlayerFootSteps.stop(STOP_MODE.ALLOWFADEOUT);
            }

            headBobbing.SetModifier(10f, 0.6f, 5f);
        }
        else if (rb.velocity.magnitude <= 0)
        {
            // Si la vitesse est faible, arrêtez l'événement
            PLAYBACK_STATE playBakStateSprint;
            PlayerFootStepsSprint.getPlaybackState(out playBakStateSprint);

            // Arrête le son uniquement s'il est en cours
            if (playBakStateSprint == PLAYBACK_STATE.PLAYING)
            {
                PlayerFootStepsSprint.stop(STOP_MODE.ALLOWFADEOUT);
            }

            headBobbing.SetModifier(1f, 1f, 1f);
        }
    }

    // Ajoutez une méthode pour stopper les sons des pas
    private void StopFootstepSounds()
    {
        PLAYBACK_STATE playBakState;

        // Arrête le son des pas de marche si il est en train de jouer
        PlayerFootSteps.getPlaybackState(out playBakState);
        if (playBakState == PLAYBACK_STATE.PLAYING)
        {
            PlayerFootSteps.stop(STOP_MODE.IMMEDIATE); // Ou STOP_MODE.ALLOWFADEOUT si vous préférez un fondu
            Debug.Log("Stopping Walk Footsteps");
        }

        // Arrête le son des pas de sprint si il est en train de jouer
        PlayerFootStepsSprint.getPlaybackState(out playBakState);
        if (playBakState == PLAYBACK_STATE.PLAYING)
        {
            PlayerFootStepsSprint.stop(STOP_MODE.IMMEDIATE); // Ou STOP_MODE.ALLOWFADEOUT si vous préférez un fondu
            Debug.Log("Stopping Sprint Footsteps");
        }
    }


#if UNITY_EDITOR

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Speed : " + rb.velocity.magnitude.ToString("F2"));
        GUI.Label(new Rect(10, 30, 200, 20), "Aiming at : " + (objectInSight != null ? objectInSight.Value.collider.name : "None"));
        GUI.Label(new Rect(10, 50, 200, 20), "Pick-up : " + "No");
    }

#endif

    #endregion

}
