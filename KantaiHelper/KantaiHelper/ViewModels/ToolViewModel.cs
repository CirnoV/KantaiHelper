using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Messaging;
using KantaiHelper.ViewModels.Fleet;

namespace KantaiHelper.ViewModels
{
	class ToolViewModel : ViewModel
	{
		#region  FleetShips의 변경 안내 프로퍼티
		private FleetShipViewModel[] _FleetShips;

		public FleetShipViewModel[] FleetShips
		{
			get
			{ return this._FleetShips; }
			set
			{
				if (this._FleetShips == value)
					return;
				this._FleetShips = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region  SelectShip의 변경 안내 프로퍼티
		private FleetShipViewModel[] _SelectShip;

		public FleetShipViewModel[] SelectShip
		{
			get
			{ return this._SelectShip; }
			set
			{
				if (this._SelectShip == value)
					return;
				this._SelectShip = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public ToolViewModel()
		{
			
		}
	}
}
