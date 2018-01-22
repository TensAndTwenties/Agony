using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // Use this for initialization

    public Vector2 position { get; set; }
    Cover cover;
    public const float tileLength = 1.0f;
    public GameObject occupantCharacter { get;  set;} //one character per tile


    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
