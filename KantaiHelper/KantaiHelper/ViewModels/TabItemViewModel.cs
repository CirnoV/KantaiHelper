using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livet;
using MetroRadiance.UI.Controls;
using Livet.EventListeners;

namespace KantaiHelper.ViewModels
{
	/// <summary>
	/// KanColleViewerから移植
	/// </summary>
	public abstract class TabItemViewModel : ItemViewModel, ITabItem
	{
		#region Name  변경 통지 프로퍼티

		/// <summary>
		/// タブ名を取得します。
		/// </summary>
		public abstract string Name { get; protected set; }

		#endregion

		#region Badge  변경 통지 프로퍼티

		private int? _Badge;

		/// <summary>
		/// バッジを取得します。
		/// </summary>
		public virtual int? Badge
		{
			get { return this._Badge; }
			protected set
			{
				if (this._Badge != value)
				{
					this._Badge = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Status  변경 통지 프로퍼티

		private ViewModel _Status;

		/// <summary>
		/// ステータス バーに表示するステータスを取得します。
		/// </summary>
		public virtual ViewModel Status
		{
			get { return this._Status; }
			protected set
			{
				if (this._Status != value)
				{
					this._Status = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		//protected TabItemViewModel()
		//{
		//	if (Helper.IsInDesignMode) return;

		//	this.CompositeDisposable.Add(new PropertyChangedEventListener(ResourceService.Current)
		//	{
		//		(sender, args) => this.RaisePropertyChanged(nameof(this.Name)),
		//	});
		//}
	}
}
