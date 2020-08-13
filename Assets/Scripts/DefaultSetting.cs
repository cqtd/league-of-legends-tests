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
            Application.Quit(0);
        }
    }
}
