using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livet;

namespace KantaiHelper.ViewModels
{
	/// <summary>
	/// KanColleViewerから移植
	/// </summary>
	public class ItemViewModel : ViewModel
	{
		#region IsSelected  변경 통지 프로퍼티

		private bool _IsSelected;

		public virtual bool IsSelected
		{
			get { return this._IsSelected; }
			set
			{
				if (this._IsSelected != value)
				{
					this._IsSelected = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
	}
}