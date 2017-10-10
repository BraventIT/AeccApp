using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.Behaviors
{

    /// <summary>
    /// Behaviour to handle events with a Command
    /// </summary>
    public class EventToCommandBehavior : BaseBindableBehavior<View>
    {
        protected Delegate _handler;
        private EventInfo _eventInfo;

        #region Bindable Properties

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public static readonly BindableProperty EventNameProperty =
        BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), string.Empty);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null, propertyChanged: OnCommandChanged);

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var realCommand = newValue as Command;
            if (realCommand != null)
                realCommand.CanExecuteChanged += ((EventToCommandBehavior)bindable).OnCommandCanExecuteChanged;
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);



        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }


        public static readonly BindableProperty EventArgsConverterProperty =
            BindableProperty.Create("EventArgsConverter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);



        public object EventArgsConverterParameter
        {
            get { return GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        public static readonly BindableProperty EventArgsConverterParameterProperty =
            BindableProperty.Create("EventArgsConverterParameter", typeof(object), typeof(EventToCommandBehavior), null);

        #endregion


        protected override void OnAttachedTo(View visualElement)
        {
            base.OnAttachedTo(visualElement);

            var events = AssociateObject.GetType().GetRuntimeEvents();

            if (events.Any())
            {
                _eventInfo = events.FirstOrDefault(e => e.Name == EventName);
                if (_eventInfo == null)
                {
                    throw new ArgumentException($"EventToCommand: Can´t find any event name '{EventName}' on attached type");
                }
                AddEventHandler(_eventInfo, AssociateObject, OnFired);
            }
        }

        protected override void OnDetachingFrom(View visualElement)
        {
            base.OnDetachingFrom(visualElement);

            if (_handler != null)
            {
                var realCommand = Command as Command;
                if (realCommand != null)
                    realCommand.CanExecuteChanged -= OnCommandCanExecuteChanged;

                _eventInfo.RemoveEventHandler(AssociateObject, _handler);
            }
        }

        private void AddEventHandler(EventInfo eventInfo, View associateObject, Action<object, EventArgs> action)
        {
            var eventParameters = eventInfo.EventHandlerType.GetRuntimeMethods()
                 .First(m => m.Name == "Invoke")
                 .GetParameters()
                 .Select(p => Expression.Parameter(p.ParameterType))
                 .ToArray();

            var actionInvoke = action.GetType().GetRuntimeMethods()
                .First(m => m.Name == "Invoke");

            _handler = Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
                eventParameters).Compile();

            eventInfo.AddEventHandler(associateObject, _handler);
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            var realCommand = sender as Command;
            AssociateObject.IsEnabled = !AssociateObject.IsEnabled;
        }

        private void OnFired(object sender, EventArgs eventArgs)
        {
            if (Command == null)
                return;

            var parameter = CommandParameter;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                parameter = eventArgs;

                if (EventArgsConverter != null)
                {
                    parameter = EventArgsConverter.Convert(eventArgs, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentUICulture);
                }
            }

            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }
}
