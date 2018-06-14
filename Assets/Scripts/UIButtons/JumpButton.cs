using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Sprite jumpSpt;
    public Sprite chargeSpt;
    Character player;
    private Button btn;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Character>();
        btn = GetComponent<Button>();
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.3f, Screen.width * 0.3f);
        btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.closeWall && !player.dead)
        {
            player.charging = true;  
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!player.dead)
        {
            player.Jump();
        }
    }

    private void Update()
    {
        if (!player.charging)
        {
            btn.GetComponent<Image>().sprite = jumpSpt;
            
        } else
        {
            btn.GetComponent<Image>().sprite = chargeSpt;
            ParticleSystem par = PoolManager.Spawn("Charge2Particle");
            par.transform.position = transform.position;
            par.transform.parent = transform;
            par.transform.localScale = Vector3.one *3;
            
        }
    }

}
