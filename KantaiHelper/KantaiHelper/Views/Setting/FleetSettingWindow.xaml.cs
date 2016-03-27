using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MetroRadiance.UI.Controls;

namespace KantaiHelper.Views.Setting
{
	/* 
	* ViewModel에서의 변경 통지 등 각종 이벤트를 받을 경우 PropertyChangedWeakEventListener 나
	* CollectionChangedWeakEventListener를 사용하면 편리합니다. 자체 이벤트의 경우 LivetWeakEventListener를 사용할 수 있습니다.
	* 닫을 때 등에 LivetCompositeDisposable에 저장된 각종 이벤트 리스너를 Dispose하는 것으로 이벤트 핸들러의 개방이 용이합니다.
	*
	* WeakEventListener이므로 명시적으로 개방하지 않고도 메모리 누수를 일으키지 않지만, 가능한 한 명시적으로 개방 하도록 합시다.
	*/

	/// <summary>
	/// FleetSettingWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class FleetSettingWindow : MetroWindow
	{

		public FleetSettingWindow()
		{
			this.InitializeComponent();
			WeakEventManager<Window, EventArgs>.AddHandler(
				Application.Current.MainWindow,
				"Closed",
				(_, __) => this.Close());
		}
	}
}
