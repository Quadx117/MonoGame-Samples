namespace ParticleSystem.Particles;

using System;
using Microsoft.Xna.Framework;
using ParticleSystem.Managers;

internal class ParticleEmitter
{
    private static readonly Random _random = new();

    private readonly ParticleEmitterData _data;
    private float _intervalLeft;
    private readonly IEmitter _emitter;

    public ParticleEmitter(IEmitter emitter, ParticleEmitterData data)
    {
        _emitter = emitter;
        _data = data;
        _intervalLeft = data.interval;
    }
    private static float RandomFloat(float min, float max)
    {
        return (float)(_random.NextDouble() * (max - min)) + min;
    }

    private void Emit(Vector2 position)
    {
        ParticleData particleData = _data.particleData;
        particleData.lifespan = RandomFloat(_data.lifespanMin, _data.lifespanMax);
        particleData.speed = RandomFloat(_data.speedMin, _data.speedMax);
        particleData.angle = RandomFloat(_data.angle - _data.angleVariance, _data.angle + _data.angleVariance);

        Particle p = new(position, particleData);
        ParticleManager.AddParticle(p);
    }

    public void Update(float totalSecondsElapsed)
    {
        _intervalLeft -= totalSecondsElapsed;
        while (_intervalLeft <= 0f)
        {
            _intervalLeft += _data.interval;
            Vector2 pos = _emitter.EmitPosition;
            for (int i = 0; i < _data.emitCount; i++)
            {
                Emit(pos);
            }
        }
    }
}
