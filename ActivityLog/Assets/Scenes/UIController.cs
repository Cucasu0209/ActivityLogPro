using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public Button listitemPrefab;

    public RectTransform listTrans;
    public TextMeshProUGUI nameTask;
    public TextMeshProUGUI time;
    public TextMeshProUGUI currenttime;
    public TMP_InputField newtask;
    public TextMeshProUGUI startorStopButton;


    public GameObject AlertBox;
    public Button YesAlert;
    public Button NoAlert;

    private string currenTask = "";
    private float currentime = 0;
    private bool iscd = false;


    private void Start()
    {
        DisplayList();
    }

    public void DisplayList()
    {
        var list = ScriptMain.instance.ListTask();

        foreach (Transform child in listTrans)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in list)
        {
            string task = item.Key;
            Button newitem = Instantiate(listitemPrefab, listTrans);
            newitem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(task);
            newitem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(formatTIme(item.Value));
            newitem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                AlertBox.SetActive(true);

                NoAlert.onClick.RemoveAllListeners();
                NoAlert.onClick.AddListener(() =>
                {
                    AlertBox.SetActive(false);
                });

                YesAlert.onClick.RemoveAllListeners();
                YesAlert.onClick.AddListener(() =>
                {
                    ScriptMain.instance.RemoveTask(task);
                    AlertBox.SetActive(false);
                    DisplayList();
                });


            });

            newitem.onClick.AddListener(() => { DisplayTask(task); });
        }
    }

    public void Createnew()
    {
        ScriptMain.instance.CreateNewTask(newtask.text);
        newtask.text = "";
        DisplayList();
    }

    public void StartOrStop()
    {
        if (string.IsNullOrEmpty(currenTask)) return;
        iscd = !iscd;
        startorStopButton.SetText(iscd ? "Stop" : "Start");
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(currenTask)) return;
        ScriptMain.instance.SaveProgress(currenTask, currentime);
        DisplayList();
        DisplayTask(currenTask);
    }

    public void DisplayTask(string taskname)
    {
        currenTask = taskname;
        nameTask.SetText(taskname);
        currenttime.SetText(formatTIme(ScriptMain.instance.getTime(taskname)));

        time.SetText(formatTIme(0));
        iscd = false;
        currentime = 0;

        startorStopButton.SetText("Start");
    }
    public void ResetTime()
    {
        if (string.IsNullOrEmpty(currenTask)) return;
        time.SetText(formatTIme(0));
        iscd = false;
        currentime = 0;
        startorStopButton.SetText("Start");
    }

    private string formatTIme(float time)
    {
        float secound = Mathf.Floor(time % 60);
        float minute = Mathf.Floor((time % 3600) / 60);
        float hour = Mathf.Floor(time / 3600);
        return $"{hour}:{minute}:{secound}";
    }

    private void Update()
    {
        if (iscd)
        {
            currentime += Time.deltaTime;
            time.SetText(formatTIme(currentime));

        }

    }
}
