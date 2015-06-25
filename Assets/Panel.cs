using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Panel : MonoBehaviour {

	[SerializeField]
	Text text;
	public int moveCost;
	public int maxMove;

	public void SetNum(int n){
		text.text = n.ToString();
		GetComponent<Image>().color = Color.white;
		text.color = Color.black;
	}
	
	public void SetPlayer(){
		text.text = "P";
		text.color = Color.red;
	}

	public bool Mark(int move){
		if (move < maxMove)
			return false;
		GetComponent<Image>().color = Color.gray;
		maxMove = move;
		return true;
	}
}
