using Microsoft.Xna.Framework;
using Blok3Game.GameObjects;

public class ShootingEnemy : Enemy
{
    public ShootingEnemy(int hitPoints, int moveSpeed, Vector2 position) : base(hitPoints, moveSpeed, position, "Images/Characters/rat", 0, " ", 0)
    {
    }
}