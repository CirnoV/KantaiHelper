using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Livet;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper;
using KantaiHelper.ViewModels;

namespace KantaiHelper.Models
{
	public class ShipSlotData : NotificationObject
	{
		/// <summary>
		/// 해당 장비가 몇번째 슬롯인지를 나타냅니다.
		/// </summary>
		public int ShipSlotId;
		public int ShipId;

		public int SlotId;
		public int MastarId;

		public int Firepower { get; set; }
		public int Torpedo { get; set; }
		public int AA { get; set; }
		public int Armer { get; set; }
		public int Bomb { get; set; }
		public int ASW { get; set; }
		public int Hit { get; set; }
		public int Evade { get; set; }
		public int LOS { get; set; }

		public string NameWithLevel { get; set; }
		public SlotItemIconType IconType { get; set; }

		#region EquippedShipExist 변경 통지 프로퍼티
		private bool _EquippedShipExist;

		public bool EquippedShipExist
		{
			get
			{
				return _EquippedShipExist;
			}
			set
			{
				if (_EquippedShipExist == value)
					return;
				_EquippedShipExist = value;
				this.RaisePropertyChanged();
			}
		}
		//ToolViewModel.ShowExSlot && ExSlot != null
		#endregion

		#region EquippedShipName 변경 통지 프로퍼티
		private string _EquippedShipName;

		public string EquippedShipName
		{
			get
			{
				return _EquippedShipName;
			}
			set
			{
				if (_EquippedShipName == value)
					return;
				_EquippedShipName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public string Description => (this.Firepower != 0 ? "화력:" + this.Firepower : "")
								 + (this.Torpedo != 0 ? " 뇌장:" + this.Torpedo : "")
								 + (this.AA != 0 ? " 대공:" + this.AA : "")
								 + (this.Armer != 0 ? " 장갑:" + this.Armer : "")
								 + (this.Bomb != 0 ? " 폭장:" + this.Bomb : "")
								 + (this.ASW != 0 ? " 대잠:" + this.ASW : "")
								 + (this.Hit != 0 ? " 명중:" + this.Hit : "")
								 + (this.Evade != 0 ? " 회피:" + this.Evade : "")
								 + (this.LOS != 0 ? " 색적:" + this.LOS : "");

		public ShipSlotData(SlotItem item, int shipid)
		{
			this.ShipId = shipid;
			var info = item.Info;

			if (info == null) return;

			this.SlotId = item.Id;
			this.MastarId = item.Info.Id;

			this.NameWithLevel = item.NameWithLevel;
			this.IconType = item.Info.IconType;
			
			var ship = KanColleClient.Current.Homeport.Organization.Ships.Where(x => x.Value.Id != shipid && x.Value.EquippedItems.Where(y => y.Item.Id == item.Id).SingleOrDefault() != null).SingleOrDefault().Value;
			this.EquippedShipName = ship != null ? ("Lv. " + ship.Level + " " + ship.Info.Name) : null;
			EquippedShipExist = ToolViewModel.ShowEquippedShip && EquippedShipName != null;

			var m = HelperPlugin.RawStart2.api_mst_slotitem.SingleOrDefault(x => x.api_id == info.Id);
			if (m == null) return;
			this.Armer = m.api_souk;
			this.Firepower = m.api_houg;
			this.Torpedo = m.api_raig;
			this.Bomb = m.api_baku;
			this.AA = m.api_tyku;
			this.ASW = m.api_tais;
			this.Hit = m.api_houm;
			this.Evade = m.api_houk;
			this.LOS = m.api_saku;
		}
	}
}
