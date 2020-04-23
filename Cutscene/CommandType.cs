using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Cutscene command type.
    /// </summary>
    public enum CommandType : byte {

        /// <summary>
        /// Custom command type.
        /// </summary>
        Custom = 0x00,

        /// <summary>
        /// Wait for a particular amount of milliseconds.
        /// </summary>
        Wait,

        /// <summary>
        /// Load an audio. String 1: Audio name. String 2: File path.
        /// </summary>
        LoadSound,

        /// <summary>
        /// Unload an audio file. String 1: Audio name.
        /// </summary>
        UnloadSound,

        /// <summary>
        /// Play a sound. String 1: Audio name. String 2 (Optional): Loop file path. Float 1 (Optional): Volume. Boolean 1 (Optional): Unload audio on completion.
        /// </summary>
        PlaySound,

        /// <summary>
        /// Play an independent sound. String 1: Audio name prefix. String 2: Audio file path. String 3 (Optional): Loop path. Float 1 (Optional): Volume.
        /// </summary>
        PlayIndependentSound,

        /// <summary>
        /// Resume a sound. String 1: Audio name.
        /// </summary>
        ResumeSound,

        /// <summary>
        /// Pause a sound. String 1: Audio name.
        /// </summary>
        PauseSound,

        /// <summary>
        /// Stop a sound. String 1: Audio name.
        /// </summary>
        StopSound,

        /// <summary>
        /// Set the sound position. String 1: Audio name. Decimal 1 (Optional): Days. Double 1 (Optional): Hours. Float 1 (Optional): Minutes. Int 1 (Optional): Seconds. UInt 1 (Optional): Milliseconds.
        /// </summary>
        SoundPosition,

        /// <summary>
        /// Set the sound volume. String 1: Audio name. Float 2: Volume.
        /// </summary>
        SoundVolume,

        /// <summary>
        /// Load a drawing or replace it with a new one. String 1: Drawing name. String 2: File path. Boolean 1 (Optional): Unload after ending cutscene.
        /// </summary>
        LoadDrawing,

        /// <summary>
        /// Load an image from a color. String 1: Drawing name. Float 1: Width. Float 2: Height. Int 1: Red. Int 2: Green. Int 3: Blue. Int 4 (Optional): Alpha. Boolean 1 (Optional): Unload after ending cutscene.
        /// </summary>
        LoadImageFromColor,

        /// <summary>
        /// If the drawing is left after the cutscene ends. String 1: Drawing name. Boolean 1 (Optional): If the drawing survives end of cutscene. If no boolean present, it toggles.
        /// </summary>
        DrawingPersistence,

        /// <summary>
        /// Set whether or not the drawing ignores the camera position. String 1: Drawing name. Boolean 1 (Optional): If the drawing ignores the camera position. If no boolean present, it toggles.
        /// </summary>
        DrawingIgnoreCamera,

        /// <summary>
        /// Change the X position of the drawing. String 1: Drawing name. Number 1: X position.
        /// </summary>
        DrawingX,

        /// <summary>
        /// Change the X position of the drawing. String 1: Drawing name. Number 1: X position.
        /// </summary>
        DrawingY,

        /// <summary>
        /// Change the width of a drawing. String 1: Drawing name. Number 1: Width.
        /// </summary>
        DrawingWidth,

        /// <summary>
        /// Change the height of a drawing. String 1: Drawing name. Number 1: Height.
        /// </summary>
        DrawingHeight,

        /// <summary>
        /// Change the velocity of a drawing. String 1: Drawing name. Number 1: Velocity X.
        /// </summary>
        DrawingVelocityX,

        /// <summary>
        /// Change the velocity of a drawing. String 1: Drawing name. Number 1: Velocity Y.
        /// </summary>
        DrawingVelocityY,

        /// <summary>
        /// Change the acceleration of a drawing. String 1: Drawing name. Number 1: Acceleration X.
        /// </summary>
        DrawingAccelerationX,

        /// <summary>
        /// Change the acceleration of a drawing. String 1: Drawing name. Number 1: Acceleration Y.
        /// </summary>
        DrawingAccelerationY,

        /// <summary>
        /// Wait till a drawing moves to the given coordinates with its current velocity and acceleration. String 1: Drawing name. Number 1: Destination X. Number 2: Destination Y.
        /// </summary>
        DrawingWaitTillPosition,

        /// <summary>
        /// Change the rotation of the drawing. String 1: Drawing name. UInt 1 (Optional): Degrees on a scale to 0 to 0x10000. Double 1 (Optional): Radians. Float 1 (Optional): Degrees.
        /// </summary>
        DrawingRotation,

        /// <summary>
        /// Change the origin of the drawing relative to the top-left coordinates of the drawing. String 1: Drawing name. Number 1: Origin X. Number 2: Origin Y.
        /// </summary>
        DrawingOrigin,

        /// <summary>
        /// Change the layer depth of a drawing. String 1: Drawing name. Number 1: Layer depth.
        /// </summary>
        DrawingLayerDepth,

        /// <summary>
        /// Drawing sprite effects. String 1: Drawing name. String 2: Sprite effects. Sprite effects can be None, FlipHorizontal, FlipVertical, or FlipBoth.
        /// </summary>
        DrawingSpriteEffects,

        /// <summary>
        /// Change the X scale of a drawing. String 1: Drawing name. Number 1: X Scale.
        /// </summary>
        DrawingScaleX,

        /// <summary>
        /// Change the Y scale of a drawing. String 1: Drawing name. Number 1: Y Scale.
        /// </summary>
        DrawingScaleY,

        /// <summary>
        /// Change the color of the drawing. String 1: Drawing name. Int 1: Red. Int 2: Green. Int 3: Blue. Int 4 (Optional): Alpha.
        /// </summary>
        DrawingColor,

        /// <summary>
        /// Fade the drawing. String 1: Drawing name. Number 1: Time in milliseconds to fade.
        /// </summary>
        FadeDrawing,

        /// <summary>
        /// Unfade the drawing. String 1: Drawing name. Number 1: Time in milliseconds to unfade.
        /// </summary>
        UnfadeDrawing,

        /// <summary>
        /// Show a drawing. String 1: Drawing name.
        /// </summary>
        ShowDrawing,

        /// <summary>
        /// Hide a drawing. String 1: Drawing name.
        /// </summary>
        HideDrawing,

        /// <summary>
        /// Maximize a drawing by setting its width and height to the resolution's. String 1: Drawing name.
        /// </summary>
        MaximizeDrawing,

        /// <summary>
        /// If the drawing is a sprite, this can be used to change the current sprite group. String 1: Drawing name. String 2: Sprite group to change to.
        /// </summary>
        ChangeDrawingSpriteGroup,

        /// <summary>
        /// Unload a drawing. String 1: Name.
        /// </summary>
        UnloadDrawing,

        //Debating.
        LoadFont,
        StringIndentation,
        StringAlignment,
        StringScale,
        String,
        Message,
        UnloadFont,

        /// <summary>
        /// Wait for a certain game button to be down. String 1: Game button to wait for.
        /// </summary>
        WaitForButton,

        /// <summary>
        /// Wait for any game buttons to be down.
        /// </summary>
        WaitForAnyButton,

        /// <summary>
        /// Push a button down. String 1: Game button to push.
        /// </summary>
        ButtonDown,

        /// <summary>
        /// Set a button up. String 1: Game button to unpush.
        /// </summary>
        ButtonUp,

        /// <summary>
        /// Push a button. String 1: Game button to push.
        /// </summary>
        PushButton,

        /// <summary>
        /// Block input. String 1: Game button to block input from.
        /// </summary>
        BlockInput,

        /// <summary>
        /// Allow input. String 1: Game button to allow input to.
        /// </summary>
        AllowInput,

        /// <summary>
        /// Block inputs for all game buttons.
        /// </summary>
        BlockAllInputs,

        /// <summary>
        /// Allow inputs for all game buttons.
        /// </summary>
        AllowAllInputs,

        /// <summary>
        /// Signal an object.
        /// </summary>
        Signal,

        //Debating.
        X,
        Y,
        WaitTillPosition,
        Show,
        Hide,
        VelocityX,
        VelocityY,
        ApplyForceX,
        ApplyForceY,
        StopForceX,
        StopForceY,
        ChangeSpriteGroup,
        ChangeDrawing,
        EnableCollision,
        DisableCollision,

        //Debating.
        CameraX,
        CameraY,
        CameraVelocityX,
        CameraVelocityY,
        CameraAccelerationX,
        CameraAccelerationY,
        CameraWaitTillPosition,

        /// <summary>
        /// Exit the game.
        /// </summary>
        ExitGame,

        /// <summary>
        /// Change the scene. String 1: Scene to change to. Boolean 1 (Optional): Initialize scene.
        /// </summary>
        ChangeScene,

        /// <summary>
        /// Change the window size. Number 1: Width. Number 2: Height.
        /// </summary>
        ChangeSize,

        /// <summary>
        /// Change the window resolution. Number 1: Width. Number 2: Height. Boolean 1 (Optional):
        /// </summary>
        ChangeResolution,

        /// <summary>
        /// Set fullscreen mode. Boolean 1 (Optional): If fullscreen is enabled or not. If no boolean is present, the fullscreen toggles.
        /// </summary>
        Fullscreen,

        /// <summary>
        /// Force resolution on resize. Boolean 1 (Optional): If to force the current resolution on resize. If no boolean is present, it toggles.
        /// </summary>
        ForceResizeResolution,

        /// <summary>
        /// Set the window title. String 1: Text.
        /// </summary>
        WindowTitle,

        /// <summary>
        /// End the cutscene.
        /// </summary>
        Fin = 0xFF

    }

}
