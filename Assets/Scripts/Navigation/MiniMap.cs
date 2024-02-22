using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    //Enemy Highlighting Mask
    [SerializeField] LayerMask EnemyHighlightingMask;

    //Highlighting Circle (change highlighting circle asset and can change the size of it in the prefab)
    [SerializeField] GameObject HighlightingCircle;

    //the mini map camera
    Camera MiniMapCam;

    //The renderer texture for displaying the mini map camera
    public RenderTexture targetTexture;

    //the image to display the renderer texture
    [SerializeField] RawImage MinimapImage;

    //the multiplier fro detecting enemies (change it as you like according to the map size)
    [SerializeField] float EnemyDetectormultiplier;


    // Start is called before the first frame update
    void Start()
    {
        //refrencing the camera
        MiniMapCam = GetComponent<Camera>();

        //making the target texture of the camera is the renderer texture
        MiniMapCam.targetTexture = targetTexture;

        //assiging the renderer texture to the texture of the image
        MinimapImage.texture = targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        //making a raycast hit the starting point of it is the main camera and the size of it is the orthographicSize of the main camera multiplies by the enemy detection multiplier and the aspect of the main camera times the multiplier and the angle is zero direction is the forward of the main camera the radius is the orthographicSize of the main camera * the aspect and then the enemy layermask
        RaycastHit2D[] Raycast = Physics2D.BoxCastAll(Camera.main.transform.position, new Vector2(Camera.main.orthographicSize * EnemyDetectormultiplier, Camera.main.aspect * Camera.main.orthographicSize) * EnemyDetectormultiplier, 0, Camera.main.transform.forward, Camera.main.orthographicSize * Camera.main.aspect * EnemyDetectormultiplier, EnemyHighlightingMask);

        //then loop all over the deteted castes with our choosen layermask
        foreach (RaycastHit2D hits in Raycast){
            Debug.Log("Detected enemy " + hits.transform.gameObject.name);
            ShowingHighlight(hits);
        }

        
    }

    void ShowingHighlight(RaycastHit2D hits)
    {
        GameObject Highlighting = Instantiate(HighlightingCircle, hits.transform);
        Highlighting.transform.position = hits.transform.position;
        Highlighting.SetActive(true);
    }
}


