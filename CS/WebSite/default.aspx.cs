using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;

public partial class _default : System.Web.UI.Page {
    /* Data */
    public class MyItem {
        private string id;
        private int itemValue;

        public string Id {
            get { return id;}
            set { id = value; }
        }
        public int ItemValue {
            get { return itemValue; }
            set { itemValue = value; }
        }
        public MyItem()
            : this(-1) {
        }
        public MyItem(int itemValue) {
            this.id = Guid.NewGuid().ToString();
            this.itemValue = itemValue;
        }
    }
    public class MyItems : List<MyItem>{

        public MyItems() : base() {
            // Adding sample data
            Add(new MyItem(1));
            Add(new MyItem(2));
            Add(new MyItem(3));
        }
        public MyItem NewItem() {
            MyItem item = new MyItem();
            Add(item);
            return item;
        }
        public void DeleteById(string id) {
            for(int i = 0; i < Count; i++) {
                MyItem item = this[i];
                if(item.Id == id) {
                    RemoveAt(i);
                    break;
                }
            }
        }
    }

    public MyItems Data {
        get {
            if(Session["MyItems"] == null)
                Session["MyItems"] = new MyItems();
            return (MyItems)(Session["MyItems"]);
        }
    }
    /* End Data */

    protected void Page_Load(object sender, EventArgs e) {
        dataView.DataSource = Data;
        dataView.DataBind();
    }

    protected void dataView_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
        UpdateMyItemsFromClient();

        if(e.Parameter.StartsWith("delete")) {
            string[] parts = e.Parameter.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            string id = parts[1];
            Data.DeleteById(id);
        }
        else
            if(e.Parameter == "add")
                Data.NewItem();

        dataView.DataBind();
    }
    protected void btSave_Click(object sender, EventArgs e) {
        UpdateMyItemsFromClient();
        // Here you can save data to a database
    }

    protected string GetDeleteLink(object id) {
        string s = (id != null) ? id.ToString() : "";
        return string.Format("javascript:DeleteItem('{0}');", s);
    }
    protected void UpdateMyItemsFromClient() {
        for(int i = 0; i < dataView.VisibleItems.Count; i++) {
            DataViewItem item = dataView.Items[i];
            ASPxComboBox comboBox = (ASPxComboBox)dataView.FindItemControl("comboItemValue", item);

            Data[i].ItemValue = (int)comboBox.Value;
        }
    }
}
