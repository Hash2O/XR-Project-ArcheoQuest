using UnityEngine;

public class TerrainRealTimeAlteration : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObject;

    public enum DeformMode { RaiseLower, Flatten, Smooth }
    DeformMode deformMode = DeformMode.RaiseLower;

    public Terrain terrain;
    public Texture2D deformTexture;
    public float strength = 1;
    public float area = 1;
    public bool showHelp;

    Transform buildTarget;
    Vector3 buildTargPos;
    Light spotLight;

    //GUI
    Rect windowRect = new Rect(10, 10, 400, 185);
    bool onWindow = false;
    bool onTerrain;
    Texture2D newTex;
    float strengthSave;

    //Raycast
    private RaycastHit hit;

    //Deformation variables
    private int xRes;
    private int yRes;
    private float[,] saved;
    float flattenTarget = 0;
    Color[] craterData;

    TerrainData tData;

    float strengthNormalized
    {
        get
        {
            return (strength) / 9.0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Create build target object
        GameObject tmpObj = new GameObject("BuildTarget");
        buildTarget = tmpObj.transform;

        //Add Spot Light to build target
        GameObject spotLightObj = new GameObject("SpotLight");
        spotLightObj.transform.SetParent(buildTarget);
        spotLightObj.transform.localPosition = new Vector3(0, 2, 0);
        spotLightObj.transform.localEulerAngles = new Vector3(90, 0, 0);
        spotLight = spotLightObj.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        spotLight.range = 20;

        tData = terrain.terrainData;
        if (tData)
        {
            //Save original height data
            xRes = tData.heightmapResolution;
            yRes = tData.heightmapResolution;
            saved = tData.GetHeights(0, 0, xRes, yRes);
        }

        //Change terrain layer to UI
        terrain.gameObject.layer = 5;
        strength = 2;
        area = 2;
        BrushScaling();
    }

    void FixedUpdate()
    {
        AltRaycastHit();
        WheelValuesControl();

        if (onTerrain && !onWindow)
        {
            TerrainDeform();
        }

        //Update Spot Light Angle according to the Area value
        spotLight.spotAngle = area * 25f;
    }

    //Raycast
    //______________________________________________________________________________________________________________________________
    void AltRaycastHit()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = new RaycastHit();
        //Do Raycast hit only against UI layer
        if (Physics.Raycast(_gameObject.transform.position, _gameObject.transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Raycasting GameObject Did Hit " + hit.transform.name);
            onTerrain = true;
            if (buildTarget)
            {
                buildTarget.position = Vector3.Lerp(buildTarget.position, hit.point + new Vector3(0, 1, 0), Time.time);
            }
        }
        else
        {
            if (buildTarget)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                buildTarget.position = curPosition;
                onTerrain = false;
            }
        }
    }

    //TerrainDeformation
    //___________________________________________________________________________________________________________________
    void TerrainDeform()
    {
        buildTargPos = buildTarget.position - terrain.GetPosition();
        float x = Mathf.Clamp01(buildTargPos.x / tData.size.x);
        float y = Mathf.Clamp01(buildTargPos.z / tData.size.z);
        flattenTarget = tData.GetInterpolatedHeight(x, y) / tData.heightmapScale.y;

        buildTargPos = buildTarget.position - terrain.GetPosition();

        strengthSave = strength;

        if (newTex && tData && craterData != null)
        {
            int w = (int)Mathf.Lerp(0, xRes, Mathf.InverseLerp(0, tData.size.x, buildTargPos.x));
            int z = (int)Mathf.Lerp(0, yRes, Mathf.InverseLerp(0, tData.size.z, buildTargPos.z));
            w = Mathf.Clamp(w, newTex.width / 2, xRes - newTex.width / 2);
            z = Mathf.Clamp(z, newTex.height / 2, yRes - newTex.height / 2);
            int startX = w - newTex.width / 2;
            int startY = z - newTex.height / 2;
            float[,] areaT = tData.GetHeights(startX, startY, newTex.width, newTex.height);
            for (int i = 0; i < newTex.height; i++)
            {
                for (int j = 0; j < newTex.width; j++)
                {
                    if (deformMode == DeformMode.RaiseLower)
                    {
                        areaT[i, j] = areaT[i, j] - craterData[i * newTex.width + j].a * strengthSave / 15000;
                    }
                    else if (deformMode == DeformMode.Flatten)
                    {
                        areaT[i, j] = Mathf.Lerp(areaT[i, j], flattenTarget, craterData[i * newTex.width + j].a * strengthNormalized);
                    }
                    else if (deformMode == DeformMode.Smooth)
                    {
                        if (i == 0 || i == newTex.height - 1 || j == 0 || j == newTex.width - 1)
                            continue;

                        float heightSum = 0;
                        for (int ySub = -1; ySub <= 1; ySub++)
                        {
                            for (int xSub = -1; xSub <= 1; xSub++)
                            {
                                heightSum += areaT[i + ySub, j + xSub];
                            }
                        }

                        areaT[i, j] = Mathf.Lerp(areaT[i, j], (heightSum / 9), craterData[i * newTex.width + j].a * strengthNormalized);
                    }
                }
            }
            tData.SetHeights(w - newTex.width / 2, z - newTex.height / 2, areaT);

        }
    }

    void BrushScaling()
    {
        //Apply current deform texture resolution 
        newTex = Instantiate(deformTexture) as Texture2D;
        TextureScale.Point(newTex, deformTexture.width * (int)area / 10, deformTexture.height * (int)area / 10);
        newTex.Apply();
        craterData = newTex.GetPixels();
    }

    void WheelValuesControl()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(mouseWheel) > 0.0)
        {
            if (mouseWheel > 0.0)
            {
                //More
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if (area < 13)
                    {
                        area += 0.5f;
                    }
                    else
                    {
                        area = 13;
                    }
                }
                else
                {
                    if (strength < 13)
                    {
                        strength += 0.5f;
                    }
                    else
                    {
                        strength = 13;
                    }
                }
            }
            else if (mouseWheel < 0.0)
            {
                //Less
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if (area > 1)
                    {
                        area -= 0.5f;
                    }
                    else
                    {
                        area = 1;
                    }
                }
                else
                {
                    if (strength > 1)
                    {
                        strength -= 0.5f;
                    }
                    else
                    {
                        strength = 1;
                    }
                }
            }
            if (area > 1) BrushScaling();
        }
    }

    void OnApplicationQuit()
    {
        //Reset terrain height when exiting play mode
        tData.SetHeights(0, 0, saved);
    }
}
