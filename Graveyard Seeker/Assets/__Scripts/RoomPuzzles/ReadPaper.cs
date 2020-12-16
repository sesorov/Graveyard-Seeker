using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPaper : MonoBehaviour {

    public GameObject Paper;
    public bool Checked;
	public void OnClick()
    {
        Checked = true;
        if (!Paper.activeSelf) Paper.SetActive(true);
        else Paper.SetActive(false);
    }
    public void OnClose()
    {
        Paper.SetActive(false);
    }
}
