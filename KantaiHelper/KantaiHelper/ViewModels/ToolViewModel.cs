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
using System.IO;
using KantaiHelper.Models;
using KantaiHelper.ViewModels.Setting;
using System.Collections.ObjectModel;
using System.Xml;

using KantaiHelper.Views.Behaviors;
using System.Windows;

namespace KantaiHelper.ViewModels
{
	class ToolViewModel : ViewModel
	{
		#region Fleets 변경 통지 프로퍼티
		private ObservableCollection<FleetShipViewModel> _Fleets;

		public ObservableCollection<FleetShipViewModel> Fleets
		{
			get
			{ return this._Fleets; }
			set
			{
				if (this._Fleets == value)
					return;
				this._Fleets = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region SelectedFleet 변경 통지 프로퍼티
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

		#region ExSlotChecked 변경 통지 프로퍼티
		public static bool ShowExSlot;

		public bool ExSlotChecked
		{
			get
			{ return ShowExSlot; }
			set
			{
				if (ShowExSlot == value)
					return;
				ShowExSlot = value;
				this.UpdateFleet();
				this.SaveFleets();
			}
		}
		#endregion

		#region Description 프로퍼티
		private readonly DragAcceptDescription _Description;

		public DragAcceptDescription Description
		{
			get { return this._Description; }
		}
		#endregion

		public ToolViewModel()
		{
			this._Description = new DragAcceptDescription();
			this._Description.DragOver += this.OnDragOver;
			this._Description.DragDrop += this.OnDragDrop;

			Fleets = new ObservableCollection<FleetShipViewModel>();
			LoadFleets();
		}

		public void ShowAddFleetWindow()
		{
			var fleetwd = new AddFleetWindowViewModel(this);
			var message = new TransitionMessage(fleetwd, TransitionMode.Normal, "AddFleetWindow.Show");
			this.Messenger.Raise(message);
		}

		public void ShowFleetSettingWindow()
		{
			if (SelectedFleet == null) return;
			var fleetwd = new FleetSettingWindowViewModel(this, this.SelectedFleet);
			var message = new TransitionMessage(fleetwd, TransitionMode.Normal, "FleetSettingWindow.Show");
			this.Messenger.Raise(message);
		}

		public void RequestDeleteFleet(FleetShipViewModel fleet)
		{
			var fleetwd = new DeleteFleetWindowViewModel(this, fleet);
			var message = new TransitionMessage(fleetwd, TransitionMode.Normal, "DeleteFleetWindow.Show");
			this.Messenger.Raise(message);
		}

		public void ShowExSlot_Click()
		{
			UpdateFleet();
		}

		#region Drag & Drop
		private void OnDragOver(DragEventArgs args)
		{
			if (args.AllowedEffects.HasFlag(DragDropEffects.Move) &&
				args.Data.GetDataPresent(typeof(FleetShipViewModel)))
			{
				args.Effects = DragDropEffects.Move;
			}
		}

		void OnDragDrop(DragEventArgs args)
		{
			// check and get data
			if (!args.Data.GetDataPresent(typeof(FleetShipViewModel))) return;
			var data = args.Data.GetData(typeof(FleetShipViewModel)) as FleetShipViewModel;
			if (data == null) return;
			var fe = args.OriginalSource as FrameworkElement;
			if (fe == null) return;
			var target = fe.DataContext as FleetShipViewModel;
			if (target == null) return;

			// move data
			var si = _Fleets.IndexOf(data);
			var di = _Fleets.IndexOf(target);
			if (si < 0 || di < 0 || si == di) return;
			Fleets.Move(si, di);

			SaveFleets();
		}
		#endregion

		private string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

		/// <summary>
		/// 함대 데이터를 불러옵니다.
		/// </summary>
		public void LoadFleets()
		{
			if (!Directory.Exists(Path.Combine(MainFolder, "KantaiHelper"))) return;

			var xmlPath = Path.Combine(MainFolder, "KantaiHelper/Fleets.xml");

			if (File.Exists(xmlPath))
			{
				XmlDocument xml = new XmlDocument();
				xml.Load(xmlPath);

				XmlNode root = xml.SelectSingleNode("fleets");

				var showexslot = root.Attributes["showexslot"];
				if (showexslot != null)
					ExSlotChecked = bool.Parse(showexslot.Value);

				foreach (XmlNode kantai in root.SelectNodes("fleet"))
				{
					var fleet = new FleetShipViewModel(this);
					fleet.FleetShipId = new List<int>();
					fleet.FleetSlotId = new List<List<int>>();
					fleet.FleetExSlotId = new List<int>();

					fleet.FleetName = kantai.Attributes["name"].Value;

					foreach (XmlNode shipid in kantai.SelectNodes("ship"))
					{
						fleet.FleetShipId.Add(int.Parse(shipid.Attributes["id"].Value));
						fleet.FleetExSlotId.Add(int.Parse(shipid.Attributes["exslotid"].Value));

						List<int> slot = new List<int>();
						foreach (XmlNode slotid in shipid.SelectNodes("slot"))
						{
							slot.Add(int.Parse(slotid.Attributes["id"].Value));
						}
						fleet.FleetSlotId.Add(slot);
					}

					Fleets.Add(fleet);
				}
			}
		}

		/// <summary>
		/// 함대 데이터를 저장합니다.
		/// </summary>
		public void SaveFleets()
		{
			if (!Directory.Exists(Path.Combine(MainFolder, "KantaiHelper")))
				Directory.CreateDirectory(Path.Combine(MainFolder, "KantaiHelper"));

			string path = Path.Combine(MainFolder, "KantaiHelper");

			XmlDocument xml = new XmlDocument();

			XmlNode root = xml.CreateElement("fleets");
			xml.AppendChild(root);

			XmlAttribute showexslot = xml.CreateAttribute("showexslot");
			showexslot.Value = ExSlotChecked ? bool.TrueString : bool.FalseString;
			root.Attributes.Append(showexslot);

			foreach (var fleet in Fleets)
			{
				XmlNode kantai = xml.CreateElement("fleet");
				XmlAttribute kantaiName = xml.CreateAttribute("name");

				kantaiName.Value = fleet.FleetName;

				kantai.Attributes.Append(kantaiName);
				root.AppendChild(kantai);

				int count = 0;
				foreach(var kantaishipid in fleet.FleetShipId)
				{
					XmlNode ship = xml.CreateElement("ship");
					XmlAttribute shipid = xml.CreateAttribute("id");
					XmlAttribute exslotid = xml.CreateAttribute("exslotid");

					shipid.Value = kantaishipid.ToString();
					exslotid.Value = fleet.FleetExSlotId[count].ToString();

					ship.Attributes.Append(shipid);
					ship.Attributes.Append(exslotid);
					kantai.AppendChild(ship);

					foreach (var kantaislotid in fleet.FleetSlotId[count])
					{
						XmlNode slot = xml.CreateElement("slot");
						XmlAttribute slotid = xml.CreateAttribute("id");

						slotid.Value = kantaislotid.ToString();

						slot.Attributes.Append(slotid);
						ship.AppendChild(slot);
					}
					count++;
				}
			}
			path += @"\Fleets.xml";
			xml.Save(path);
		}

        public void AddFleet(FleetShipViewModel fleet)
		{
			this.Fleets.Add(fleet);
			UpdateFleet(fleet);

			this.RaisePropertyChanged();

			SaveFleets();
		}

		public void ModifyFleet(FleetShipViewModel fleet)
		{
			UpdateFleet(fleet);
			this.RaisePropertyChanged();

			SaveFleets();
		}

		public void DeleteFleet(FleetShipViewModel fleet)
		{
			this.Fleets.Remove(fleet);
			this.RaisePropertyChanged();

			SaveFleets();
		}

		public void Initialize()
		{
			this.CompositeDisposable.Add(new PropertyChangedEventListener(KanColleClient.Current.Homeport.Organization)
			{
				{
					() => KanColleClient.Current.Homeport.Organization.Ships,
					(_, __) => { this.UpdateFleet(); }
				}
			});
		}

		private void UpdateFleet(FleetShipViewModel fleet)
		{
			fleet.UpdateFleetData();
		}

		private void UpdateFleet()
		{
			foreach (FleetShipViewModel fleet in Fleets)
			{
				fleet.UpdateFleetData();
			}
		}
	}
}
