using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KantaiHelper.Models;
using Grabacr07.KanColleWrapper;
using Livet.EventListeners;
using Livet;
using Livet.Messaging.Windows;

namespace KantaiHelper.ViewModels.Fleet
{
	class FleetShipViewModel : TabItemViewModel
	{
		public ToolViewModel _ViewModel;

		public List<int> FleetShipId;

		public List<List<int>> FleetSlotId;

		public List<int> FleetExSlotId;

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

		#region Ships 변경 통지 프로퍼티
		private IEnumerable<ShipData> _Ships;

		public IEnumerable<ShipData> Ships
		{
			get
			{ return this._Ships; }
			set
			{
				if (this._Ships == value)
					return;
				this._Ships = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public override string Name
		{
			get
			{
				return this.FleetName;
			}protected set { throw new NotImplementedException(); }
		}

		public FleetShipViewModel(ToolViewModel viewmodel)
		{
			_ViewModel = viewmodel;
		}

		public void RequestDeleteFleet()
		{
			_ViewModel.RequestDeleteFleet(this);
		}

		public void UpdateFleetData()
		{
			if (KanColleClient.Current.IsStarted == false) return;
			var organization = KanColleClient.Current.Homeport.Organization;
			this._Ships = organization.Ships.Where(x => this.FleetShipId.Any(t => x.Value.Id == t)).Select(s => new MembersShipData(s.Value)).ToArray();

			foreach(ShipData ship in _Ships)
			{
				for(int i = 0; i < FleetShipId.Count(); i++)
				{
					if(ship.Id == FleetShipId[i])
					{
						if (this.FleetSlotId.Count() > i)
						{
							ship.ShipSlotId = this.FleetSlotId[i];
						}
						ship.ShipExSlotId = this.FleetExSlotId[i];

						ship.FleetNo = i + 1;
					}
				}
                ship.UpdateSlots();
            }
			Ships = this.Ships.OrderBy(x => x.FleetNo).ToArray();
		}
	}
}
