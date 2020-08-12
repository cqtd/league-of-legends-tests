using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
      using UnityEditor;  
#endif

public class AssetFinder : MonoBehaviour
{
    // Start is called before the first frame update
    
    [ContextMenu("Find Asset")]
    void Start()
    {
        var jinx = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(jinx);
        
#if UNITY_EDITOR
	    Debug.Log(PrefabUtility.IsAnyPrefabInstanceRoot(jinx));
	    Debug.Log(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(jinx));
	    Debug.Log(PrefabUtility.GetPrefabAssetType(jinx));
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int[] a = new [] {1, 4, 3, 2, 6, 2, 7, 5};
    
    void d()
    {
	    bool match = false;
	    for (int i = 0; i < 8; i++)
	    {
		    for (int j = 0; j < 8; j++)
		    {
			    if (a[i] != a[j]) continue;
			    
			    for (int k = 0; k < 8; k++)
			    {
				    if (a[j] == a[k])
				    {
					    match = true;
					    break;
				    }
			    }
		    }
	    }
    }
}
