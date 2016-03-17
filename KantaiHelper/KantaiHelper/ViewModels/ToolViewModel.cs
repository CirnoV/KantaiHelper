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
		#region  FleetShips 변경 통지 프로퍼티
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

		#region  SelectShip 변경 통지 프로퍼티
		private FleetShipViewModel _SelectedFleet;

		public FleetShipViewModel SelectedFleet
		{
			get
			{ return this._SelectedFleet; }
			set
			{
				if (this._SelectedFleet == value)
					return;
				this._SelectedFleet = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public ToolViewModel()
		{
			/*_FleetShips = new FleetShipViewModel[3];
			_FleetShips[0] = new FleetShipViewModel("황금함머");
			_FleetShips[1] = new FleetShipViewModel("3-2-1 레벨링");
			_FleetShips[2] = new FleetShipViewModel("트롤 세팅");*/
		}
	}
}
