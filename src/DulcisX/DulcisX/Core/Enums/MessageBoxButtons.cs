using StringyEnums;
using System;

namespace DulcisX.Core.Enums
{
	[Flags]
	public enum MessageBoxButtons
	{
		[StringRepresentation("Ok")]
		Ok = 1 << 0,
		[StringRepresentation("Cancel")]
		Cancel = 1 << 1,
		[StringRepresentation("Retry")]
		Retry = 1 << 2,
		[StringRepresentation("Ignore")]
		Ignore = 1 << 3,
		[StringRepresentation("Abort")]
		Abort = 1 << 4,
		[StringRepresentation("Yes")]
		Yes = 1 << 5,
		[StringRepresentation("No")]
		No = 1 << 6,

		YesNo = Yes | No,
		OkCancel = Ok | Cancel,
		AbortRetryIgnore = Abort | Retry | Ignore,
		YesNoCancel = YesNo | Cancel,
		RetryCancel = Retry | Cancel
	}
}
