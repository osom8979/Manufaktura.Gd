using Godot;
using Manufaktura.Controls.Gd;
using Manufaktura.Controls.Model;
using Manufaktura.Controls.Model.Fonts;
using Manufaktura.Controls.Rendering;
using Manufaktura.Controls.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Text;

public class ManufakturaCanvas : Control
{
    private ScoreRenderingModes RenderingMode { get; set; } = ScoreRenderingModes.Panorama;

    private GodotScoreRendererSettings _settings;
    private MusicXmlParser _parser;

    private Score _score;

    public override void _Ready()
    {
        _settings = new GodotScoreRendererSettings();
        _parser = new MusicXmlParser();
    }

    public override void _ExitTree()
    {
        // EMPTY.
    }

    public override void _Process(float delta)
    {
        if (_score == null)
        {
            return;
        }
        Update();
    }

    public override void _Draw()
    {
        if (_score == null)
        {
            return;
        }

        DrawRect(new Rect2(0, 0, RectSize.x, RectSize.y), Colors.WhiteSmoke);

        var renderer = new GodotCanvasScoreRenderer(this, _settings);
        renderer.Settings.PageWidth = RectSize.x;
        renderer.Settings.RenderingMode = RenderingMode;
        renderer.Render(_score);
    }

    public string ReadMusicMxlContainer(string path)
    {
        using (var zippedStream = new FileStream(path, FileMode.Open))
        {
            using (var archive = new ZipArchive(zippedStream))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.Name == "META-INF/container.xml")
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(ms);
                                var unzippedArray = ms.ToArray();
                                return Encoding.Default.GetString(unzippedArray);
                            }
                        }
                    }
                }
            }
        }
        throw new FileNotFoundException();
    }

    public string ReadMusicXml(string path)
    {
        using (var zippedStream = new FileStream(path, FileMode.Open))
        {
            using (var archive = new ZipArchive(zippedStream))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.Name == "META-INF/container.xml")
                    {
                    }

                    if (entry.Name == ".xml")
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(ms);
                                // return ms.ToArray();
                                var unzippedArray = ms.ToArray();
                                return Encoding.Default.GetString(unzippedArray);
                            }
                        }
                    }
                }
            }
        }
        throw new FileNotFoundException();
    }

    public bool Open(string path)
    {
        try
        {
            var xml = ReadMusicXml(path);
            _score = _parser.Parse(XDocument.Parse(xml));
            return true;
        }
        catch (FileNotFoundException exception)
        {
            GD.PrintErr(exception);
            return false;
        }
    }
}
