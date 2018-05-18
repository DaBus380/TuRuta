using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;

namespace TuRuta.iOS
{
    class ItemSelectedEventArgs
    {
        public string SelectedItem { get; set; }
        public ItemSelectedEventArgs(string selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }

    internal class TableSource : UITableViewSource
    {
        private string[] data;
        private string CellIdentifier = "TableCell";
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        protected TableSource() { }
        public TableSource(IEnumerable<string> source)
            => data = source.ToArray();

        protected TableSource(NSObjectFlag t) : base(t) { }
        protected internal TableSource(IntPtr handle) : base(handle) { }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = data[indexPath.Row];

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }

            cell.TextLabel.Text = item;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            => ItemSelected?.Invoke(this, new ItemSelectedEventArgs(data[indexPath.Row]));

        public override nint RowsInSection(UITableView tableview, nint section)
            => data.Length;
    }
}
