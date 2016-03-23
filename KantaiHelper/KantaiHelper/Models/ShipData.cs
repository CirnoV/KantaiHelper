using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantaiHelper.Models
{
	public class ShipData : NotificationObject
	{
		public List<int> ShipSlotId;
		public int ShipExSlotId;

		#region FleetNo 변경 통지 프로퍼티
		private int _FleetNo;

		public int FleetNo
		{
			get
			{ return this._FleetNo; }
			set
			{
				if (this._FleetNo == value)
					return;
				this._FleetNo = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Id 변경 통지 프로퍼티
		private int _Id;

		public int Id
		{
			get
			{ return this._Id; }
			set
			{
				if (this._Id == value)
					return;
				this._Id = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

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

		#region TypeName 변경 통지 프로퍼티
		private string _TypeName;

		public string TypeName
		{
			get
			{ return this._TypeName; }
			set
			{
				if (this._TypeName == value)
					return;
				this._TypeName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Level 변경 통지 프로퍼티
		private int _Level;
		public int Level
		{
			get { return this._Level; }
			set
			{
				if (this._Level == value)
					return;
				this._Level = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Situation 변경 통지 프로퍼티
		private ShipSituation _Situation;

		public ShipSituation Situation
		{
			get
			{ return _Situation; }
			set
			{
				if (_Situation == value)
					return;
				_Situation = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region ConditionType 변경 통지 프로퍼티
		private ConditionType _ConditionType;

		public ConditionType ConditionType
		{
			get
			{ return _ConditionType; }
			set
			{
				if (_ConditionType == value)
					return;
				_ConditionType = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region Condition 변경 통지 프로퍼티
		private int _Condition;

		public int Condition
		{
			get
			{ return _Condition; }
			set
			{
				if (_Condition == value)
					return;
				_Condition = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region MaxHP 변경 통지 프로퍼티
		private int _MaxHP;
		public int MaxHP
		{
			get { return this._MaxHP; }
			set
			{
				if (this._MaxHP == value)
					return;
				this._MaxHP = value;
				this.RaisePropertyChanged();
				this.RaisePropertyChanged(() => this.HP);
			}
		}
		#endregion

		#region NowHP 변경 통지 프로퍼티
		private int _NowHP;
		public int NowHP
		{
			get { return this._NowHP; }
			set
			{
				if (this._NowHP == value)
					return;
				this._NowHP = value;
				this.RaisePropertyChanged();
				this.RaisePropertyChanged(() => this.HP);
			}
		}
		#endregion

		#region Slots 변경 통지 프로퍼티
		private IEnumerable<ShipSlotData> _Slots;

		public IEnumerable<ShipSlotData> Slots
		{
			get
			{ return this._Slots; }
			set
			{
				if (this._Slots == value)
					return;
				this._Slots = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public bool ExSlotEquipped => ExSlot != null;

		#region ExSlot 변경 통지 프로퍼티
		private ShipSlotData _ExSlot;

		public ShipSlotData ExSlot
		{
			get
			{ return this._ExSlot; }
			set
			{
				if (this._ExSlot == value)
					return;
				this._ExSlot = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public LimitedValue HP => new LimitedValue(this.NowHP, this.MaxHP, 0);

		public ShipData()
		{
			this._Name = "？？？";
			this._TypeName = "？？？";
			this._Situation = ShipSituation.None;
			this._Slots = new ShipSlotData[0];
		}
		
		public void UpdateSlots()
		{
			if (KanColleClient.Current.IsStarted == false) return;
			if (ShipSlotId == null) return;
			var itemYard = KanColleClient.Current.Homeport.Itemyard;
			this._Slots = itemYard.SlotItems.Where(x => this.ShipSlotId.Any(t => x.Value.Id == t)).Select(s => new ShipSlotData(s.Value)).ToArray();

			foreach (ShipSlotData slot in _Slots)
			{
				for (int i = 0; i < ShipSlotId.Count(); i++)
				{
					if (slot.SlotId == ShipSlotId[i])
					{
						slot.ShipSlotId = i;
					}
				}
			}
			this.Slots = this.Slots.OrderBy(x => x.ShipSlotId).ToArray();

			var exSlotItem = itemYard.SlotItems.SingleOrDefault(x => x.Value.Id == ShipExSlotId).Value;
			if(exSlotItem != null)
			{
				this.ExSlot = new ShipSlotData(exSlotItem);
			}
		}
	}

	public class MembersShipData : ShipData
	{
		#region Source 변경 통지 프로퍼티
		private Ship _Source;

		public Ship Source
		{
			get
			{ return this._Source; }
			set
			{
				if (this._Source == value)
					return;
				this._Source = value;
				this.RaisePropertyChanged();
				this.UpdateFromSource();
			}
		}
		#endregion

		public MembersShipData()
		{
		}

		public MembersShipData(Ship ship) : this()
		{
			this._Source = ship;
			this.UpdateFromSource();
		}

		private void UpdateFromSource()
		{
			this.Id = this.Source.Id;
			this.Name = this.Source.Info.Name;
			this.TypeName = this.Source.Info.ShipType.Name;
			this.Level = this.Source.Level;
			this.Situation = this.Source.Situation;
			this.ConditionType = this.Source.ConditionType;
			this.Condition = this.Source.Condition;
			this.NowHP = this.Source.HP.Current;
			this.MaxHP = this.Source.HP.Maximum;

			/*this.Slots = this.Source.Slots
				.Where(s => s != null)
				.Where(s => s.Equipped)
				.Select(s => new ShipSlotData(s)).ToArray();
			this.ExSlot = new ShipSlotData(this.Source.ExSlot);*/
		}
	}
}
