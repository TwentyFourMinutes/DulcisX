using StringyEnums;

namespace DulcisX.Core.Models.Enums
{
    public enum CopyToOutputDirectoryX
    {
        [StringRepresentation("Always")]
        Always,
        [StringRepresentation("Never")]
        Never,
        [StringRepresentation("PreserveNewest")]
        IfNewer
    }
}
