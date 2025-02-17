using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesMy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.Invoke("DesMy1", 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DesMy1()
    {
        Destroy(this.gameObject);
    }
}
