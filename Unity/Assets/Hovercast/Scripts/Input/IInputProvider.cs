﻿using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public interface IInputProvider {
		
		Vector3 PalmDirection { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IInputSide GetSide(bool pIsLeft);

	}

}