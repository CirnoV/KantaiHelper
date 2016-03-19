using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KantaiHelper.Models;
using Grabacr07.KanColleWrapper;
using Livet.EventListeners;
using Livet;

namespace KantaiHelper.ViewModels.Fleet
{
	class FleetShipViewModel : TabItemViewModel
	{
		public string FleetName;

		public int[] FleetShipId;

		public int[][] FleetItemId;

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

		public FleetShipViewModel() { }

		public FleetShipViewModel(string fleetname, int[] fleetshipid, int[][] fleetitemid)
		{
			this.FleetName = fleetname;
			this.FleetShipId = fleetshipid;
			this.FleetItemId = fleetitemid;
		}

		public void UpdateFleetData()
		{
			if (KanColleClient.Current.IsStarted == false) return;
			var organization = KanColleClient.Current.Homeport.Organization;
			this.Ships = organization.Ships.Where(x => this.FleetShipId.Any(t => x.Value.Id == t)).Select(s => new MembersShipData(s.Value)).ToArray();

			foreach(ShipData ship in Ships)
			{
				for(int i = 0; i < FleetShipId.Count(); i++)
				{
					if(ship.Id == FleetShipId[i])
					{
						if (this.FleetItemId.Count() > i)
						{
							ship.ShipItemId = this.FleetItemId[i];
						}
						ship.FleetNo = i + 1;
					}
				}
			}
			Ships = this.Ships.OrderBy(x => x.FleetNo).ToArray();
		}

		public void UpdateFleetItem()
		{
			foreach(ShipData ship in Ships)
			{
				ship.UpdateSlots();
			}
		}
	}
}
