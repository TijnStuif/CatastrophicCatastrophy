using System;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

public class Player : GameObjectList
{
    //all variables that a player needs
    protected SpriteGameObject player;
    public int HP;
    private readonly int Size = 187;
    private int MoveSpeed = 5;
    private int PlayerDashTimer = 0;
    private Vector2 Direction = new();
    private bool IsDashing = false;
    private int DashCooldown = 0;
    public new Vector2 Position = new();

    public Player(int X, int Y, int Health) : base()
    {
        //initialises player with a sprite and position
        player = new SpriteGameObject("Images/Characters/circle", 1, "")
        {
            Position = new Vector2(X, Y)
        };
        Add(player);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Console.WriteLine(PlayerDashTimer);
        base.HandleInput(inputHelper);
        CheckPlayerDashDuration();

        //sets dashing state to true when the shift button is pressed and the cooldown requirement is met, and then activates dashing logic while IsDashing is still true
        if (inputHelper.KeyPressed(Keys.LeftShift) && DashCooldown <= 0)
        {
            IsDashing = true;
        }
        if (IsDashing)
        {
            if (player.Position.X is <= 0 or >= 613 || player.Position.Y is <= 0 or >= 413) 
            {
                ResetDashValue();
                return;
            }
            PlayerDash();
        }
        CheckForMovementInputs(inputHelper);
    }

    //Increases movement speed for a short duration, which launches the player forward, and puts dash on a cooldown
    private void PlayerDash()
    {
        PlayerDashTimer++;
        DashCooldown = 60;
        MoveSpeed = 25;
        player.Position = new Vector2(player.Position.X + MoveSpeed * Direction.X, player.Position.Y + MoveSpeed * Direction.Y);
        return; 
    }

    //Reduces DashCooldown every frame, and also stops the player from dashing once the dash duration limit is met
    private void CheckPlayerDashDuration()
    {
        if (DashCooldown > 0)
        {
            DashCooldown--;
        }
        if (PlayerDashTimer > 3)
        {
            ResetDashValue();
            return;
        }
    }

    //checks for wasd movement, then sets position based on movespeed and direction (which is determined by what key on the keyboard is pressed)
    private void CheckForMovementInputs(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.IsKeyDown(Keys.W) && player.Position.Y > 0)
        {
            Direction = new Vector2(0, -1);
            player.Position = new Vector2(player.Position.X, player.Position.Y + MoveSpeed * Direction.Y);
        }
        if (inputHelper.IsKeyDown(Keys.A) && player.Position.X > 0)
        {
            Direction = new Vector2(-1, 0);
            player.Position = new Vector2(player.Position.X + MoveSpeed * Direction.X, player.Position.Y);
        }
        if (inputHelper.IsKeyDown(Keys.S) && player.Position.Y < 600 - Size)
        {
            Direction = new Vector2(0, 1);
            player.Position = new Vector2(player.Position.X, player.Position.Y + MoveSpeed * Direction.Y);
        }
        if (inputHelper.IsKeyDown(Keys.D) && player.Position.X < 800 - Size)
        {
            Direction = new Vector2(1, 0);
            player.Position = new Vector2(player.Position.X + MoveSpeed * Direction.X, player.Position.Y);
        }
    }

    private void ResetDashValue()
    {
        IsDashing = false;
        PlayerDashTimer = 0;
        MoveSpeed = 5;
    }
}