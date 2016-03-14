using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantaiHelper.ViewModels.Fleet
{
	class FleetShipViewModel : TabItemViewModel
	{
		private Random Random = new Random();

		public override string Name
		{
			get
			{
				return "테스트";
			}protected set { throw new NotImplementedException(); }
		}
	}
}
