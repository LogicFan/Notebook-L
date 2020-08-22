using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notebook_L.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Common
{
    using OcOiInt32 = ObservableCollection<ObservableItem<Int32>>;
    using OcOiString = ObservableCollection<ObservableItem<String>>;

    [TestClass]
    public class ObservableItemTest
    {
        [TestMethod]
        public void ObservableItemGeneral()
        {
            Int32 data = 32409;

            ObservableItem<Int32> item;

            item = new ObservableItem<Int32>(data);
            Assert.AreEqual(data, item.Data);

            item = new ObservableItem<Int32>()
            {
                Data = data
            };
            Assert.AreEqual(data, item.Data);

            ObservableItem<String> item1 = item.PropagationSelect(e => e.ToString("X8"));
            Assert.AreEqual(data.ToString("X8"), item1.Data);

            item.Data = data = 7321749;
            Assert.AreEqual(data, item.Data);
            Assert.AreEqual(data.ToString("X8"), item1.Data);
        }

        [TestMethod]
        public void ObservableCollectionGeneral()
        {
            IEnumerable<Int32> data = new Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            OcOiInt32 collection = new OcOiInt32(data.Select(e => new ObservableItem<Int32>(e)));
            Assert.IsTrue(Enumerable.SequenceEqual(collection.Select(e => e.Data), data));

            OcOiString collection1 = collection.PropagationSelect(e => e.ToString("X8"));
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection[5].Data = 18297;
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Add(new ObservableItem<Int32>(233));
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Insert(7, new ObservableItem<Int32>(233));
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Remove(collection[3]);
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.RemoveAt(4);
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection[5] = new ObservableItem<Int32>(233);
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Move(3, 7);
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Move(7, 3);
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));

            collection.Clear();
            Assert.IsTrue(Enumerable.SequenceEqual(
                collection1.Select(e => e.Data),
                collection.Select(e => e.Data.ToString("X8"))
            ));
        }
    }
}
