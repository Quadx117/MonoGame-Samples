namespace ParticleSystem.Managers;

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystem.Particles;

internal class ParticleManager
{
    private static readonly List<Particle> _particles = new();
    private static readonly List<ParticleEmitter> _particleEmitters = new();

    public static void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    public static void AddParticleEmitter(ParticleEmitter particleEmitter)
    {
        _particleEmitters.Add(particleEmitter);
    }

    public static void UpdateParticles(float totalSecondsElapsed)
    {
        foreach (Particle particle in _particles)
        {
            particle.Update(totalSecondsElapsed);
        }

        _ = _particles.RemoveAll(p => p.isFinished);
    }

    public static void UpdateEmitters(float totalSecondsElapsed)
    {
        foreach (ParticleEmitter emitter in _particleEmitters)
        {
            emitter.Update(totalSecondsElapsed);
        }
    }

    public static void Update(float totalSecondsElapsed)
    {
        UpdateParticles(totalSecondsElapsed);
        UpdateEmitters(totalSecondsElapsed);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Particle particle in _particles)
        {
            particle.Draw(spriteBatch);
        }
    }
}
