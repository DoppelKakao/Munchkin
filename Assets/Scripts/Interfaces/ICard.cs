using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
	string Name { get; set; }
	string Description { get; set; }
	Sprite Background { get; set; }
	Sprite Illustration { get; set; }
}
