using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Grabacr07.KanColleViewer.Composition;
using KantaiHelper.Views;
using KantaiHelper.ViewModels;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;

namespace KantaiHelper
{
	[Export(typeof(IPlugin))]
	[Export(typeof(ITool))]
	[ExportMetadata("Guid", "614F4F22-82F3-4BAD-BB5C-5FF7EF1EECD0")]
	[ExportMetadata("Title", "KantaiHelper")]
	[ExportMetadata("Description", "함대 관리 기능을 추가하는 플러그인 입니다.")]
	[ExportMetadata("Version", "0.2")]
	[ExportMetadata("Author", "@CirnoV")]
	public class HelperPlugin : IPlugin, ITool
	{
		private readonly ToolViewModel ViewModel;
		internal static kcsapi_start2 RawStart2 { get; private set; }

		public HelperPlugin()
		{
			this.ViewModel = new ToolViewModel();
		}

		public void Initialize()
		{
			KanColleClient.Current.Proxy.api_start2.TryParse<kcsapi_start2>().Subscribe(x =>
			{
				RawStart2 = x.Data;
				//Models.Repositories.Master.Current.Update(x.Data);
			});
			KanColleClient.Current.Proxy.api_port.TryParse<kcsapi_port>().Subscribe(x =>
			{
				ViewModel.Initialize();
			});
		}

		public string Name => "KantaiHelper";

		// 탭을 볼 때마다 new가 되어 버리지만, 지금은 이렇게 하지 않으면 멀티 윈도우에서 제대로 표시되지 않습니다.
		public object View => new ToolView {DataContext = this.ViewModel};
	}
}
