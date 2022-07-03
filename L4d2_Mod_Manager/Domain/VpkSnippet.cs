using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace L4d2_Mod_Manager_Tool.Domain
{
    /// <summary>
    /// vpk文件简略信息，可能包含图片、说明
    /// </summary>
    public record VpkSnippet(string VpkName, Maybe<string> AddonImage, Maybe<string> AddonInfo);
}
