using System;
using System.IO;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public String HighScore { get; private set; }
    
    private const String DirectoryName = "Data";
    private const String FileName = "score.txt";
    private String Path = DirectoryName + "/" + FileName;

    private void Start()
    {
        Path = Application.dataPath + "/" + Path;
    }

    public void LoadHighScore()
    {
        CheckPath();
        if (IsFileEmpty())
            HighScore = "00:32:10:00";
        else
            HighScore = File.ReadAllLines(Path)[0];
    }

    public void SaveHighScore(String score)
    {
        if (Int64.Parse(HighScore.Replace(":", "")) > Int64.Parse(score.Replace(":", "")))
        {
            Debug.Log("aaa");
            File.WriteAllLines(Path, new []{score});
            HighScore = score;
        }
    }

    private bool IsFileEmpty()
    {
        return new FileInfo(Path).Length == 0;
    }
    
    private void CheckPath()
    {
        if (!Directory.Exists(Application.dataPath + "/" + DirectoryName))
            Directory.CreateDirectory(Application.dataPath + "/" + DirectoryName);
        if (!File.Exists(Path))
            File.Create(Path);
    }
}
