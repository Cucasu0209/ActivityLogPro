using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScriptMain : MonoBehaviour
{
    public static ScriptMain instance;

    private void Awake()
    {
        instance = this;
    }

    string keyTasks = "_Tasks_";
    public void CreateNewTask(string name)
    {

        string tasks = PlayerPrefs.GetString(keyTasks, "");
        PlayerPrefs.SetString(keyTasks, tasks + name + ",");
        PlayerPrefs.SetFloat(getkey(name), 0);
    }

    public Dictionary<string, float> ListTask()
    {
        string tasksstring = PlayerPrefs.GetString(keyTasks, "");

        string[] tasks = tasksstring.Trim().Trim(',').Split(',');
        Dictionary<string, float> result = new Dictionary<string, float>();
        foreach (string task in tasks)
        {
            if (string.IsNullOrEmpty(task) == false)
                result.Add(task, getTime(task));
        }

        return result;
    }

    public void RemoveTask(string name)
    {
        string tasksstring = PlayerPrefs.GetString(keyTasks, "");

        string[] tasks = tasksstring.Trim().Trim(',').Split(',');

        string newresult = "";
        foreach (string task in tasks)
        {
            if (task != name) newresult += "," + task;
        }

        PlayerPrefs.SetString(keyTasks, newresult);
    }

    public void SaveProgress(string task, float time)
    {
        PlayerPrefs.SetFloat(getkey(task), getTime(task) + time);
    }

    private string getkey(string name)
    {
        return "__" + name.Trim().Trim(',').Trim();
    }

    public float getTime(string name)
    {
        return PlayerPrefs.GetFloat(getkey(name), 0);
    }
}
