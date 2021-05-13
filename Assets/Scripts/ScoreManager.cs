using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : Singleton<ScoreManager>
{
    public String HighScore { get; private set; }
    
    private const String DirectoryName = "Data";
    private const String FileName = "score.txt";
    private const String Path = DirectoryName + "/" + FileName;
    
    public void LoadHighScore()
    {
        CheckPath();
        if (IsFileEmpty())
            HighScore = "00:00:00:00";
        else
            HighScore = File.ReadAllLines(Path)[0];
    }

    public void SaveHighScore(String score)
    {
        if (Int64.Parse(HighScore.Replace(":", "")) > Int64.Parse(score.Replace(":", "")))
        {
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
        if (!Directory.Exists(DirectoryName))
            Directory.CreateDirectory(DirectoryName);
        if (!File.Exists(Path))
            File.Create(Path);
    }
}
