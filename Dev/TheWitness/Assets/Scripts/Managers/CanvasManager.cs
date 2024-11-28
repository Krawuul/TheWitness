namespace Manager
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class CanvasManager : Singleton<CanvasManager>
    {

        [SerializeField] Image m_imageFade;
        bool m_isFade;

        bool IsFade {  get { return m_isFade; } }

        CanvasManager()
        {
            m_isFade = false;
        }

        public void Fade(float _speed = 0.5f, float _waitSeconds = 1f)
        {
            StartCoroutine(FadeInOut(_speed,_waitSeconds));
        }

        IEnumerator FadeInOut(float _speed, float _waitSeconds)
        {
            m_imageFade.gameObject.SetActive(true);
  
            float alpha = 0f;
            while(alpha < 1f)
            {
                alpha += Time.deltaTime * _speed;
                m_imageFade.color = new Color(0,0,0,alpha);
                yield return null;
            }

            yield return new WaitForSeconds(_waitSeconds);

            while(alpha > 0f)
            {
                alpha -= Time.deltaTime * _speed;
                m_imageFade.color = new Color(0, 0, 0,alpha);
                yield return null;
            }

            m_imageFade.gameObject.SetActive(false);
        }

    }
}