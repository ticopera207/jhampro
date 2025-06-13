using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using PdfSharpCore.Fonts;

namespace jhampro.Service
{
    public class CustomFontResolver : IFontResolver
    {
        private readonly byte[] _fontData;

        public CustomFontResolver()
        {
            // Asegúrate de que el archivo esté presente
            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fuentes", "ArialCE.ttf");
            _fontData = File.ReadAllBytes(fontPath);
        }

        public byte[] GetFont(string faceName)
        {
            return _fontData;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("ArialCE#");
        }

        public string DefaultFontName => "ArialCE#";
    }
}