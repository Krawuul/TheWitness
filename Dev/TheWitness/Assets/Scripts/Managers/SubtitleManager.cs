namespace Manager
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine;
    using System.IO;
    using TMPro;

    using Subtitle;
    using Utils;

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class SubtitleManager : Singleton<SubtitleManager>
    {
        private string m_path = Application.streamingAssetsPath + "/SubTitle/";
        [SerializeField] private string m_extension = ".json";
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private TextMeshProUGUI m_talkerText;
        private bool m_isSubtitleDisplayed = false;
        private float m_currentSentencePriority = 0;
        Queue<SubTitleData> m_textQueue = new Queue<SubTitleData>();
        SentenceData m_currentSentence;
        bool m_canPassNextSentence = false;
        bool m_isWaitingForNextSentence = false;
        bool m_sentenceComplete = false;
        Coroutine startDialogueCoroutine;
        const float SPEED = 0.05f;
        public bool subtitlePlaying = false;


        void Start()
        {
            if (m_text == null)
            {
                m_text = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        /// <summary>
        /// Start a Coroutine for the Subtitle or if One Subtitle was Already Start, Stack the Subtitle in Queue.
        /// </summary>
        public void InvokeSubTitle(string _nameFile,string _talkerName)
        {
            SubTitleData data;
            try
            {
                string jsonString = File.ReadAllText(m_path + _nameFile + m_extension);
                data = JsonUtility.FromJson<SubTitleData>(jsonString);
            }
            catch (System.Exception _e)
            {
                m_text.text = _e.Message;
                throw;
            }

           

            if (m_isSubtitleDisplayed == false)
            {
                m_currentSentencePriority = data.priority;
                startDialogueCoroutine = StartCoroutine(StartSubtitle(data,_talkerName));
            }
            else if (data.priority > m_currentSentencePriority)
            {
                m_currentSentencePriority = data.priority;
                StopCoroutine(startDialogueCoroutine);
                m_text.text = "";
                startDialogueCoroutine = StartCoroutine(StartSubtitle(data, _talkerName));
            }
            else
            {
                m_textQueue.Enqueue(data);
                startDialogueCoroutine = StartCoroutine(OnHoldSubTitle(_talkerName));
            }
        }

        IEnumerator OnHoldSubTitle(string _talkerName)
        {
            while (m_textQueue.Count > 0)
            {
                if (m_isSubtitleDisplayed == false)
                {
                    StartCoroutine(StartSubtitle(m_textQueue.Dequeue(),_talkerName));
                }

                yield return null;
            }
        }

        /// <summary>
        /// Coroutine for play Subtitle
        /// </summary>
        /// <param name="_subtitle"></param>
        /// <returns></returns>
        IEnumerator StartSubtitle(SubTitleData _subtitle,string _talkerName)
        {
            subtitlePlaying = true;
            m_isSubtitleDisplayed = true;
            m_talkerText.text = _talkerName;
            for (int i = 0; i < _subtitle.sentences.Count; i++)
            {
                m_currentSentence = _subtitle.sentences[i];
                m_sentenceComplete = false;
                m_text.text = "";

                int index = 0;
                while (index < m_currentSentence.sentence.Length && !m_sentenceComplete)
                {
                    string tempText = string.Empty;
                    if (m_currentSentence.sentence[index] == '<')
                    {

                        while (m_currentSentence.sentence[index] != '>')
                        {
                            tempText += m_currentSentence.sentence[index];
                            index++;
                        }

                        tempText += m_currentSentence.sentence[index];
                        //index++;
                    }
                    else
                    {
                        tempText += m_currentSentence.sentence[index];
                    }
                    m_text.text += tempText;
                    yield return new WaitForSeconds(m_currentSentence.timeForEachChar * SPEED);
                    index++;
                }
                m_sentenceComplete = true;

                if (_subtitle.nextSentenceByTime)
                {
                    yield return new WaitForSeconds(m_currentSentence.time );
                }
                else
                {
                    m_isWaitingForNextSentence = true;
                    //wait change the bool by another script with the function nextSentence
                    while (!m_canPassNextSentence)
                    {
                        yield return null;
                    }

                    m_canPassNextSentence = false;
                }
            }


            m_text.text = "";
            m_talkerText.text = "";
            m_isSubtitleDisplayed = false;
            subtitlePlaying = false;
            EventsManager.instance.Active(_subtitle.idEventOnFinish);
            EventsManager.instance.Active("ALL_DIALOGUE");
            EventsManager.instance.Active(_subtitle.id);
        }


        public void NextSentence()
        {
            if (!m_isWaitingForNextSentence)
            {
                if(!m_sentenceComplete)
                {
                    m_text.text = m_currentSentence.sentence;
                    m_sentenceComplete = true;
                }

                return;
            }

            m_canPassNextSentence = true;
            m_isWaitingForNextSentence = false;

        }
    }
}