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

		#region Ships 변경 통지 프로퍼티
		private ShipData[] _Ships;

		public ShipData[] Ships
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
				return this.FleetName+" " + FleetShipId[0] + ", " + FleetShipId[1] + ", " + FleetShipId[2];
			}protected set { throw new NotImplementedException(); }
		}

		public FleetShipViewModel(string fleetname, int[] fleetshipid)
		{
			this.FleetName = fleetname;
			this.FleetShipId = fleetshipid;
		}

		private void UpdateFleetData()
		{
			var organization = KanColleClient.Current.Homeport.Organization;
            this._Ships = organization.Ships.Where(x => this.FleetShipId.Any(t => x.Value.Id == t)).Select(s => new MembersShipData(s.Value)).ToArray();
		}
	}
}
