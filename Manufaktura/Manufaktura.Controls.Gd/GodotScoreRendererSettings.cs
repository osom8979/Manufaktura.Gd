using Godot;
using Manufaktura.Controls.Model.Fonts;
using Manufaktura.Controls.Rendering;
using System;
using System.Collections.Generic;

namespace Manufaktura.Controls.Gd
{
    public class GodotScoreRendererSettings : ScoreRendererSettings
    {
        // private readonly Dictionary<MusicFontStyles, float> fontSizes = new Dictionary<MusicFontStyles, float>()
        // {
        //         {MusicFontStyles.MusicFont, 27.5f},
        //         {MusicFontStyles.GraceNoteFont, 22},
        //         {MusicFontStyles.StaffFont, 30.5f},
        //         {MusicFontStyles.LyricsFont, 11},
        //         {MusicFontStyles.DirectionFont, 11},
        //         {MusicFontStyles.TimeSignatureFont, 14.5f}
        // };

        private readonly Dictionary<MusicFontStyles, DynamicFont> typefaces;

        private static DynamicFont CreateDynamicFont(DynamicFontData fontData, int size)
        {
            var font = new DynamicFont();
            font.FontData = fontData;
            font.Size = size;
            font.UseFilter = true;
            return font;
        }

        public GodotScoreRendererSettings()
        {
            var polihymniaFontData = ResourceLoader.Load<DynamicFontData>("res://Assets/Font/Polihymnia.ttf");
            var nanumFontData = ResourceLoader.Load<DynamicFontData>("res://Assets/Font/NanumBarunGothicBold.ttf");
            // var bravuraFontData = ResourceLoader.Load<DynamicFontData>("res://Assets/Font/Bravura.otf");
            // var bravuraTextFontData = ResourceLoader.Load<DynamicFontData>("res://Assets/Font/BravuraText.otf");

            typefaces = new Dictionary<MusicFontStyles, DynamicFont>()
            {
                {MusicFontStyles.MusicFont, CreateDynamicFont(polihymniaFontData, 27)},
                {MusicFontStyles.GraceNoteFont, CreateDynamicFont(polihymniaFontData, 22)},
                {MusicFontStyles.StaffFont, CreateDynamicFont(polihymniaFontData, 30)},
                {MusicFontStyles.LyricsFont, CreateDynamicFont(nanumFontData, 11)},
                {MusicFontStyles.DirectionFont, CreateDynamicFont(nanumFontData, 11)},
                {MusicFontStyles.TimeSignatureFont, CreateDynamicFont(nanumFontData, 14)}
            };
        }

        public DynamicFont GetFont(MusicFontStyles style)
        {
            if (!typefaces.ContainsKey(style))
            {
                throw new Exception($"Font information missing for style {style}");
            }

            return typefaces[style];
        }
    }
}