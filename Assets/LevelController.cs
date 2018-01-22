using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    // Use this for initialization
    public int hallWidth; // width of hallway in tiles
    public int hallLength;
    public Tile[][] levelTiles;
    
    public GameObject coverPrefab;
    public GameObject tilePrefab;
    public GameObject playerPrefab;
    
    private bool playerSpawned = false;

	void Start () {
        //GenerateLevel(hallWidth, hallLength);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private Tile[][] GenerateLevel(int hallwidth, int hallLength) {
        Tile[][] levelTiles = new Tile[hallwidth][];
        // [0,0][1,0] // -> +X
        // [0,1][1,1] // ^ -Y

        for (int j = 0; j < hallLength; j++)
        {
            for (int i = 0; i < hallwidth + 2; i++)
            {
                GameObject currentTile = Instantiate(tilePrefab) as GameObject;
                currentTile.GetComponent<Tile>().position = new Vector2(i, j);
                currentTile.transform.parent = this.gameObject.transform;
                currentTile.transform.position = new Vector3(10 * i, 10 * -j);
                
                int coverRoll = Random.Range(0, 11);

                //Ensure we have walls!
                if (i == 0 || i == hallWidth + 1) {
                    coverRoll = 10;
                }

                if (coverRoll > 9)
                {
                    GameObject currentCover = Instantiate(coverPrefab) as GameObject;
                    currentCover.transform.parent = currentTile.transform;
                    currentCover.transform.position = currentTile.transform.position + new Vector3(0, 0, -currentCover.transform.localScale.z / 2);
                }
                else {
                    if(playerSpawned == false)
                    {
                        GameObject playerObj = Instantiate(playerPrefab) as GameObject;
                        playerObj.transform.position = currentTile.transform.position + new Vector3(0,0,-3.75f);
                        playerSpawned = true;
                    }
                }
            }
        }

        return levelTiles;
    }
}