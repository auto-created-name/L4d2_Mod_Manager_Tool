using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModLocalInfo
{
    public class LocalInfo
    {
        public int Id { get; private set; }
        public ImageFile AddonImage { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Tagline { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Category[] Categories { get; set; }

        public LocalInfo(int id)
        {
            Id = id;
        }

        public LocalInfo WithId(int id)
        {
            var clone = (LocalInfo)MemberwiseClone();
            clone.Id = id;
            return clone;
        }

        public LocalInfo()
        {
            Title = string.Empty;
            Version = string.Empty;
            Tagline = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
        }
    }
}
