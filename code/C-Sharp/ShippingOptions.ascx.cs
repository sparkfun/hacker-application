using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using WillCallWeb.StoreObjects;

namespace WillCallWeb.Components.Cart
{
    public partial class ShippingOptions : BaseControl
    {
        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.ToString().ToLower() != "asp.store_shipping_aspx")
            {
                this.Controls.Clear();
            }
            else
            {
                if (!IsPostBack)
                {
                    BindChildControls();
                }
            }
        }

        #endregion

        public void BindChildControls()
        {
            SaleItem_Shipping generalMerch = Ctx.Cart.Shipments_Merch.Find(delegate(SaleItem_Shipping match)
            { return (match.ShipContext == Wcss._Enums.InvoiceItemContext.shippingmerch && match.IsGeneral); });

            if (generalMerch != null)
            {
                ShipRateListing_Normal.ItemGuid = generalMerch.GUID.ToString();
            }
            else
                ShipRateListing_Normal.Visible = false;

            rptBackorders.DataBind();

            rptSeparate.DataBind();
        }

        #region Backordered && Separate Items

        protected void rptShipping_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            //rpt.Visible = false;

            if (rpt.ID == "rptBackorders" && Ctx.Cart.IsShipMultiple_Merch && Ctx.Cart.HasBackorderedMerch)
            {
                List<SaleItem_Shipping> backorders = new List<SaleItem_Shipping>();
                backorders.AddRange(Ctx.Cart.Shipments_Merch.FindAll(delegate(SaleItem_Shipping match) { return (match.IsBackOrder); }));

                //rpt.Visible = true;
                rpt.DataSource = backorders;
            }
            else if (rpt.ID == "rptSeparate" && (Ctx.Cart.HasFlatShipMerch || Ctx.Cart.HasShipSeparateMerch))
            {
                List<SaleItem_Shipping> separate = new List<SaleItem_Shipping>();
                separate.AddRange(Ctx.Cart.Shipments_Merch.FindAll(delegate(SaleItem_Shipping match) { return (match.IsFlatShip || match.IsShipSeparate); }));

                //rpt.Visible = true;
                rpt.DataSource = separate;
            }
        }

        #endregion

        public void ShipRateChange (object sender, EventArgs e)
        {
            OnShipRateChanged(EventArgs.Empty);
        }

        private static readonly object ShipRateChangedEventKey = new object();

        public delegate void ShipRateChangedEventHandler(object sender, EventArgs e);

        public event ShipRateChangedEventHandler ShipRateChanged
        {
            add { Events.AddHandler(ShipRateChangedEventKey, value); }
            remove { Events.RemoveHandler(ShipRateChangedEventKey, value); }
        }
        public virtual void OnShipRateChanged(EventArgs e)
        {
            ShipRateChangedEventHandler handler = (ShipRateChangedEventHandler)Events[ShipRateChangedEventKey];

            if (handler != null)
                handler(this, e);
        }
}
}