using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueStory.Common.Mappers
{
    public static class FormatingMapper
    {
        public static List<string> FormatGuidListToString(this List<Guid> list)
        {
            return list.Select(x => x.ToString("N").ToLower()).ToList();
        }

        public static string FormatGuidToString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
