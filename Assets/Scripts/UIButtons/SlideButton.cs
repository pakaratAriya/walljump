using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Sprite slideSpt;
    public Sprite dashSpt;
    Character player;
    private Button btn;
    void Start()
    {
        player = FindObjectOfType<Character>();
        btn = GetComponent<Button>();
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.3f, Screen.width * 0.3f);
        btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(-Screen.width * 0.3f / 2, Screen.width * 0.3f / 2);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.rb.gravityScale != 0 && !player.dead)
        {
            player.Dash();
        }
        else if (player.charging && !player.dead)
        {
            player.charging = false;
            player.power = 0;
        }
        else
        {
            if (!player.dead)
            {
                player.sliding = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.sliding = false;

    }

    private void Update()
    {
        if (player.rb.gravityScale != 0)
        {
            btn.GetComponent<Image>().sprite = dashSpt;
        }
        else
        {
            btn.GetComponent<Image>().sprite = slideSpt;
        }
    }

}
