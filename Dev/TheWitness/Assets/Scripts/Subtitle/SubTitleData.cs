namespace Subtitle
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class SubTitleData
    {
        public string id = "";
        public List<SentenceData> sentences;
        public int priority = 0;
        public bool canRepeat;
        public bool nextSentenceByTime;
        public bool canSkip;

        public string idEventOnFinish;
    }
}