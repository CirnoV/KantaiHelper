using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Messaging;
using KantaiHelper.ViewModels.Fleet;
using Livet.EventListeners;
using Grabacr07.KanColleWrapper;
using MetroTrilithon.Mvvm;
using MetroTrilithon.Lifetime;

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

		#region SelectShip 변경 통지 프로퍼티
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
			_FleetShips = new FleetShipViewModel[1];
			_FleetShips[0] = new FleetShipViewModel("2전 3뇌순 1항", new int[6] { 6528, 192, 40, 456, 356, 734 }, new int[][] {
				new int[4] { 384, 8408, 739, 2236 },
				new int[4] { 1381, 1380, 607, 8410 } ,
				new int[3] { 4603, 362, 1488 },
				new int[3] { 8409, 4596, 165 },
				new int[3] { 597, 716, 166 },
				new int[4] { 4617, 4865, 7537, 1057 }
			});
			//_FleetShips[1] = new FleetShipViewModel("시작함", new int[3] { 1, 4, 6 }, new int[][] { new int[1] { 3 } });
			//_FleetShips[2] = new FleetShipViewModel("4잠", new int[4] { 6400, 294, 388, 3186 });
		}

		public void Initialize()
		{
			this.CompositeDisposable.Add(new PropertyChangedEventListener(KanColleClient.Current.Homeport.Organization)
			{
				{
					() => KanColleClient.Current.Homeport.Organization.Ships,
					(_, __) => { this.UpdateFleets(); this.UpdateFleetItems(); }
				}
			});
		}

		private void UpdateFleets()
		{
			foreach (FleetShipViewModel fleet in FleetShips)
			{
				fleet.UpdateFleetData();
			}
		}

		private void UpdateFleetItems()
		{
			foreach (FleetShipViewModel fleet in FleetShips)
			{
				fleet.UpdateFleetItem();
			}
		}
	}
}
