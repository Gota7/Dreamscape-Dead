using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A way to render stuff to a render target instead of the main screen.
    /// </summary>
    public class RenderBatch {

        /// <summary>
        /// Render targets.
        /// </summary>
        private ExpandablePool<RenderBatchEntry> RTs = new ExpandablePool<RenderBatchEntry>();

        /// <summary>
        /// Render target pointer.
        /// </summary>
        private int RenderTargetPtr = -1;

        /// <summary>
        /// Render target.
        /// </summary>
        private RenderBatchEntry RT => RTs.ElementAt(RenderTargetPtr);

        /// <summary>
        /// Begin the render batch.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="sortMode">Sort mode.</param>
        /// <param name="blendState">Blend state.</param>
        /// <param name="samplerState">Sampler state.</param>
        /// <param name="depthStencilState">Depth stencil state.</param>
        /// <param name="rasterizerState">Rasterizer state.</param>
        /// <param name="effect">Effect.</param>
        /// <param name="transformMatrix">Transform matrix.</param>
        public void Begin(int width, int height, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null) {
            var e = RTs.Where(x => x.RenderTarget.Width == width && x.RenderTarget.Height == height && x.Used == false).FirstOrDefault();
            if (e == null) {
                var n = RTs.GetObject(new RenderBatchEntryParameters(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix));
                n.RenderTarget = new RenderTarget2D(GameHelper.Graphics, width, height);
                RenderTargetPtr = RTs.IndexOf(n);
            } else {
                e.Initialize(new RenderBatchEntryParameters(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix));
                RenderTargetPtr = RTs.IndexOf(e);
            }
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
        /// <param name="color">Color.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color) {
            Draw(texture, null, destinationRectangle, null, null, 0, null, color, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="color">Color.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color) {
            Draw(texture, position, null, null, null, 0, null, color, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="color">Color.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color) {
            Draw(texture, position, null, sourceRectangle, null, 0, null, color, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="color">Color.</param>
        /// <param name="effects">Effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) {
            Draw(texture, null, destinationRectangle, sourceRectangle, origin, rotation, null, color, effects, layerDepth);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="color">Color.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color) {
            Draw(texture, null, destinationRectangle, sourceRectangle, null, 0, null, color, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="color">Color.</param>
        /// <param name="effects">Effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            Draw(texture, position, null, sourceRectangle, origin, rotation, scale, color, effects, layerDepth);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="color">Color.</param>
        /// <param name="effects">Effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            Draw(texture, position, null, sourceRectangle, origin, rotation, new Vector2(scale, scale), color, effects, layerDepth);
        }

        /// <summary>
        /// Draw.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="color">Color.</param>
        /// <param name="effects">Effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public void Draw(Texture2D texture, Vector2? position = null, Rectangle? destinationRectangle = null, Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0, Vector2? scale = null, Color? color = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0) {
            RT.DrawCommands.Enqueue(new RenderBatchDrawEntry(texture, position, destinationRectangle, sourceRectangle, origin, rotation, scale, color, effects, layerDepth));
        }

        /// <summary>
        /// End the render batch.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="ignoreCamera">Whether or not to ignore the camera.</param>
        /// <param name="color">Color to draw over.</param>
        /// <param name="origin">Origin of the drawing.</param>
        /// <param name="rotation">Rotation of the drawing.</param>
        /// <param name="scale">Scaling of the drawing.</param>
        /// <param name="spriteEffects">Sprite effects.</param>
        /// <param name="layerDepth">Depth to draw at.</param>
        public void End(float x = 0, float y = 0, bool ignoreCamera = false, Color? color = null, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {
            if (RenderTargetPtr != -1 && RT != null && RT.RenderTarget != null) {
                GameHelper.SpriteBatch.Draw(RT.RenderTarget, new Vector2(x, y), null, null, origin, rotation == null ? 0 : (float)rotation.Radians, scale, color, spriteEffects, layerDepth);
            }
            RenderTargetPtr = -1;
        }

        /// <summary>
        /// Render all the batches. Use this somewhere after the spritebatch is done.
        /// </summary>
        public void Render() {

            //Render used objects.
            foreach (var r in RTs.Where(x => x.Used)) {
                r.Render();
            }

            //Remove unused entries.
            foreach (var r in RTs) {
                if (!r.Used) {
                    RTs.RecycleObject(r);
                } else {
                    r.Used = false;
                }
            }

        }

    }

    /// <summary>
    /// A render target entry.
    /// </summary>
    public class RenderBatchEntry : IPoolable {

        /// <summary>
        /// Render target.
        /// </summary>
        public RenderTarget2D RenderTarget;

        /// <summary>
        /// If the entry was used.
        /// </summary>
        public bool Used;

        /// <summary>
        /// Spritebatch parameters.
        /// </summary>
        public RenderBatchEntryParameters Params;

        /// <summary>
        /// Draw commands.
        /// </summary>
        public Queue<RenderBatchDrawEntry> DrawCommands = new Queue<RenderBatchDrawEntry>();

        /// <summary>
        /// Initialize the render batch entry.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void Initialize(object parameters) {
            Params = (RenderBatchEntryParameters)parameters;
            Used = true;
        }

        /// <summary>
        /// Remove expensive data.
        /// </summary>
        public void DeInitialize() {
            RenderTarget.Dispose();
        }

        /// <summary>
        /// Render the data.
        /// </summary>
        public void Render() {
            GameHelper.Graphics.SetRenderTarget(RenderTarget);
            GameHelper.SpriteBatch.Begin(Params.SortMode, Params.BlendState, Params.SamplerState, Params.DepthStencilState, Params.RasterizerState, Params.Effect, Params.TransformMatrix);
            GameHelper.Graphics.Clear(Color.Transparent);
            while (DrawCommands.Count > 0) {
                var d = DrawCommands.Dequeue();
                GameHelper.SpriteBatch.Draw(d.Texture, d.Position, d.DestinationRectangle, d.SourceRectangle, d.Origin, d.Rotation, d.Scale, d.Color, d.Effects, d.LayerDepth);
            }
            GameHelper.SpriteBatch.End();
            GameHelper.Graphics.SetRenderTarget(null);
        }

        /// <summary>
        /// All entries are important.
        /// </summary>
        /// <returns>0.</returns>
        public int Rank() => 0;

    }

    /// <summary>
    /// Render batch entry parameters.
    /// </summary>
    public class RenderBatchEntryParameters {

        /// <summary>
        /// Sort mode.
        /// </summary>
        public SpriteSortMode SortMode;

        /// <summary>
        /// Blend state.
        /// </summary>
        public BlendState BlendState;

        /// <summary>
        /// Sampler state.
        /// </summary>
        public SamplerState SamplerState;

        /// <summary>
        /// Depth stencil state.
        /// </summary>
        public DepthStencilState DepthStencilState;

        /// <summary>
        /// Rasterizer state.
        /// </summary>
        public RasterizerState RasterizerState;

        /// <summary>
        /// Effect.
        /// </summary>
        public Effect Effect;

        /// <summary>
        /// Transform matrix.
        /// </summary>
        public Matrix? TransformMatrix;

        /// <summary>
        /// Create a new render batch entry.
        /// </summary>
        /// <param name="sortMode">Sort mode.</param>
        /// <param name="blendState">Blend state.</param>
        /// <param name="samplerState">Sampler state.</param>
        /// <param name="depthStencilState">Depth stencil state.</param>
        /// <param name="rasterizerState">Rasterizer state.</param>
        /// <param name="effect">Effect.</param>
        /// <param name="transformMatrix">Transform matrix.</param>
        public RenderBatchEntryParameters(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null) {
            SortMode = sortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            Effect = effect;
            TransformMatrix = transformMatrix;
        }

    }

    /// <summary>
    /// Render batch draw entry.
    /// </summary>
    public class RenderBatchDrawEntry {

        /// <summary>
        /// Texture.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Position.
        /// </summary>
        public Vector2? Position;

        /// <summary>
        /// Destination rectangle.
        /// </summary>
        public Rectangle? DestinationRectangle;

        /// <summary>
        /// Source rectangle.
        /// </summary>
        public Rectangle? SourceRectangle;

        /// <summary>
        /// Origin.
        /// </summary>
        public Vector2? Origin;

        /// <summary>
        /// Rotation.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Scale.
        /// </summary>
        public Vector2? Scale;

        /// <summary>
        /// Color.
        /// </summary>
        public Color? Color;

        /// <summary>
        /// Effects.
        /// </summary>
        public SpriteEffects Effects;

        /// <summary>
        /// Layer depth.
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// A render batch draw entry.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="position">Position.</param>
        /// <param name="destinationRectangle">Destination rectangle.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="color">Color.</param>
        /// <param name="effects">Effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public RenderBatchDrawEntry(Texture2D texture, Vector2? position = null, Rectangle? destinationRectangle = null, Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0, Vector2? scale = null, Color? color = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0) {
            Texture = texture;
            Position = position;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Origin = origin;
            Rotation = rotation;
            Scale = scale;
            Color = color;
            Effects = effects;
            LayerDepth = layerDepth;
        }

    }

}
