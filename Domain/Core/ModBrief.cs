using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public class ModBrief
    {
        private const string NotName = "<无名称>";
        private const string NotAuthor = "<未知作者>";
        private const string NotTagline = "<无简介>";

        public int Id { get;private set; }
        public VpkId VpkId { get; set; }
        public string Name { get; set; }
        public string ReadableName => string.IsNullOrEmpty(Name) ? NotName : Name;
        public string FileName { get; set; }
        public bool Enabled { get; set; }
        public string ReadableEnabled => Enabled ? "启用" : "禁用";
        public string Author { get; set; }
        public string ReadableAuthor => string.IsNullOrEmpty(Author) ? NotAuthor : Author;
        public string Tagline { get; set; }
        public string ReadableTagline => string.IsNullOrEmpty(Tagline) ? NotTagline : Tagline;
        public string Tags { get; set; }
        public string Categories { get; set; }

        public ModBrief(int id)
        {
            Id = id;
            Author = string.Empty;
            Categories = string.Empty;
            Enabled = false;
            FileName = string.Empty;
            Name = string.Empty;
            Tagline = string.Empty;
            Tags = string.Empty;
        }

        public static ModBrief Default
            => new(0)
            {
                VpkId = VpkId.Undefined,
                Author = string.Empty,
                Categories = string.Empty,
                Enabled = false,
                FileName = string.Empty,
                Name = string.Empty,
                Tagline = string.Empty,
                Tags = string.Empty
            };
    }
}
