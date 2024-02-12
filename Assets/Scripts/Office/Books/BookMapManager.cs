using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMapManager : MonoBehaviour
{
    private Transform[] _children;
    // Start is called before the first frame update
    void Start()
    {
        _children = transform.GetComponentsInChildren<Transform>();
        
        Debug.LogError("Nombre de Game Objects trouvés : " + _children.Length);

        for (int i = 0; i < _children.Length; i++)
        {
            _children[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMapAndLight()
    {
        
        for(int i = 0; i < _children.Length; i++)
        {
            if (_children[i].gameObject.activeSelf)
            {
                _children[i].gameObject.SetActive(false);
            }
            else
            {
                _children[i].gameObject.SetActive(true);
            }
        }
        

    }
}
