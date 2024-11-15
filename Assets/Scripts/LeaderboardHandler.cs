using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LeaderboardHandler : MonoBehaviour
{
    public string LBfile = "Assets/Data/leaderboard.csv";

    public GameManager gameManager;

    


    private void Awake()
    {
        

    }
    // Start is called before the first frame update
    void Start()
    {
        //WANT TO BE WRITING FILES IN THE GAMEMANAGER SCRIPT----------
        //ReadFile(LBfile);
        //WriteFile("Isaac", 10, LBfile);
        DontDestroyOnLoad(gameManager);
        DontDestroyOnLoad(this);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadFile(string file)
    {
        if(File.Exists(file))
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string[] header = reader.ReadLine().Split(',');

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(',');

                    string name = fields[0];
                    int score;

                    if(int.TryParse(fields[1], out score))
                    {
                    Debug.Log($"{header[0]}: {name} | {header[1]}: {score}");
                    }
                }
            }
        }
        else
        {
            Debug.Log("File Not Found");
        }
    }

    public void WriteFile(string name, int score, string file)
    {
        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.WriteLine(name + ',' + score);
        }
    }
}
