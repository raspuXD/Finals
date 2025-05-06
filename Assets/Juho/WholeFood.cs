using UnityEngine;

[CreateAssetMenu(fileName = "NewWholeFood", menuName = "Food/WholeFood")]
public class WholeFood : ScriptableObject
{
    public string foodName;
    public Sprite image;
    public Ingredient.State ingredient1;
    public Ingredient.State ingredient2;
    public Ingredient.State ingredient3;
}
