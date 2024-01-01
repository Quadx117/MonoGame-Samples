# RenderTarget2D For Windows DX Projects

This project shows how to change the game window resolution while keeping the same aspect ratio, switch to borderless fullscreen, and toggle exclusive fullscreen using the WindowsDX backend.

## Differences between the WindowsDX and OpenGL backends

With the OpenGL backend, switching out of borderless fullscreen or exclusive fullscreen resets the window position in the center of the screen and switching to borderless resets the position to the top left of the screen. This is not the case with the WindowsDX backend when using the borderless fullscreen mode, which means we need to reset it ourselves in this case. In the case of exclusive fullscreen, the window position is automatically restored to its last position.

In any case, the best option would be to save the window position before switching to borderless fullscreen, so we can restore the window to its last position to keep a consistent behavior.

## Differences between fullscreen and borderless

Fullscreen gives the game exclusive control of the monitor and may involve changing the screen resolution to match the application's requirements. This may result in desktop icons begin relocated if the game resolution is smaller than the desktop resolution, but has the potential to boost performance when compared to borderless windowed mode. Also, the mouse cursor remains locked to whichever screen is displaying the game. To navigate out of the game, the player would need to use the Alt+Tab shortcut.

Borderless runs the game in a borderless window that covers the entire screen, giving the illusion of fullscreen while allowing the user to quickly switch to other programs or move the mouse seamlessly from one monitor to another.
