using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Math;

namespace WhitePaperBible.Views.Controls
{
	/// <summary>
    /// Provides a blank area for any page content, drawer backdrop, and a drawer at the bottom with a container, the notch, and content.
    ///
    /// Possible to put the backdrop and drawer higher than the Navigation Bar, Tab Bar?
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class BottomDrawerContainer : TemplatedView
    {
        const string ExpandAnimationName = nameof(ExpandAnimationName);
        const uint DefaultAnimationLength = 250;
        const double EnabledOpacity = 1;
        const double DisabledOpacity = .6;

		public static readonly BindableProperty DrawerContentProperty = BindableProperty.Create(nameof(DrawerContent), typeof(View), typeof(Expander), default(View), propertyChanged: (bindable, oldValue, newValue)
			=> ((BottomDrawerContainer)bindable).SetDrawerContent((View)oldValue));

		public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(BottomDrawerContainer), default(View), propertyChanged: (bindable, oldValue, newValue)
            => ((BottomDrawerContainer)bindable).SetContent());

        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(BottomDrawerContainer), default(DataTemplate), propertyChanged: (bindable, oldValue, newValue)
            => ((BottomDrawerContainer)bindable).SetContent(true));

        public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(BottomDrawerContainer), default(bool), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue)
            => ((BottomDrawerContainer)bindable).SetContent(false));

        public static readonly BindableProperty ExpandAnimationLengthProperty = BindableProperty.Create(nameof(ExpandAnimationLength), typeof(uint), typeof(BottomDrawerContainer), DefaultAnimationLength);

        public static readonly BindableProperty CollapseAnimationLengthProperty = BindableProperty.Create(nameof(CollapseAnimationLength), typeof(uint), typeof(BottomDrawerContainer), DefaultAnimationLength);

        public static readonly BindableProperty ExpandAnimationEasingProperty = BindableProperty.Create(nameof(ExpandAnimationEasing), typeof(Easing), typeof(BottomDrawerContainer), default(Easing));

        public static readonly BindableProperty CollapseAnimationEasingProperty = BindableProperty.Create(nameof(CollapseAnimationEasing), typeof(Easing), typeof(BottomDrawerContainer), default(Easing));

		public static readonly BindableProperty ForceUpdateSizeCommandProperty = BindableProperty.Create(nameof(ForceUpdateSizeCommand), typeof(ICommand), typeof(BottomDrawerContainer), default(ICommand), BindingMode.OneWayToSource);

		DataTemplate _previousTemplate;
		double _lastVisibleHeight = -1;
		double _previousWidth = -1;
		double _startHeight;
		double _endHeight;
		bool _shouldIgnoreContentSetting;
		bool _shouldIgnoreAnimation;

		public BottomDrawerContainer()
        {
            ContainerLayout = new Grid();
            ForceUpdateSizeCommand = new Command(ForceUpdateSize);
            InternalChildren.Add(ContainerLayout);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        Grid ContainerLayout { get; set; }

        ContentView ContentHolder { get; set; }

        double ContentHeightRequest
        {
            get
            {
                var heightRequest = Content.HeightRequest;
                if (heightRequest < 0 || !(Content is Layout layout))
                    return heightRequest;
                return heightRequest + layout.Padding.VerticalThickness;
            }
        }

		public View DrawerContent
		{
			get => (View)GetValue(DrawerContentProperty);
			set => SetValue(DrawerContentProperty, value);
		}

		public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public uint ExpandAnimationLength
        {
            get => (uint)GetValue(ExpandAnimationLengthProperty);
            set => SetValue(ExpandAnimationLengthProperty, value);
        }

        public uint CollapseAnimationLength
        {
            get => (uint)GetValue(CollapseAnimationLengthProperty);
            set => SetValue(CollapseAnimationLengthProperty, value);
        }

        public Easing ExpandAnimationEasing
        {
            get => (Easing)GetValue(ExpandAnimationEasingProperty);
            set => SetValue(ExpandAnimationEasingProperty, value);
        }

        public Easing CollapseAnimationEasing
        {
            get => (Easing)GetValue(CollapseAnimationEasingProperty);
            set => SetValue(CollapseAnimationEasingProperty, value);
        }

        public ICommand ForceUpdateSizeCommand
        {
            get => (ICommand)GetValue(ForceUpdateSizeCommandProperty);
            set => SetValue(ForceUpdateSizeCommandProperty, value);
        }



        public void ForceUpdateSize()
        {
            _lastVisibleHeight = -1;
            OnIsOpenChanged();
        }

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			_lastVisibleHeight = -1;
			SetContent(true, true);
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsEnabledProperty.PropertyName)
				OnIsEnabledChanged();
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (Abs(width - _previousWidth) >= double.Epsilon)
			{
				ForceUpdateSize();
			}
			_previousWidth = width;
		}

		void OnIsOpenChanged(bool isBindingContextChanged = false)
		{
			if (ContentHolder == null || (!IsOpen && !ContentHolder.IsVisible))
			{
				return;
			}

			var isAnimationRunning = ContentHolder.AnimationIsRunning(ExpandAnimationName);
			ContentHolder.AbortAnimation(ExpandAnimationName);

			_startHeight = ContentHolder.IsVisible
				? Max(ContentHolder.Height, 0)
				: 0;

			if (IsOpen)
			{
				ContentHolder.IsVisible = true;
			}

			_endHeight = ContentHeightRequest >= 0
				? ContentHeightRequest
				: _lastVisibleHeight;

			if (IsOpen)
			{
				if (_endHeight <= 0)
				{
					ContentHolder.HeightRequest = -1;
					_endHeight = ContentHolder.Measure(Width, double.PositiveInfinity).Request.Height;
					ContentHolder.HeightRequest = 0;
				}
			}
			else
			{
				_lastVisibleHeight = _startHeight = ContentHeightRequest >= 0
						? ContentHeightRequest
						: !isAnimationRunning
							? ContentHolder.Height
							: _lastVisibleHeight;
				_endHeight = 0;
			}

			_shouldIgnoreAnimation = isBindingContextChanged || Height < 0;

			InvokeAnimation();
		}

		void SetDrawerContent(View oldContent)
		{
			if (oldContent != null)
			{
				ContainerLayout.Children.Remove(oldContent);
			}
			if (DrawerContent != null)
			{
				ContainerLayout.Children.Insert(1, DrawerContent); // TODO make sure 1 isn't out of bounds
				DrawerContent.GestureRecognizers.Add(new TapGestureRecognizer
				{
					CommandParameter = this,
					Command = new Command(parameter =>
					{
						var parent = (parameter as View).Parent;
						while (parent != null && !(parent is Page))
						{
							if (parent is Expander ancestorExpander)
							{
								ancestorExpander.ContentHolder.HeightRequest = -1;
							}
							parent = parent.Parent;
						}
						IsExpanded = !IsExpanded;
						Command?.Execute(CommandParameter);
						Tapped?.Invoke(this, EventArgs.Empty);
					})
				});
			}
		}

		void SetContent(bool isForceUpdate, bool isBindingContextChanged = false)
		{
			if (IsExpanded && (Content == null || isForceUpdate))
			{
				_shouldIgnoreContentSetting = true;
				Content = CreateContent() ?? Content;
				_shouldIgnoreContentSetting = false;
			}
			OnIsOpenChanged(isBindingContextChanged);
		}

		void SetContent()
		{
			if (ContentHolder != null)
			{
				ContentHolder.AbortAnimation(ExpandAnimationName);
				ExpanderLayout.Children.Remove(ContentHolder);
				ContentHolder = null;
			}
			if (Content != null)
			{
				ContentHolder = new ContentView
				{
					IsClippedToBounds = true,
					IsVisible = false,
					HeightRequest = 0,
					Content = Content
				};
				ExpanderLayout.Children.Add(ContentHolder);
			}

			if (!_shouldIgnoreContentSetting)
			{
				SetContent(true);
			}
		}

		View CreateContent()
		{
			var template = ContentTemplate;
			while (template is DataTemplateSelector selector)
			{
				template = selector.SelectTemplate(BindingContext, this);
			}
			if (template == _previousTemplate && Content != null)
			{
				return null;
			}
			_previousTemplate = template;
			return (View)template?.CreateContent();
		}

		void InvokeAnimation()
		{
			State = IsExpanded ? ExpanderState.Expanding : ExpanderState.Collapsing;

			if (_shouldIgnoreAnimation)
			{
				State = IsExpanded ? ExpanderState.Expanded : ExpanderState.Collapsed;
				ContentHolder.HeightRequest = _endHeight;
				ContentHolder.IsVisible = IsExpanded;
				return;
			}

			var length = ExpandAnimationLength;
			var easing = ExpandAnimationEasing;
			if (!IsExpanded)
			{
				length = CollapseAnimationLength;
				easing = CollapseAnimationEasing;
			}

			if (_lastVisibleHeight > 0)
			{
				length = Max((uint)(length * (Abs(_endHeight - _startHeight) / _lastVisibleHeight)), 1);
			}

			new Animation(v => ContentHolder.HeightRequest = v, _startHeight, _endHeight)
				.Commit(ContentHolder, ExpandAnimationName, 16, length, easing, (value, isInterrupted) =>
				{
					if (isInterrupted)
					{
						return;
					}
					if (!IsExpanded)
					{
						ContentHolder.IsVisible = false;
						State = ExpanderState.Collapsed;
						return;
					}
					State = ExpanderState.Expanded;
				});
		}

		void OnIsEnabledChanged()
			=> ExpanderLayout.Opacity = IsEnabled ? EnabledOpacity : DisabledOpacity;

	}
}

