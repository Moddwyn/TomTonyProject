using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Maze Difficulty", menuName = "Maze/Difficulty")]
public class MazeDifficulty : ScriptableObject
{
	[MinValue(1)] public int rows;
	[MinValue(1)] public int columns;
}
