using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Specialized;
using MetroLog;
using Notebook_L.Common;

namespace Notebook_L
{
    public sealed partial class Playground : Page
    {
        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<Playground>();

        private Int64 counter = 0;

        private String GetUniqueString()
        {
            String str = String.Format("String {0:X8}", counter);
            counter += 1;
            return str;
        }

        public ObservableItem<String> OrigString;
        public ObservableItem<String> TargString;
        public ObservableCollection<ObservableItem<String>> OrigCollection;
        public ObservableCollection<ObservableItem<String>> TargCollection;


        public Playground()
        {
            this.InitializeComponent();

            OrigString = new ObservableItem<String>
            {
                Data = GetUniqueString()
            };
            TargString = OrigString.PropagationSelect(e => e + " Mirror");

            //OrigCollection.CollectionChanged += (sender, args) =>
            //{
            //    Log.Fatal("Action = {0}", args.Action.ToString("G"));

            //    if (args.NewItems != null)
            //    {
            //        String newItems = String.Join(", ", args.NewItems
            //            .OfType<ObservableItem<String>>()
            //            .Select(e => e.Data));
            //        Log.Fatal("NewItems = {0}", newItems);
            //    }
            //    else
            //    {
            //        Log.Fatal("NewItems = null");
            //    }
            //    Log.Fatal("NewStartingIndex = {0}", args.NewStartingIndex);

            //    if (args.OldItems != null)
            //    {
            //        String oldItems = String.Join(", ", args.OldItems
            //            .OfType<ObservableItem<String>>()
            //            .Select(e => e.Data));
            //        Log.Fatal("OldItems = {0}", oldItems);
            //    }
            //    else
            //    {
            //        Log.Fatal("OldItems = null");
            //    }
            //    Log.Fatal("OldStartingIndex = {0}", args.OldStartingIndex);


            //    ObservableItem<String> item;
            //    switch (args.Action)
            //    {
            //        case NotifyCollectionChangedAction.Add:
            //            item = args.NewItems.OfType<ObservableItem<String>>().First();
            //            TargCollection.Insert(args.NewStartingIndex, item.PropagationSelect(e => e + " Mirror"));
            //            break;
            //        case NotifyCollectionChangedAction.Move:
            //            TargCollection.Move(args.OldStartingIndex, args.NewStartingIndex);
            //            break;
            //        case NotifyCollectionChangedAction.Remove:
            //            TargCollection.RemoveAt(args.OldStartingIndex);
            //            break;
            //        case NotifyCollectionChangedAction.Replace:
            //            item = args.NewItems.OfType<ObservableItem<String>>().First();
            //            TargCollection[args.NewStartingIndex] = item.PropagationSelect(e => e + " Mirror");
            //            break;
            //        case NotifyCollectionChangedAction.Reset:
            //            TargCollection.Clear();
            //            break;
            //        default:
            //            break;
            //    }
            //};

            OrigCollection = new ObservableCollection<ObservableItem<String>>();

            for (int i = 0; i < 10; i += 1)
            {
                ObservableItem<String> orig = new ObservableItem<String>() { Data = GetUniqueString() };
                OrigCollection.Add(orig);
            }

            TargCollection = OrigCollection.PropagationSelect(e => e + " Mirror");
        }

        private void Button_ModifyStr_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_ModifyStr_Click");
            OrigString.Data = GetUniqueString();
        }

        private void Button_Modify_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Modify_Click");
            OrigCollection[Convert.ToInt32(NumberBox.Value)].Data = GetUniqueString();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Add_Click");
            OrigCollection.Add(new ObservableItem<String> { Data = GetUniqueString() });
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Clear_Click");
            OrigCollection.Clear();
        }

        private void Button_Insert_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Insert_Click");
            OrigCollection.Insert(Convert.ToInt32(NumberBox.Value),
                new ObservableItem<String> { Data = GetUniqueString() });
        }

        private void Button_Remove_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Remove_Click");
            OrigCollection.Remove(OrigCollection[Convert.ToInt32(NumberBox.Value)]);
        }

        private void Button_RemoveAt_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_RemoveAt_Click");
            OrigCollection.RemoveAt(Convert.ToInt32(NumberBox.Value));
        }

        private void Button_Set_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Set_Click");
            OrigCollection[Convert.ToInt32(NumberBox.Value)] = new ObservableItem<String> { Data = GetUniqueString() };
        }

        private void Button_Move_Click(object sender, RoutedEventArgs args)
        {
            Log.Trace("Button_Move_Click");
            OrigCollection.Move(Convert.ToInt32(OrigCollection.Count / 2), Convert.ToInt32(NumberBox.Value));
        }

        private void ListView_Orig_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).ItemsSource = OrigCollection;
        }

        private void ListView_Targ_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).ItemsSource = TargCollection;
        }
    }
}
