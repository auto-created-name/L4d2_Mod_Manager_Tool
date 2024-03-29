﻿using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;

namespace L4d2_Mod_Manager_Tool.Domain
{
    /// <summary>
    /// Entity - 模组信息
    /// </summary>
    public record ModInfo(
        Maybe<string> Title,
        Maybe<string> Version,
        Maybe<string> Tagline,
        Maybe<string> Author,
        Maybe<string> Description,
        ImmutableArray<string> Categories
        );
}
