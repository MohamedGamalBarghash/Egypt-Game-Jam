using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingCommunication : MonoBehaviour
{
    [SerializeField] KeyCode PingButton;

    //Drag and Drop Higlight Circle in the prefabs folder to this Variable
    [SerializeField] GameObject HighlightCircle;

    int PingCount = 1;


    /// <summary>
    /// put this script on the players 
    /// choose the keycode for every player to Ping


    // Update is called once per frame
    void Update(){
        //checks if the Chosen ping button is pressed
        if (Input.GetKeyDown(PingButton)){

            //see if the player have enough tries at a time to ping
            if(PingCount >= 1) {
                //then it ping's as showing a highlight circle above the player
                ShowHighlight();

                //then it decreases the counts for pinging
                PingCount -= 1;
            }

            else{
                //if the player don't have enough Ping Countes it will cal a ienumrator funtion to relaod his ping counts;
                StartCoroutine(RelaodHighlight());
            }


        }
        //Just Debuging the remaining ping counts 
        Debug.Log(gameObject.name + "Count is " + PingCount);
    }


    void ShowHighlight(){
        //showing the highlight throigh making a copy of this highlight prefab

        GameObject Highlight = Instantiate(HighlightCircle, transform );
        //then makes the player that pressed the ping button a parent

        Highlight.transform.position = transform.position;
        //then it showes the ping highlight

        Highlight.SetActive(true);

        //after 3 seconds it will destory the copy that ping higlight prefab
        Destroy(Highlight, 3);

        return;

    }

    IEnumerator RelaodHighlight()
    {
        //after 4 seconds it will reload the ping count
        yield return new WaitForSeconds(4);
        PingCount = 1;
    }

}
