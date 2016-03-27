using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MetroTrilithon.Mvvm;
using Grabacr07.KanColleWrapper;
using KantaiHelper.ViewModels.Fleet;
using Livet.Messaging.Windows;

namespace KantaiHelper.ViewModels.Setting
{
	class AddFleetWindowViewModel : WindowViewModel
	{
		private ToolViewModel _ViewModel;

		#region FleetName 변경 통지 프로퍼티
		private string _FleetName;

		public string FleetName
		{
			get
			{ return this._FleetName; }
			set
			{
				if (this._FleetName == value)
					return;
				this._FleetName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public AddFleetWindowViewModel(ToolViewModel viewmodel)
		{
			this._ViewModel = viewmodel;
			this._FleetName = "";
		}

		public void AddByFleet(string data)
		{
			int deckid;
			if (!int.TryParse(data, out deckid)) return;

			if (deckid > 0)
			{
				var KanColleFleets = KanColleClient.Current.Homeport.Organization.Fleets;
				if (KanColleClient.Current.IsStarted == false || KanColleFleets.Count < deckid || KanColleFleets[deckid].Ships.Count() < 1) return;

				var fleet = new FleetShipViewModel(_ViewModel);
				fleet.FleetName = this.FleetName == "" ? "이름없는 함대" : this.FleetName;

				var fleetships = KanColleClient.Current.Homeport.Organization.Fleets.Where(x => x.Value.Id == deckid).Single().Value.Ships;
				fleet.FleetShipId = fleetships.Select(y => y.Id).ToList();
				fleet.FleetSlotId = fleetships.Select(y => y.Slots.Select(z => z.Item.Id).ToList()).ToList();
				fleet.FleetExSlotId = fleetships.Select(y => y.ExSlot.Item.Id).ToList();

				this._ViewModel.AddFleet(fleet);

				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
			}
		}
	}
}
