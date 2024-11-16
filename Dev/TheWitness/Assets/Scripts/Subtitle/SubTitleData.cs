namespace Subtitle
{
    using System.Collections.Generic;

    [System.Serializable]
    public class SubTitleData
    {
        public string id = "";
        public List<SentenceData> sentences;
        public int priority = 0;
        public bool canRepeat;
        public bool nextSentenceByTime;
        public bool canSkip;
    }
}