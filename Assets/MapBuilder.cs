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
	IList<Vector2> directions = new List<Vector2>(){
		Vector2.up,
		Vector2.left,
		Vector2.down,
		Vector2.right,
		// 斜め移動対応時
		/*
		new Vector2(1, 1),
		new Vector2(-1, -1),
		new Vector2(-1, 1),
		new Vector2(1, -1) */
	};
	
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
				p.maxMove = 0;
				p.SetNum(p.moveCost);
			}
		}
		var cost = 5;
		
		var px = Random.Range(0, Width);
		var py = Random.Range(0, Height);
		var pp = map[px, py];
		pp.SetPlayer();
		Search(px, py, cost + pp.moveCost);
	}

	void Search(int x, int y, int cost){
		if (x < 0 || y < 0 || x >= Width || y >= Height) {
			return;
		}

		var p = map[x, y];
		if(p.moveCost > cost || !p.Mark(cost)){
			return;
		}

		var restCost = cost - p.moveCost;

		foreach (var d in directions) {
			Search ((int)(x + d.x), (int)(y + d.y), restCost);
		}
	}

	GameObject CreatePanel(Vector3 pos){
		var go = Instantiate(panelPrefab);
		go.transform.SetParent(mapRoot.transform);
		go.transform.localPosition = pos;
		go.transform.localScale = Vector3.one;
		return go;
	}
}
