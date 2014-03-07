using UnityEngine;
using System.Collections;

public class GameControllerHash
{
	public struct Buttons
	{
		public const string A = "A";
		public const string B = "B";
		public const string X = "X";
		public const string Y = "Y";

		public const string RB = "RIGHT_BUMPER";
		public const string LB = "LEFT_BUMPER";

		public const string START = "START";
		public const string BACK = "BACK";
	}

	public struct Triggers
	{
		public const string LEFT = "LEFT_TRIGGER";
		public const string RIGHT = "RIGHT_TRIGGER";
	}

	public struct Dpad
	{
		public const string HORIZONTAL = "HORIZONTAL_D";
		public const string VERTICAL = "VERTICAL_D";
	}

	public struct LeftStick
	{
		public const string HORIZONTAL = "Horizontal";
		public const string VERTICAL = "Vertical";
	}

	public struct RightStick
	{
		public const string HORIZONTAL = "HORIZONTAL_R";
		public const string VERTICAL = "HORIZONTAL_V";
	}
}
