using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class wwwFormGameData : MonoBehaviour
{
    [SerializeField] private string apiURL = "http://localhost:5000/api/gamedata";
    private LevelComplete levelComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        levelComplete = GetComponent<LevelComplete>();
    }

    public IEnumerator uploadData()
    {
        WWWForm form = new WWWForm();
        
        form.AddField("nivel",levelComplete.levelName);
        form.AddField("tiempo", levelComplete.formatedTime);
        form.AddField("muertes", levelComplete.commandExecuter.numberDeaths);
        form.AddField("comandosUsados", levelComplete.commandExecuter.numberCommands);
        
        Debug.Log(form);

        using (UnityWebRequest request = UnityWebRequest.Post(apiURL, form))
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
