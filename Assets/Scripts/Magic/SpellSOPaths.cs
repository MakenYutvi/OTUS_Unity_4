using System.Collections.Generic;

public static class SpellSOPaths
{
    public static readonly Dictionary<SpellType, string> Spells =
        new Dictionary<SpellType, string>
    {
        {SpellType.FireBall, "ScriptableObjects/Spells/FireBall"},
        {SpellType.Lightning, "ScriptableObjects/Spells/Lightning"},


    };

}
