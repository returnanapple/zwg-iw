﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Class;
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ParticipatedRecordOfExchangesPage_TableRow : UserControl, ITableToolRow
    {
        public ExchangeParticipateRecordResult Participate { get; set; }
        int _row = 0;

        public ParticipatedRecordOfExchangesPage_TableRow(ExchangeParticipateRecordResult participate, int row)
        {
            InitializeComponent();
            this.Participate = participate;
            this._row = row;

            button_owner.Text = participate.OwnerName;
            button_exchange.Text = participate.ExchangeName;
            text_sum.Text = participate.Sum.ToString();
            text_reward.Text = participate.RewardOfMoney.ToString();
            text_time.Text = participate.ParticipatedTime.ToShortDateString();
        }

        #region 事件

        public event NDelegate SelecteForOwnerEventHandler;
        public event NDelegate SeleteForActivityEventHandler;
        public event TableToolBodyHoverDelegate HoverEventHandler;
        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        private void SelecteForOwner(object sender, MouseButtonEventArgs e)
        {
            if (SelecteForOwnerEventHandler != null)
            {
                SelecteForOwnerEventHandler(this, new EventArgs());
            }
        }

        private void SeleteForActivity(object sender, MouseButtonEventArgs e)
        {
            if (SeleteForActivityEventHandler != null)
            {
                SeleteForActivityEventHandler(this, new EventArgs());
            }
        }

        private void Hover(object sender, MouseEventArgs e)
        {
            if (HoverEventHandler != null)
            {
                HoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            if (UnhoverEventHandler != null)
            {
                UnhoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }
    }
}