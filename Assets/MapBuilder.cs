using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour {

	[SerializeField]
	GameObject panelPrefab;
	[SerializeField]
	GameObject mapRoot;
	[SerializeField]
	CanvasScaler scaler;

	const int Width = 7;
	const int Height = 10;
	const float displayRatio = 1.77f;
	Panel[,] map;
	
	void Start () {
		map = new Panel[Width, Height];
		var sw = scaler.referenceResolution.x;
		var sh = scaler.referenceResolution.y;
		var panelWidth = Width * displayRatio > Height ? sw / Width : sh / Height;
		for(int i = 0; i < Width; i ++){
			for(int j = 0; j < Height; j++){
				var go = CreatePanel(Vector3.zero);
				go.transform.localPosition = new Vector3(i * panelWidth - sw / 2 + panelWidth / 2, j * panelWidth - sh / 2 + panelWidth / 2, 0);
				go.GetComponent<RectTransform>().sizeDelta = Vector2.one * panelWidth;
				go.transform.SetParent(mapRoot.transform);
				var p = go.GetComponent<Panel>();
				p.moveCost = Random.Range(1, 4);
				p.SetNum(p.moveCost);
				map[i, j] = p;
			}
		}

		CreateMap();
	}

	public void CreateMap(){
		for(int i = 0; i < Width; i++){
			for(int j = 0; j < Height; j++){
				var p = map[i, j];
				p.moveCost = Random.Range(1, 4);
				p.SetNum(p.moveCost);
			}
		}
		var cost = 5;
		
		var px = Random.Range(0, Width);
		var py = Random.Range(0, Height);
		map[px, py].SetPlayer();
		map[px, py].Mark();
		
		Search(px - 1, py, cost, 1);
		Search(px, py - 1, cost, 2);
		Search(px + 1, py, cost, 3);
		Search(px, py + 1, cost, 4);
	}

	GameObject CreatePanel(Vector3 pos){
		var go = Instantiate(panelPrefab);
		go.transform.SetParent(mapRoot.transform);
		go.transform.localPosition = pos;
		go.transform.localScale = Vector3.one;
		return go;
	}

	void Search(int x, int y, int cost, int di){
		if(x < 0 || y < 0 || x >= Width || y >= Height)
			return;

		var p = map[x, y];
		if(p.moveCost > cost){
			return;
		}

		var restCost = cost - p.moveCost;
		p.Mark();
		if(di != 3)
			Search(x - 1, y, restCost, 1);
		if(di != 4)
			Search(x, y - 1, restCost, 2);
		if(di != 1)
			Search(x + 1, y, restCost, 3);
		if(di != 2)
			Search(x, y + 1, restCost, 4);
	}
}
