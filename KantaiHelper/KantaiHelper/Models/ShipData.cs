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

		#region Firepower  변경 통지 프로퍼티

		private int _Firepower;

		/// <summary>
		/// 火力ステータス値を取得します。
		/// </summary>
		public int Firepower
		{
			get { return this._Firepower; }
			set
			{
				this._Firepower = value;
				this.RaisePropertyChanged();

			}
		}

		#endregion

		#region Torpedo  변경 통지 프로퍼티

		private int _Torpedo;

		/// <summary>
		/// 雷装ステータス値を取得します。
		/// </summary>
		public int Torpedo
		{
			get { return this._Torpedo; }
			set
			{
				this._Torpedo = value;
				this.RaisePropertyChanged();

			}
		}

		#endregion

		#region AA  변경 통지 프로퍼티

		private int _AA;

		/// <summary>
		/// 対空ステータス値を取得します。
		/// </summary>
		public int AA
		{
			get { return this._AA; }
			set
			{
				this._AA = value;
				this.RaisePropertyChanged();
			}

		}

		#endregion

		#region Armer  변경 통지 프로퍼티

		private int _Armer;

		/// <summary>
		/// 装甲ステータス値を取得します。
		/// </summary>
		public int Armer
		{
			get { return this._Armer; }
			set
			{
				this._Armer = value;
				this.RaisePropertyChanged();

			}
		}

		#endregion

		#region Luck  변경 통지 프로퍼티

		private int _Luck;

		/// <summary>
		/// 運のステータス値を取得します。
		/// </summary>
		public int Luck
		{
			get { return this._Luck; }
			set
			{
				this._Luck = value;
				this.RaisePropertyChanged();
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

		public int SlotsFirepower => this.Slots.Sum(x => x.Firepower);
		public int SlotsTorpedo => this.Slots.Sum(x => x.Torpedo);
		public int SlotsAA => this.Slots.Sum(x => x.AA);
		public int SlotsArmer => this.Slots.Sum(x => x.Armer);
		public int SlotsASW => this.Slots.Sum(x => x.ASW);
		public int SlotsHit => this.Slots.Sum(x => x.Hit);
		public int SlotsEvade => this.Slots.Sum(x => x.Evade);

		public int SumFirepower => 0 < this.Firepower ? this.Firepower + this.SlotsFirepower : this.Firepower;
		public int SumTorpedo => 0 < this.Torpedo ? this.Torpedo + this.SlotsTorpedo : this.Torpedo;
		public int SumAA => 0 < this.AA ? this.AA + this.SlotsAA : this.AA;
		public int SumArmer => 0 < this.Armer ? this.Armer + this.SlotsArmer : this.Armer;

		public LimitedValue HP => new LimitedValue(this.NowHP, this.MaxHP, 0);

		public ShipData()
		{
			this._Name = "？？？";
			this._TypeName = "？？？";
			this._Situation = ShipSituation.None;
			this._Slots = new ShipSlotData[0];
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
			this.NowHP = this.Source.HP.Current;
			this.MaxHP = this.Source.HP.Maximum;
			this.Slots = this.Source.Slots
				.Where(s => s != null)
				.Where(s => s.Equipped)
				.Select(s => new ShipSlotData(s)).ToArray();
			this.ExSlot = new ShipSlotData(this.Source.ExSlot);

			this.Firepower = this.Source.Firepower.Current;
			this.Torpedo = this.Source.Torpedo.Current;
			this.AA = this.Source.AA.Current;
			this.Armer = this.Source.Armer.Current;
			this.Luck = this.Source.Luck.Current;
		}
	}
}
