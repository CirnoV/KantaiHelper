using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantaiHelper.ViewModels.Fleet
{
	class FleetShipViewModel : TabItemViewModel
	{
		public readonly string Text;

		public override string Name
		{
			get
			{
				return Text;
			}protected set { throw new NotImplementedException(); }
		}

		public FleetShipViewModel(string text)
		{
			this.Text = text;
		}
	}
}
