# RenderTarget2D For OpenGL Projects

This project shows how to change the game window resolution while keeping the same aspect ratio, switch to borderless fullscreen, and toggle exclusive fullscreen using the OpenGL backend.

## Differences between the WindowsDX and OpenGL backends

With the OpenGL backend, switching out of borderless fullscreen or exclusive fullscreen resets the window position in the center of the screen (with some caveats, see below) and switching to borderless resets the position to the top left of the screen. This is not the case with the WindowsDX backend, which means we need to reset it ourselves in both cases.

In any case, the best option would be to save the window position before switching to borderless fullscreen, so we can restore the window to its last position to keep a consistent behavior.

### Caveats

Switching out of borderless fullscreen usually resets the window in the center of the screen, but not when the center of the screen is in the lower left quadrant of the screen. This behaviour is axis independant, meaning that if the center is below the middle of the screen on the x-axis, the window will be re-centered on the y-axis only and vice versa.

## Differences between fullscreen and borderless

Fullscreen gives the game exclusive control of the monitor and may involve changing the screen resolution to match the application's requirements. This may result in desktop icons begin relocated if the game resolution is smaller than the desktop resolution, but has the potential to boost performance when compared to borderless windowed mode. Also, the mouse cursor remains locked to whichever screen is displaying the game. To navigate out of the game, the player would need to use the Alt+Tab shortcut.

Borderless runs the game in a borderless window that covers the entire screen, giving the illusion of fullscreen while allowing the user to quickly switch to other programs or move the mouse seamlessly from one monitor to another.
