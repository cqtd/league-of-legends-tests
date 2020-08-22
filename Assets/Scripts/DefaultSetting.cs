using UnityEngine;

public class DefaultSetting : MonoBehaviour
{
    public int targetFrameRate = 60;
    public KeyCode escapeKey = KeyCode.Escape;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
#if UNITY_EDITOR
	            
#elif UNITY_STANDALONE
            Application.Quit(0);
#endif
        }

        if (Input.GetKey(escapeKey) && Input.GetKey(KeyCode.LeftShift))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
