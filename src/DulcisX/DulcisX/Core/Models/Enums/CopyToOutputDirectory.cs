using StringyEnums;

namespace DulcisX.Core.Models.Enums
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
