// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace EOS.UI.iOS.Components
{
	[Register ("WorkTimeCalendarCell")]
	partial class WorkTimeCalendarCell
	{
		[Outlet]
		UIKit.UIView cellContentView { get; set; }

		[Outlet]
		UIKit.UILabel dayLabel { get; set; }

		[Outlet]
		UIKit.UIView dayOffDevider { get; set; }

		[Outlet]
		UIKit.UILabel startBreakLabel { get; set; }

		[Outlet]
		UIKit.UILabel startWorkLabel { get; set; }

		[Outlet]
		UIKit.UILabel stopBreakLabel { get; set; }

		[Outlet]
		UIKit.UILabel stopWorkLabel { get; set; }

		[Outlet]
		UIKit.UIView weekDayDevider { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (dayLabel != null) {
				dayLabel.Dispose ();
				dayLabel = null;
			}

			if (dayOffDevider != null) {
				dayOffDevider.Dispose ();
				dayOffDevider = null;
			}

			if (startBreakLabel != null) {
				startBreakLabel.Dispose ();
				startBreakLabel = null;
			}

			if (startWorkLabel != null) {
				startWorkLabel.Dispose ();
				startWorkLabel = null;
			}

			if (stopBreakLabel != null) {
				stopBreakLabel.Dispose ();
				stopBreakLabel = null;
			}

			if (stopWorkLabel != null) {
				stopWorkLabel.Dispose ();
				stopWorkLabel = null;
			}

			if (cellContentView != null) {
				cellContentView.Dispose ();
				cellContentView = null;
			}

			if (weekDayDevider != null) {
				weekDayDevider.Dispose ();
				weekDayDevider = null;
			}
		}
	}
}
