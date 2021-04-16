using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to upload games information to a server that is then stored in a mysql database
 */

public class wwwFormGameData : MonoBehaviour
{
    [SerializeField] private string apiURL = "http://localhost:5000/api/gamedata"; // url where the server is listening
    private LevelComplete levelComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        levelComplete = GetComponent<LevelComplete>();
    }

    public IEnumerator uploadData()
    {
        WWWForm form = new WWWForm();
        
        // add fields to the form that is going ot be uploaded. Current level, time taken to complete level, total number of deaths, cumber of commands used to pass the level
        form.AddField("nivel",levelComplete.levelName);
        form.AddField("tiempo", levelComplete.formatedTime);
        form.AddField("muertes", levelComplete.commandExecuter.numberDeaths);
        form.AddField("comandosUsados", levelComplete.commandExecuter.numberCommands);
        
        Debug.Log(form);

        using (UnityWebRequest request = UnityWebRequest.Post(apiURL, form)) // send web request and handle errors
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }
}
