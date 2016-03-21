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

		public ToolViewModel()
		{
			Fleets = new ObservableCollection<FleetShipViewModel>();
			LoadFleets();
			/*
			_FleetShips = new FleetShipViewModel[1];
			_FleetShips[0] = new FleetShipViewModel("2전 3뇌순 1항", new int[6] { 6528, 192, 40, 456, 356, 734 }, new int[][] {
				new int[4] { 384, 8408, 739, 2236 },
				new int[4] { 1381, 1380, 607, 8410 } ,
				new int[3] { 4603, 362, 1488 },
				new int[3] { 8409, 4596, 165 },
				new int[3] { 597, 716, 166 },
				new int[4] { 4617, 4865, 7537, 1057 }
			});
			*/
			//_FleetShips[1] = new FleetShipViewModel("시작함", new int[3] { 1, 4, 6 }, new int[][] { new int[1] { 3 } });
			//_FleetShips[2] = new FleetShipViewModel("4잠", new int[4] { 6400, 294, 388, 3186 });
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

		private string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

		public void LoadFleets()
		{
			if (!Directory.Exists(Path.Combine(MainFolder, "KantaiHelper")))
				Directory.CreateDirectory(Path.Combine(MainFolder, "KantaiHelper"));

			var csvPath = Path.Combine(MainFolder, "KantaiHelper/Fleets.csv");
			if (File.Exists(csvPath))
			{
				foreach (var line in File.ReadAllLines(csvPath))
				{
					int partcount = 0;
					var parts = line.Split(',');
					if (parts[0] != "")
					{
						var fleet = new FleetShipViewModel();
						fleet.FleetShipId = new int[6];
						fleet.FleetSlotId = new int[6][];

						fleet.FleetName = parts[partcount];
						partcount++;
						for (int x = 0; x < 6; x++)
						{
							int.TryParse(parts[partcount], out fleet.FleetShipId[x]);
							partcount++;

							fleet.FleetSlotId[x] = new int[4];
							for (int y = 0; y < 4; y++)
							{
								int.TryParse(parts[partcount], out fleet.FleetSlotId[x][y]);
								partcount++;
							}
						}
						_Fleets.Add(fleet);
					}
				}
			}
		}

		public void SaveFleets()
		{
			if (!Directory.Exists(Path.Combine(MainFolder, "KantaiHelper")))
				Directory.CreateDirectory(Path.Combine(MainFolder, "KantaiHelper"));

			var csvPath = Path.Combine(MainFolder, "KantaiHelper/Fleets.csv");
			
			StreamWriter writer = new StreamWriter(csvPath, false, Encoding.UTF8);

			foreach(var fleet in Fleets)
			{
				string data = "";

				data += fleet.FleetName;
				data += ",";
				for(int x = 0; x < 6; x++)
				{
					if (fleet.FleetShipId.Count() > x) data += fleet.FleetShipId[x];
					data += ",";
					for(int y = 0; y < 4; y++)
					{
						if (fleet.FleetShipId.Count() > x && fleet.FleetSlotId[x].Count() > y) data += fleet.FleetSlotId[x][y];
						if (x < 5 || y < 3) data += ",";
					}
				}
				writer.WriteLine(data);
			}
			writer.Close();
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
