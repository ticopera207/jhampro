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
            _fontData = File.ReadAllBytes(Path.Combine("wwwroot", "fuentes", "ARIAL.ttf"));
        }

        public byte[] GetFont(string faceName)
        {
            return _fontData;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("ARIAL#");
        }

        public string DefaultFontName => "ARIAL#";
    }
}