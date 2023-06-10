using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropField : MonoBehaviour
{
    private float generateTime;

    private GameObject crop = null;
    private GameObject lv1 = null;
    private GameObject lv2 = null;
    private GameObject lv3 = null;

    private FieldState state;
    public FieldState State { get { return state; } set { state = value; } }

    public GameObject Crop { get { return crop; } set { crop = value; } }

    public enum FieldState
    {
        Empty,
        Growing,
        Grown
    }
    public enum CropType
    {
        None = 0,
        Carrot = 1,
        Corn = 2,
        Cabbage = 3
    }


    private void Start()
    {
        state = FieldState.Empty;
    }

    public void Plant(CropType type)
    {
        string name = "";
        switch (type)
        {
            case CropType.Carrot:
                name = "Prefabs/Farm/Carrot";
                break;
            case CropType.Cabbage:
                name = "Prefabs/Farm/Cabbage";
                break;
            case CropType.Corn:
                name = "Prefabs/Farm/Corn";
                break;
            default:
                break;
        }

        GameObject prefab = Resources.Load<GameObject>(name);
        crop = GameObject.Instantiate(prefab);
        crop.transform.SetParent(transform);
        crop.transform.localPosition = Vector3.zero;
        generateTime = crop.GetComponent<Crop>().generateTime;

        //Lv별 작물 찾아두기 
        Transform[] children = crop.GetComponentsInChildren<Transform>();

        foreach(Transform child in children)
        {
            if (child.name == "Lv1")
                lv1 = child.gameObject;
            else if (child.name == "Lv2")
                lv2 = child.gameObject;
            else if (child.name == "Lv3")
                lv3 = child.gameObject;
        }

        state = FieldState.Growing;
        lv2.SetActive(false);
        lv3.SetActive(false);

        StartCoroutine(GrowToLv2AfterDelay());
    }

    public IEnumerator GrowToLv2AfterDelay()
    { //작물을 Lv2로 
        yield return new WaitForSeconds(generateTime / 2);

        //일정 시간 후 보이게
        lv1.SetActive(false);
        lv2.SetActive(true);

        StartCoroutine(GrowToLv3AfterDelay());
    }

    public IEnumerator GrowToLv3AfterDelay()
    { //작물을 Lv3으로
        yield return new WaitForSeconds(generateTime / 2);

        //일정 시간 후 보이게
        lv2.SetActive(false);
        lv3.SetActive(true);

        state = FieldState.Grown;
    }

}
