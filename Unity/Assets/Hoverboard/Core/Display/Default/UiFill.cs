﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hoverboard.Core.Display.Default {

	/*================================================================================================*/
	public class UiFill {

		public const float AngleInset = UiButton.Size*0.01f;

		private readonly bool vCalcSelections;
		private float vWidth;
		private float vHeight;

		private readonly GameObject vBackground;
		private readonly GameObject vEdge;
		private readonly GameObject vHighlight;
		private readonly GameObject vSelect;

		private readonly Vector3[] vSelectionPoints;
		private float vPrevHighAmount;
		private float vPrevSelAmount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiFill(GameObject pGameObj, bool pBackgroundOnly=false, string pName="Background") {
			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.name = pName;
			vBackground.transform.SetParent(pGameObj.transform, false);
			vBackground.transform.localRotation = rot;
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			if ( pBackgroundOnly ) {
				return;
			}

			vEdge = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vEdge.name = "Edge";
			vEdge.transform.SetParent(pGameObj.transform, false);
			vEdge.transform.localRotation = rot;
			vEdge.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vEdge.renderer.sharedMaterial.renderQueue -= 300;
			vEdge.renderer.sharedMaterial.color = Color.clear;

			vHighlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vHighlight.name = "Highlight";
			vHighlight.transform.SetParent(pGameObj.transform, false);
			vHighlight.transform.localRotation = rot;
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vSelect.name = "Select";
			vSelect.transform.SetParent(pGameObj.transform, false);
			vSelect.transform.localRotation = rot;
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 10;
			vSelect.renderer.sharedMaterial.color = Color.clear;

			UpdateSize(UiButton.Size, UiButton.Size);
			vSelectionPoints = CalcSelectionPoints();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSize(float pWidth, float pHeight) {
			var w = pWidth-AngleInset*2;
			var h = pHeight-AngleInset*2;

			if ( Math.Abs(w-vWidth) < 0.005f && Math.Abs(h-vHeight) < 0.005f ) {
				return;
			}

			vWidth = w;
			vHeight = h;

			UpdateQuad(vBackground, 1);

			if ( vHighlight != null ) {
				UpdateQuad(vEdge, 0);
				UpdateQuad(vHighlight, 0);
				UpdateQuad(vSelect, 0);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateBackground(Color pColor) {
			vBackground.renderer.sharedMaterial.color = pColor;
			vBackground.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateEdge(Color pColor) {
			vEdge.renderer.sharedMaterial.color = pColor;
			vEdge.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHighlight(Color pColor, float pAmount) {
			vHighlight.renderer.sharedMaterial.color = pColor;
			vHighlight.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevHighAmount) > 0.005f ) {
				UpdateQuad(vHighlight, pAmount);
				vPrevHighAmount = pAmount;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSelect(Color pColor, float pAmount) {
			vSelect.renderer.sharedMaterial.color = pColor;
			vSelect.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevSelAmount) > 0.005f ) {
				UpdateQuad(vSelect, pAmount);
				vPrevSelAmount = pAmount;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			//TODO: Optimize this somehow, probably by reducing the number of points/distances to 
			//check. This could be done by sampling some key points, finding the closest one, then only
			//doing further checks for the points nearest to it.

			float sqrMagMin = float.MaxValue;
			Vector3 nearest = Vector3.zero;
			//Transform tx = vBackground.transform.parent;
			//Vector3 worldCurs = tx.TransformPoint(pCursorLocalPos);
			//Vector3 worldPos;

			foreach ( Vector3 pos in vSelectionPoints ) {
				float sqrMag = (pos-pCursorLocalPos).sqrMagnitude;

				if ( sqrMag < sqrMagMin ) {
					sqrMagMin = sqrMag;
					nearest = pos;
				}

				//worldPos = tx.TransformPoint(pos);
				//Debug.DrawLine(worldPos, worldCurs, Color.yellow);
			}

			//worldPos = tx.TransformPoint(nearest);
			//Debug.DrawLine(worldPos, worldCurs, Color.red);
			return nearest;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateQuad(GameObject pObj, float pThickness) {
			pObj.transform.localScale = new Vector3(vWidth*pThickness, vHeight*pThickness, 1);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Vector3[] CalcSelectionPoints() {
			var points = new List<Vector3>();
			points.Add(Vector3.zero);

			//TODO: add more selection points

			/*const int innerSteps = 5;
			Mesh bgMesh = vBackground.GetComponent<MeshFilter>().mesh;

			for ( int i = 3 ; i < bgMesh.vertices.Length-2 ; i += 2 ) {
				Vector3 outer = bgMesh.vertices[i];
				Vector3 inner = bgMesh.vertices[i-1];

				for ( int j = 0 ; j < innerSteps ; ++j ) {
					points.Add(Vector3.Lerp(outer, inner, j/(float)(innerSteps-1)));
				}
			}*/

			return points.ToArray();
		}

	}

}
