﻿using Flantter.MilkyWay.Common;
using Flantter.MilkyWay.Models.Twitter.Wrapper;
using Flantter.MilkyWay.Setting;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flantter.MilkyWay.Models.SettingsFlyouts
{
    public class ListMembersSettingsFlyoutModel : BindableBase
    {
        public ListMembersSettingsFlyoutModel()
        {
            this.ListMembers = new ObservableCollection<Twitter.Objects.User>();
        }

        #region Tokens変更通知プロパティ
        private Tokens _Tokens;
        public Tokens Tokens
        {
            get { return this._Tokens; }
            set { this.SetProperty(ref this._Tokens, value); }
        }
        #endregion

        #region Id変更通知プロパティ
        private long _Id;
        public long Id
        {
            get { return this._Id; }
            set { this.SetProperty(ref this._Id, value); }
        }
        #endregion

        #region Updating変更通知プロパティ
        private bool _Updating;
        public bool Updating
        {
            get { return this._Updating; }
            set { this.SetProperty(ref this._Updating, value); }
        }
        #endregion

        public ObservableCollection<Twitter.Objects.User> ListMembers { get; set; }

        private long listMembersCursor = 0;
        public async Task UpdateListMembers(bool useCursor = false)
        {
            if (this.Updating)
                return;

            if (this.Id == 0 || this.Tokens == null)
                return;

            if (useCursor && listMembersCursor == 0)
                return;

            this.Updating = true;

            if (!useCursor || listMembersCursor == 0)
                this.ListMembers.Clear();
            
            try
            {
                var param = new Dictionary<string, object>()
                {
                    {"list_id", this._Id},
                    {"count", 20},
                };
                if (useCursor && listMembersCursor != 0)
                    param.Add("cursor", listMembersCursor);

                var listMembers = await Tokens.Lists.Members.ListAsync(param);
                if (!useCursor || listMembersCursor == 0)
                    this.ListMembers.Clear();

                foreach (var user in listMembers)
                {
                    this.ListMembers.Add(user);
                }

                listMembersCursor = listMembers.NextCursor;
                this.Updating = false;
            }
            catch
            {
                if (!useCursor || listMembersCursor == 0)
                    this.ListMembers.Clear();

                this.Updating = false;
                return;
            }
        }
    }
}
