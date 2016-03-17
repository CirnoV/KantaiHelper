using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Livet;

namespace KantaiHelper.Models
{
	public class FleetData : NotificationObject
	{
		#region Name 변경 통지 프로퍼티
		private string _Name;

		public string Name
		{
			get
			{ return this._Name; }
			set
			{
				if (this._Name == value)
					return;
				this._Name = value;
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

		public FleetData(IEnumerable<ShipData> ships, string name)
		{
			this._Ships = ships;
			this._Name = name;
		}
	}
}
