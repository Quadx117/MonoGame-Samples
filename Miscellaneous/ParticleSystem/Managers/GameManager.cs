namespace ParticleSystem.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystem.Particles;

internal class GameManager
{
    private readonly MouseEmitter _mouseEmitter = new();
    private readonly StaticEmitter _staticEmitter = new(new Vector2(300, 300));
    private readonly StaticEmitter _staticEmitter2 = new(new Vector2(500, 500));

    public void Init(ContentManager content)
    {
        ParticleEmitterData ped = new(content)
        {
            interval = 0.01f,
            emitCount = 10,
            angleVariance = 180f
        };

        ParticleEmitter pe = new(_mouseEmitter, ped);
        ParticleManager.AddParticleEmitter(pe);

        ParticleEmitterData ped2 = new(content)
        {
            interval = 0.01f,
            emitCount = 10
        };

        ParticleEmitter pe2 = new(_staticEmitter, ped2);
        ParticleManager.AddParticleEmitter(pe2);

        ParticleEmitterData ped3 = new(content)
        {
            interval = 1f,
            emitCount = 150,
            angleVariance = 180f,
            lifespanMin = 2f,
            lifespanMax = 2f,
            speedMin = 100f,
            speedMax = 100f,
            particleData = new(content)
            {
                colorStart = Color.Green,
                colorEnd = Color.Yellow,
                sizeStart = 8,
                sizeEnd = 32
            }
        };

        ParticleEmitter pe3 = new(_staticEmitter2, ped3);
        ParticleManager.AddParticleEmitter(pe3);
    }

    public static void Update(float totalSecondsElapsed)
    {
        InputManager.Update();
        ParticleManager.Update(totalSecondsElapsed);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        ParticleManager.Draw(spriteBatch);
    }
}
