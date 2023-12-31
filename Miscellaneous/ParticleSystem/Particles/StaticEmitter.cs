namespace ParticleSystem.Particles;

using Microsoft.Xna.Framework;

internal class StaticEmitter : IEmitter
{
    public Vector2 EmitPosition { get; }

    public StaticEmitter(Vector2 position)
    {
        EmitPosition = position;
    }
}
