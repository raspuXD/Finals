using UnityEngine;

[CreateAssetMenu(fileName = "NewWholeFood", menuName = "Food/WholeFood")]
public class WholeFood : ScriptableObject
{
    public string foodName;
    public Sprite image;
    public Ingredient ingredient1;
    public Ingredient ingredient2;
    public Ingredient ingredient3;
}
