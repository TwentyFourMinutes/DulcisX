using StringyEnums;

namespace DulcisX.Core.Enums
{
    public enum CopyToOutputDirectory
    {
        [StringRepresentation("Always")]
        Always,
        [StringRepresentation("Never")]
        Never,
        [StringRepresentation("PreserveNewest")]
        IfNewer
    }
}
