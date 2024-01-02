# Nine Patch

This project shows how to render a texture using the nine patch technique (sometimes called 9-slice, box stretch or button stretch). This technique is often used in GUI's to render buttons or frame from a texture, without distorting or creating blur in the corners of the scaled texture.

## Definition

9-patch is a 2D technique which allows you to reuse an image at various sizes without needing to prepare multiple Assets. It involves splitting the image into nine portions, so that when you re-size the sprite, the different portions scale or tile (that is, repeat in a grid formation) in different ways to keep the sprite in proportion.

The following points describe what happens when you change the dimensions of the image:

- The four corners do not change in size.
- The middle-top and middle-bottom sections stretch or tile horizontally.
- The middle-left and middle-right sections stretch or tile vertically.
- The middle section stretches or tiles both horizontally and vertically.

Note that this usually does not work when the destination rectangle is smaller than the texture, i.e. it cannot scale down. This means that it is best to create the smallest texture possible to accomodate for the widest range of options.

## Future Improvements

- [ ] Add Scaling VS Tiling by passing an enum value (default to Scaling)
- [ ] Add an overload where we specify the distance to the middle for each side individually (left, right, top, and bottom)
- [ ] Add a version using shaders on the GPU

## References

- https://gamedev.stackexchange.com/questions/13268/rounded-corners-in-xna
- [Unity Documentation](https://docs.unity3d.com/Manual/9SliceSprites.html)
- [GDevelop Documentation](https://wiki.gdevelop.io/gdevelop5/objects/panel_sprite/)

## Other Links

See this [stackexchange](https://gamedev.stackexchange.com/questions/153848/how-do-i-set-up-9-slicing-in-opengl) question for a GPU version of this technique.
