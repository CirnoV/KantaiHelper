using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KantaiHelper.Views.Behaviors
{
	public class DragStartBehavior : Behavior<FrameworkElement>
	{
		private Point _origin;
		private bool _isButtonDown;

		public DragDropEffects AllowedEffects
		{
			get { return (DragDropEffects)GetValue(AllowedEffectsProperty); }
			set { SetValue(AllowedEffectsProperty, value); }
		}

		public static readonly DependencyProperty AllowedEffectsProperty =
			DependencyProperty.Register("AllowedEffects", typeof(DragDropEffects),
					typeof(DragStartBehavior), new UIPropertyMetadata(DragDropEffects.All));

		public object DragDropData
		{
			get { return GetValue(DragDropDataProperty); }
			set { SetValue(DragDropDataProperty, value); }
		}

		public static readonly DependencyProperty DragDropDataProperty =
			DependencyProperty.Register("DragDropData", typeof(object),
					typeof(DragStartBehavior), new PropertyMetadata(null));

		protected override void OnAttached()
		{
			this.AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
			this.AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
			this.AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
		}

		protected override void OnDetaching()
		{
			this.AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
			this.AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
			this.AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
		}

		void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_origin = e.GetPosition(this.AssociatedObject);
			_isButtonDown = true;
		}

		void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton != MouseButtonState.Pressed || !_isButtonDown)
			{
				return;
			}
			var point = e.GetPosition(this.AssociatedObject);
			if (CheckDistance(point, _origin))
			{
				DragDrop.DoDragDrop(this.AssociatedObject, this.DragDropData, this.AllowedEffects);
				_isButtonDown = false;
				e.Handled = true;
			}
		}

		void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			_isButtonDown = false;
		}

		private bool CheckDistance(Point x, Point y)
		{
			return Math.Abs(x.X - y.X) >= SystemParameters.MinimumHorizontalDragDistance ||
				Math.Abs(x.Y - y.Y) >= SystemParameters.MinimumVerticalDragDistance;
		}
	}
}