using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Grabacr07.KanColleViewer.Composition;
using KantaiHelper.Views;

namespace KantaiHelper
{
	class PluginMain
	{
		[Export(typeof(IPlugin))]
		[Export(typeof(ITool))]
		[ExportMetadata("Guid", "614F4F22-82F3-4BAD-BB5C-5FF7EF1EECD0")]
		[ExportMetadata("Title", "KantaiHelper")]
		[ExportMetadata("Description", "함대 관리 기능을 추가하는 플러그인 입니다.")]
		[ExportMetadata("Version", "0.0")]
		[ExportMetadata("Author", "@CirnoV")]
		public class KantaiHelper : IPlugin, ITool
		{

			public void Initialize() { }

			string ITool.Name => "KantaiHelper";

			object ITool.View => new ToolView();
		}
	}
}
