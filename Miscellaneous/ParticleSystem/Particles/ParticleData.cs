namespace ParticleSystem.Particles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

internal struct ParticleData
{
    // TODO(PERE): The original code used Globals to automatically assign the
    // default texture here, since the default parameterless constructor must
    // always be available for structs. Modify this architecture to eliminate
    // the need to use Globals without the risk of not initializing this varialbe.
    private static Texture2D _defaultTexture;

    public Texture2D texture;
    public float lifespan = 2f;
    public Color colorStart = Color.Yellow;
    public Color colorEnd = Color.Red;
    public float opacityStart = 1f;
    public float opacityEnd = 0f;
    public float sizeStart = 32f;
    public float sizeEnd = 4f;
    public float speed = 100f;
    public float angle = 0f;

    public ParticleData(ContentManager content)
    {
        _defaultTexture ??= content.Load<Texture2D>(@"Images\particle");
        texture = _defaultTexture;
    }
}
