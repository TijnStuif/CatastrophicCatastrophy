using Blok3Game.Engine.GameObjects;

public class Bullet : GameObjectList
{
    protected SpriteGameObject bullet;

    protected int x;

    protected int y;

    protected bool active;

    public Bullet (int x, int y): base()
    {
        bullet = new SpriteGameObject("Images/Characters/White-circle")
        {

        };
    }
}

