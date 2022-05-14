using UnityEngine;

[CreateAssetMenu(fileName = "New LevelUpCard", menuName = "Cards/LevelUpCard")]
public class LevelUpCard : ScriptableObject, ICard
{
	public new string name;
	public string description;

	public Sprite background;
	public Sprite artwork;

	public int level;

	public string Name { get; set; }
	public string Description { get; set; }
	public Sprite Background { get; set; }
	public Sprite Illustration { get; set; }
}
