using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Subtitle;

public class WindowSubtitle : EditorWindow
{
    SubTitleData m_subTitleData = new SubTitleData();
    string m_path = Application.streamingAssetsPath + "/SubTitle/";
    string m_extension = ".json";
    Vector2 scrollPosition = Vector2.zero;
    int m_nbr = 0;
    string m_beforeName;

    [MenuItem("Tools/Subtitle")]
    public static void OpenWindow()
    {
        GetWindow<WindowSubtitle>("Subtitle");
    }

    private void Addsentence()
    {
        SentenceData sentence = new SentenceData();
        sentence.time = 0;
        sentence.sentence = "";
        sentence.voiceLinePath = "";
        sentence.timeForEachChar = 1f;

        if(m_subTitleData.sentences == null) m_subTitleData.sentences = new List<SentenceData>();

        m_subTitleData.sentences.Add(sentence);
    }

    private void RemoveSentence(int _index)
    {
        m_subTitleData.sentences.RemoveAt(_index);
    }

    private void BeginID()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("ID :", GUILayout.Width(20));
        m_subTitleData.id = EditorGUILayout.TextField(m_subTitleData.id, GUILayout.Width(90)).ToUpper();

        EditorGUILayout.LabelField("Priority :", GUILayout.Width(40));
        m_subTitleData.priority = EditorGUILayout.IntField(m_subTitleData.priority, GUILayout.Width(50));

        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Load", GUILayout.Width(200)))
        {
            m_beforeName = m_subTitleData.id;
            try
            {
                string jsonString = File.ReadAllText(m_path + m_subTitleData.id + m_extension);
                m_subTitleData = JsonUtility.FromJson<SubTitleData>(jsonString);
                m_nbr = m_subTitleData.sentences.Count;
                

            }
            catch (FileNotFoundException)
            {
                m_subTitleData.sentences = new List<SentenceData>();
                m_nbr = 0;
            }
        };

        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Can Repeat : ", GUILayout.Width(80));
        m_subTitleData.canRepeat = EditorGUILayout.Toggle(m_subTitleData.canRepeat, GUILayout.Width(20));

        EditorGUILayout.LabelField("Can Skip : ", GUILayout.Width(60));
        m_subTitleData.canSkip = EditorGUILayout.Toggle(m_subTitleData.canSkip, GUILayout.Width(20));

        EditorGUILayout.LabelField("Next sentence by Time :", GUILayout.Width(140));
        m_subTitleData.nextSentenceByTime = EditorGUILayout.Toggle(m_subTitleData.nextSentenceByTime, GUILayout.Width(165));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("ID Events On Finish : ", GUILayout.Width(120));
        m_subTitleData.idEventOnFinish = EditorGUILayout.TextField(m_subTitleData.idEventOnFinish, GUILayout.Width(120));

        GUILayout.EndHorizontal();
    }

    private void NumberSentences()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Number of Sentences :", GUILayout.Width(130));
        EditorGUILayout.LabelField(m_nbr.ToString(), GUILayout.Width(20));

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(20) ))
        {
            m_nbr++;
            Addsentence();
        };

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            if (m_nbr > 0)
            {
                m_nbr--;
                RemoveSentence(m_nbr);
            }
        };

        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();
    }

    private void ShowSentences()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < m_nbr; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Sentence :", GUILayout.Width(75), GUILayout.Height(100));
            m_subTitleData.sentences[i].sentence = EditorGUILayout.TextArea(m_subTitleData.sentences[i].sentence, GUILayout.MinWidth(100), GUILayout.MaxWidth(1080), GUILayout.Width(300), GUILayout.Height(100), GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            //EditorGUILayout.LabelField("Path VoiceLine :", GUILayout.Width(90));
            //m_subTitleData.sentences[i].voiceLinePath = EditorGUILayout.TextField(m_subTitleData.sentences[i].voiceLinePath, GUILayout.Width(165));

            EditorGUILayout.LabelField("Speed :", GUILayout.Width(45));
            m_subTitleData.sentences[i].timeForEachChar = EditorGUILayout.FloatField(m_subTitleData.sentences[i].timeForEachChar, GUILayout.Width(40));

            if (m_subTitleData.nextSentenceByTime)
            {
                EditorGUILayout.LabelField("Time :", GUILayout.Width(40));
                m_subTitleData.sentences[i].time = EditorGUILayout.FloatField(m_subTitleData.sentences[i].time, GUILayout.Width(40));
            }

            GUILayout.EndHorizontal();

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                m_nbr--;
                RemoveSentence(i);
            };

            GUI.backgroundColor = Color.white;
            GUILayout.Space(10);
        }

        GUILayout.EndScrollView();
    }

    private void OnGUI()
    {
        BeginID();
        NumberSentences();
        ShowSentences();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generate Subtitle", GUILayout.Width(250), GUILayout.MinWidth(200)))
        {
            if(m_beforeName != m_subTitleData.id)
            {
                File.Delete(m_path + m_beforeName + m_extension);
                File.Delete(m_path + m_beforeName + m_extension + ".meta");
                m_beforeName = m_subTitleData.id;
            }

            string subTitle = JsonUtility.ToJson(m_subTitleData);
            File.WriteAllText(m_path+ m_subTitleData.id + m_extension, subTitle);
            AssetDatabase.Refresh();
        };
        GUI.backgroundColor = Color.white;
    }
}
