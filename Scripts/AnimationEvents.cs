using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    /// <summary>
    /// выход назад
    /// </summary>
    public void disableObj()
    {
        gameObject.SetActive(false);
    }
}