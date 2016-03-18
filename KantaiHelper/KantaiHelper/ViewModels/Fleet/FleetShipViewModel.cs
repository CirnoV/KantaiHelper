using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KantaiHelper.Models;
using Grabacr07.KanColleWrapper;

namespace KantaiHelper.ViewModels.Fleet
{
	class FleetShipViewModel : TabItemViewModel
	{
		public IEnumerable<int> FleetShipId;

		#region Fleet 변경 통지 프로퍼티
		private FleetData _Fleet;

		public FleetData Fleet
		{
			get
			{ return this._Fleet; }
			set
			{
				if (this._Fleet == value)
					return;
				this._Fleet = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public override string Name
		{
			get
			{
				return this._Fleet.Name;
			}protected set { throw new NotImplementedException(); }
		}

		public FleetShipViewModel(string fleetname, IEnumerable<int> fleetshipid)
		{
			this.FleetShipId = fleetshipid;

			UpdateFleetData(fleetname);
		}

		private void UpdateFleetData(string fleetname)
		{
			var organization = KanColleClient.Current.Homeport.Organization;
            /*this._Fleet = new FleetData(
				FleetShipId.Count() > 0
					? organization.Ships.Select(s => new MembersShipData(s.Value)).ToArray()
					: new MembersShipData[0],
				fleetname);*/

            this._Fleet = new FleetData(
                this.FleetShipId.Count() > 0
                    ? organization.Ships.Where(x => this.FleetShipId.Any(t => x.Key == t)).Select(s => new MembersShipData(s.Value)).ToArray()
                    : new MembersShipData[0], fleetname);
		}
	}
}
