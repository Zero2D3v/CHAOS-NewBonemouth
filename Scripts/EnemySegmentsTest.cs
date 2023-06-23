using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySegmentsTest : MonoBehaviour
{
    public GameObject go;

    [SerializeField] 
    float frequency;

    public Material material;
    public Color color;
    public EnemyHealthBar ehb;

// private void Awake()
// {
//     
//
// }
    // Start is called before the first frame update
    void Start()
    {
        ehb = GetComponentInParent<EnemyHealthBar>();
        go = this.gameObject;
        material = go.GetComponent<Image>().material;
        frequency = ehb.newSegmentAmount;
        Debug.Log(frequency);

     //Material mat = Instantiate(material);
     //mat.SetFloat("_Frequency", frequency);
     //Debug.Log(frequency);
     //material = mat;
        //material = new Material(material);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //material.color = color;
        //material.SetFloat("_Frequency", frequency);
    }
}
