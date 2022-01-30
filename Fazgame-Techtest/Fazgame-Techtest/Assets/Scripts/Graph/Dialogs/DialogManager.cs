using UnityEngine;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public GameObject currentCamera;
    private Stack<GameObject> activeCameras = new Stack<GameObject>();

    public void ActivateCamera(GameObject cameraObject)
    {
		//DisableUIWindow (currentCamera);
        if(currentCamera != null)
            activeCameras.Push(currentCamera);

        currentCamera = cameraObject;
        Enable(cameraObject);

    }

    public void RestoreMainCamera()
    {
        GameObject previousCamera = currentCamera;
        while (activeCameras.Count > 0)
        {
            if(previousCamera != null)
                Disable(previousCamera);

            previousCamera = activeCameras.Pop();
        }

        currentCamera = previousCamera;
    }

    public void DeactivateCurrentCamera()
    {
        if (activeCameras.Count > 0)
        {
            if(currentCamera != null)
                Disable(currentCamera);

            GameObject previousCamera = activeCameras.Pop();
            currentCamera = previousCamera;
        }
    }

    private static void Disable(GameObject camObj)
    {
		DisableUIWindow (camObj);
    }

    private static void Enable(GameObject camObj)
    {
        //c.enabled = c.GetComponent<tk2dUICamera>().enabled = true;
		EnableUIWindow (camObj);
    }


    public static void DisableUIWindow(GameObject cam)
    {
//		c.enabled = c.GetComponent<tk2dUICamera>().enabled = false;
        if(cam == null)
        {
            Debug.LogWarning("Dialog Manager Warning: No camera was found to disable.");
            return;
        }
		UIWindow window = cam.GetComponent<UIWindow> ();
		if (window != null)
        {
			window.Close();
		}
	}

    public static void EnableUIWindow(GameObject cam)
    {
        //		c.enabled = c.GetComponent<tk2dUICamera>().enabled = false;
        if(cam == null)
        {
            Debug.LogWarning("Dialog Manager Warning: No camera was found to enable.");
            return;
        }
        UIWindow window = cam.GetComponent<UIWindow> ();
		if (window != null) {
			window.Enable();
		}
	}
}