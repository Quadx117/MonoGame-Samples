namespace ParticleSystem.Particles;

using Microsoft.Xna.Framework;
using ParticleSystem.Managers;

internal class MouseEmitter : IEmitter
{
    public Vector2 EmitPosition => InputManager.MousePosition;
}
