using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGod : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        GameObject.Find("Game/EventSystem").SetActive(false);
    }
}
