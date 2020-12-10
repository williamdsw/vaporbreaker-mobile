using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private int startingSceneIndex;
    private static PersistentData instance;

    public static PersistentData Instance { get => instance; }

    private void Awake()
    {
        SetupSingleton();
    }

    private void Start()
    {
        startingSceneIndex = SceneManagerController.GetActiveSceneIndex();
    }

    private void Update()
    {
        int currentSceneIndex = SceneManagerController.GetActiveSceneIndex();
        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(this.gameObject);
        }
    }

    private void SetupSingleton()
    {
        int numberOfInstances = FindObjectsOfType(GetType()).Length;
        if (numberOfInstances > 1)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void DestroyInstance()
    {
        DestroyImmediate(this.gameObject);
    }
}