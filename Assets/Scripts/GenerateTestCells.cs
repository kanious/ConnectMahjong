using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTestCells : MonoBehaviour
{
    [SerializeField] GameObject icon;
    [SerializeField] UITable table;

    List<TweenPosition> tweenPosList = new List<TweenPosition>();
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (null == table || null == icon)
            return;

        for (int i = 0; i < 100; ++i)
        {
            //GameObject newObj = Instantiate(icon, table.transform);
            GameObject newObj = Instantiate(icon, transform);
            newObj.SetActive(true);
            Transform childIconObj = newObj.transform.Find("Icon");
            UISprite sprite = childIconObj.gameObject.GetComponent<UISprite>();
            if (null != sprite)
                sprite.spriteName = i.ToString();
            TweenPosition tp = newObj.GetComponent<TweenPosition>();
            if (null != tp)
                tweenPosList.Add(tp);
        }

        //8 * 13 = 104
        //8 * 12 = 96

        //table.Reposition();
    }

    // Update is called once per frame
    void Update()
    {
        if (index < tweenPosList.Count)
        {
            tweenPosList[index].enabled = true;
            index++;
        }
    }
}
