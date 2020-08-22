using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Notebook_L.Common
{
    public class ObservableItem : INotifyPropertyChanged
    {
        private object m_data;

        public event PropertyChangedEventHandler PropertyChanged;

        public object Data
        {
            get => m_data;
            set
            {
                m_data = value;
                NotifyPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }

        public ObservableItem() { }

        public ObservableItem(object data)
        {
            m_data = data;
        }

        public void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }

    public class ObservableItem<T> : ObservableItem
    {
        public new T Data
        {
            get => (T) base.Data;
            set { base.Data = value; }
        }

        public ObservableItem() { }
        public ObservableItem(T data) : base(data) { }
    }

    public static class ObservableItemExtension
    {
        public static ObservableItem<TResult> PropagationSelect<TSource, TResult>(this ObservableItem<TSource> source, Func<TSource, TResult> selector)
        {
            ObservableItem<TResult> target = new ObservableItem<TResult>(selector(source.Data));

            source.PropertyChanged += (sender, args) =>
            {
                target.Data = selector(source.Data);
            };

            return target;
        }

        public static ObservableCollection<ObservableItem<TResult>> PropagationSelect<TSource, TResult>(
            this ObservableCollection<ObservableItem<TSource>> source,
            Func<TSource, TResult> selector)
        {
            ObservableCollection<ObservableItem<TResult>> target = new ObservableCollection<ObservableItem<TResult>>();

            foreach (ObservableItem<TSource> e in source)
            {
                target.Add(e.PropagationSelect(selector));
            }

            source.CollectionChanged += (sender, args) =>
            {
                ObservableItem<TSource> item;
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        item = args.NewItems.OfType<ObservableItem<TSource>>().First();
                        target.Insert(args.NewStartingIndex, item.PropagationSelect(selector));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        target.Move(args.OldStartingIndex, args.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        target.RemoveAt(args.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        item = args.NewItems.OfType<ObservableItem<TSource>>().First();
                        target[args.NewStartingIndex] = item.PropagationSelect(selector);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        target.Clear();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            };

            return target;
        }
    }
}
