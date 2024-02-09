using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//URL : https://www.youtube.com/watch?v=lBzwUKQ3tbw

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureDetection : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    public float threshold = 0.1f;

    private List<OVRBone> fingerBones;
    //private Gesture previousGesture;

    // Start is called before the first frame update
    void Start()
    {
        //Populating the list with all bones of the hand
        fingerBones = new List<OVRBone>(skeleton.Bones);

        //previousGesture = new Gesture();
    }

    // Update is called once per frame
    void Update()
    {
        //Initializing new gestures to feed the list
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            SaveGesture();
        }

        /*
        //Recognizing gesture or not
        Gesture currentGesture = RecognizeGesture();
        bool hasRecognized = !currentGesture.Equals(new Gesture());

        //Check if new gesture
        if(hasRecognized && !currentGesture.Equals(previousGesture))
        {
            //new gesture found and add it to the list
            Debug.Log("New Gesture Found : " + currentGesture.name);
            previousGesture = currentGesture;
            currentGesture.onRecognized.Invoke();
        }
        */
    }

    void SaveGesture()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            //finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture RecognizeGesture()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;

            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                if(distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }

        return currentGesture;
    }

    IEnumerator InitializingFingerBones()
    {
        yield return new WaitForSeconds(5f);
        if(skeleton.Bones.Count > 0)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            Debug.Log("FingerBones Count : " + fingerBones.Count);
        }
        else
        {
            Debug.LogError("Finger Bones not initialized.");
        }
    }
}
