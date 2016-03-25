using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MetroTrilithon.Mvvm;
using Grabacr07.KanColleWrapper;
using KantaiHelper.ViewModels.Fleet;
using Livet.Messaging.Windows;

namespace KantaiHelper.ViewModels.Setting
{
	class DeleteFleetWindowViewModel : WindowViewModel
	{
		private ToolViewModel _ViewModel;
		
		private FleetShipViewModel _TargetFleet;

		public DeleteFleetWindowViewModel(ToolViewModel viewmodel, FleetShipViewModel targetfleet)
		{
			this._ViewModel = viewmodel;
			this._TargetFleet = targetfleet;
		}
		
		public void DeleteFleet(string delete)
		{
			if(bool.Parse(delete))
				_ViewModel.DeleteFleet(_TargetFleet);

			Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
		}
	}
}
