﻿using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiParentRenderer : UiBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Parent");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

	}

}