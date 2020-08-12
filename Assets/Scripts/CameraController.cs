using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    public int initialCamera = 0;

    int current = 0;

    void Awake()
    {
        SetCamera(initialCamera);
    }

    public void NextCamera()
    {
        int next = (current + 1) % cameras.Length;
        current = next;
        
        SetCamera(next);
    }

    void SetCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(index == i);
        }
    }
}
