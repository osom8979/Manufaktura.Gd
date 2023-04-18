using Godot;
using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Model;
using Manufaktura.Controls.Model.Fonts;
using Manufaktura.Controls.Primitives;
using Manufaktura.Controls.Rendering;
using System;

namespace Manufaktura.Controls.Gd
{
    public class GodotCanvasScoreRenderer : ScoreRenderer<CanvasItem>
    {
        public GodotScoreRendererSettings TypedSettings => Settings as GodotScoreRendererSettings;

        public GodotCanvasScoreRenderer(CanvasItem canvas, GodotScoreRendererSettings settings) : base(canvas, settings)
        {
            // EMPTY.
        }

        public override void DrawArc(Rectangle rect, double startAngle, double sweepAngle, Pen pen, MusicalSymbol owner)
        {
            if (!EnsureProperPage(owner))
            {
                return;
            }

            if (Settings.RenderingMode != ScoreRenderingModes.Panorama)
            {
                rect = rect.Translate(CurrentScore.DefaultPageSettings);
            }

            var centerX = rect.X + (rect.Width / 2);
            var centerY = rect.Y + (rect.Height / 2);
            var center = new Vector2((float)centerX, (float)centerY);
            var radius = rect.Width / 2;
            var pointCount = 10;
            Godot.Color color = new Godot.Color(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
            float width = (float)pen.Thickness;
            Canvas.DrawArc(center, (float)radius, (float)startAngle, (float)sweepAngle, pointCount, color, width);
        }

        public override void DrawBezier(Point p1, Point p2, Point p3, Point p4, Pen pen, MusicalSymbol owner)
        {
            if (!EnsureProperPage(owner))
            {
                return;
            }

            if (Settings.RenderingMode != ScoreRenderingModes.Panorama)
            {
                p1 = p1.Translate(CurrentScore.DefaultPageSettings);
                p2 = p2.Translate(CurrentScore.DefaultPageSettings);
                p3 = p3.Translate(CurrentScore.DefaultPageSettings);
                p4 = p4.Translate(CurrentScore.DefaultPageSettings);
            }
        }

        public override bool CanDrawCharacterInBounds => throw new NotImplementedException();

        public override void DrawCharacterInBounds(char character, MusicFontStyles fontStyle, Point location, Size size, Manufaktura.Controls.Primitives.Color color, MusicalSymbol owner)
        {
            if (!EnsureProperPage(owner))
            {
                return;
            }

            if (Settings.RenderingMode != ScoreRenderingModes.Panorama)
            {
                location = location.Translate(CurrentScore.DefaultPageSettings);
            }

            GD.Print("DrawCharacterInBounds: " + character.ToString());
        }

        public override void DrawLine(Point startPoint, Point endPoint, Pen pen, MusicalSymbol owner)
        {
            if (!EnsureProperPage(owner))
            {
                return;
            }

            if (Settings.RenderingMode != ScoreRenderingModes.Panorama)
            {
                startPoint = startPoint.Translate(CurrentScore.DefaultPageSettings);
                endPoint = endPoint.Translate(CurrentScore.DefaultPageSettings);
            }

            Vector2 p1 = new Vector2((float)startPoint.X, (float)startPoint.Y);
            Vector2 p2 = new Vector2((float)endPoint.X, (float)endPoint.Y);
            Godot.Color color = new Godot.Color(pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.A);
            float width = (float)pen.Thickness;

            Canvas.DrawLine(p1, p2, color, width, true);
        }

        public override void DrawString(string text, MusicFontStyles fontStyle, Point location, Manufaktura.Controls.Primitives.Color color, MusicalSymbol owner)
        {
            if (!EnsureProperPage(owner))
            {
                return;
            }

            if (Settings.RenderingMode != ScoreRenderingModes.Panorama)
            {
                location = location.Translate(CurrentScore.DefaultPageSettings);
            }

            var font = TypedSettings.GetFont(fontStyle);
            // font.GetDescent();
            var baselineDesignUnits = font.GetAscent();
            // var baselinePixels = (baselineDesignUnits * font.Size) / font.

            var pos = new Vector2((float)location.X, (float)location.Y);
            Godot.Color c = new Godot.Color(color.R, color.G, color.B, color.A);
            Canvas.DrawString(font, pos, text, c);
        }

        protected override void DrawPlaybackCursor(PlaybackCursorPosition position, Point start, Point end)
        {
            GD.Print("DrawPlaybackCursor");
        }
    }
}