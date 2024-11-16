namespace Subtitle
{
    [System.Serializable]
    public class SentenceData
    {
        /// <summary>
        /// Time of the sentence is show before the next sentence 
        /// </summary>
        public float time = 0f;
        /// <summary>
        /// sentence show
        /// </summary>
        public string sentence = "";
        /// <summary>
        /// Path of played vocal line, can be null
        /// </summary>
        public string voiceLinePath = "";
        /// <summary>
        /// Time wait before write the next char
        /// </summary>
        public float timeForEachChar = 1;
    }
}