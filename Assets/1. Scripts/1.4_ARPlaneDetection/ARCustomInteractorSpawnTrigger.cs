using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ARCustomInteractorSpawnTrigger : MonoBehaviour
{
    public enum SpawnTriggerType
    {
        SelectAttempt,
        InputAction,
    }

    [SerializeField] XRRayInteractor m_ARInteractor;
    public XRRayInteractor arInteractor
    {
        get => m_ARInteractor;
        set => m_ARInteractor = value;
    }

    [SerializeField] ObjectSpawner m_ObjectSpawner;
    public ObjectSpawner objectSpawner
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    [SerializeField] bool m_RequireHorizontalUpSurface;

    public bool requireHorizontalUpSurface
    {
        get => m_RequireHorizontalUpSurface;
        set => m_RequireHorizontalUpSurface = value;
    }

    [SerializeField] SpawnTriggerType m_SpawnTriggerType;
    public SpawnTriggerType spawnTriggerType
    {
        get => m_SpawnTriggerType;
        set => m_SpawnTriggerType = value;
    }

    [SerializeField] XRInputButtonReader m_SpawnObjectInput = new XRInputButtonReader("Spawn Object");

    public XRInputButtonReader spawnObjectInput
    {
        get => m_SpawnObjectInput;
        set => XRInputReaderUtility.SetInputProperty(ref m_SpawnObjectInput, value, this);
    }

    [SerializeField] bool m_BlockSpawnWhenInteractorHasSelection = true;

    public bool blockSpawnWhenInteractorHasSelection
    {
        get => m_BlockSpawnWhenInteractorHasSelection;
        set => m_BlockSpawnWhenInteractorHasSelection = value;
    }

    bool m_AttemptSpawn;
    bool m_EverHadSelection;

    [SerializeField] private ARRaycastHitFollower _aRRaycastHitFollower;

    void OnEnable()
    {
        m_SpawnObjectInput.EnableDirectActionIfModeUsed();
        ARRaycastHitFollower.OnTouchInIndicator += SpawnObject;
    }

    void OnDisable()
    {
        m_SpawnObjectInput.DisableDirectActionIfModeUsed();
        ARRaycastHitFollower.OnTouchInIndicator -= SpawnObject;
    }

    void Start()
    {
        if (m_ObjectSpawner == null)
#if UNITY_2023_1_OR_NEWER
            m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
#else
            m_ObjectSpawner = FindObjectOfType<ObjectSpawner>();
#endif

        if (m_ARInteractor == null)
        {
            Debug.LogError("Missing AR Interactor reference, disabling component.", this);
            enabled = false;
        }
    }

    void Update()
    {
        var selectState = m_ARInteractor.logicalSelectState;

        if (m_BlockSpawnWhenInteractorHasSelection)
        {
            if (selectState.wasPerformedThisFrame)
                m_EverHadSelection = m_ARInteractor.hasSelection;
            else if (selectState.active)
                m_EverHadSelection |= m_ARInteractor.hasSelection;
        }

        m_AttemptSpawn = false;
        switch (m_SpawnTriggerType)
        {
            case SpawnTriggerType.SelectAttempt:
                if (selectState.wasCompletedThisFrame)
                    m_AttemptSpawn = !m_ARInteractor.hasSelection && !m_EverHadSelection;
                break;

            case SpawnTriggerType.InputAction:
                if (m_SpawnObjectInput.ReadWasPerformedThisFrame())
                    m_AttemptSpawn = !m_ARInteractor.hasSelection && !m_EverHadSelection;
                break;
        }
    }

    private void SpawnObject()
    {
        // if (m_AttemptSpawn)
        // {
        //     m_AttemptSpawn = false;
        if (_aRRaycastHitFollower.TryToGetCurrentRaycastHitPoint(out var aRRaycastHit))
        {
            if (!(aRRaycastHit.trackable is ARPlane arPlane))
                return;
            m_ObjectSpawner.TrySpawnObject(aRRaycastHit.pose.position, arPlane.normal);
        }
    }
    // }
}
