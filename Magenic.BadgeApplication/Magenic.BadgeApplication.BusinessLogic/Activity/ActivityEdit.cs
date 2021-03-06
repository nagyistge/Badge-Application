﻿using Autofac;
using Csla;
using Csla.Rules;
using Csla.Rules.CommonRules;
using Csla.Serialization.Mobile;
using Magenic.BadgeApplication.BusinessLogic.Framework;
using Magenic.BadgeApplication.BusinessLogic.Rules;
using Magenic.BadgeApplication.Common.DTO;
using Magenic.BadgeApplication.Common.Enums;
using Magenic.BadgeApplication.Common.Interfaces;
using System;
using System.Threading.Tasks;
using NuGet;

namespace Magenic.BadgeApplication.BusinessLogic.Activity
{
    [Serializable]
    public sealed class ActivityEdit : BusinessBase<ActivityEdit>, IActivityEdit, ICreateEmployee
    {
        #region Properties

        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id);
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { SetProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(c => c.Description);
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }

        public static readonly PropertyInfo<bool> RequiresApprovalProperty = RegisterProperty<bool>(c => c.RequiresApproval);
        public bool RequiresApproval
        {
            get { return GetProperty(RequiresApprovalProperty); }
            set { SetProperty(RequiresApprovalProperty, value); }
        }

        public static readonly PropertyInfo<int> CreateEmployeeIdProperty = RegisterProperty<int>(c => c.CreateEmployeeId);
        public int CreateEmployeeId
        {
            get { return GetProperty(CreateEmployeeIdProperty); }
            private set { SetProperty(CreateEmployeeIdProperty, value); }
        }

        public static readonly PropertyInfo<ActivityEntryType> EntryTypeProperty = RegisterProperty<ActivityEntryType>(c => c.EntryType);
        public ActivityEntryType EntryType
        {
            get { return GetProperty(EntryTypeProperty); }
            set { SetProperty(EntryTypeProperty, value); }
        }

        #endregion Properties

        #region Factory Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "activityEdit")]
        public static async Task<IActivityEdit> GetActivityEditByIdAsync(int activityEditId)
        {
            return await IoC.Container.Resolve<IObjectFactory<IActivityEdit>>().FetchAsync(activityEditId);
        }

        public static IActivityEdit CreateActivity()
        {
            return IoC.Container.Resolve<IObjectFactory<IActivityEdit>>().Create();
        }

        #endregion Factory Methods

        #region Rules

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            this.BusinessRules.AddRule(new MaxLength(NameProperty, 100));
            this.BusinessRules.AddRule(new Required(NameProperty));
            this.BusinessRules.AddRule(new IsInRole(AuthorizationActions.WriteProperty, RequiresApprovalProperty, PermissionType.Administrator.ToString()));
            this.BusinessRules.AddRule(new MinValue<int>(CreateEmployeeIdProperty, 1));
            this.BusinessRules.AddRule(new IsInRole(AuthorizationActions.WriteProperty, EntryTypeProperty, PermissionType.Administrator.ToString()));

            // Only run this rule if the associated properties are otherwise valid.
            this.BusinessRules.AddRule(new NoDuplicates(NameProperty, IdProperty, NameExists) { Priority = 1 });
        }

        public static void AddObjectAuthorizationRules()
        {
            BusinessRules.AddRule(typeof(IActivityEdit), new CanChange(AuthorizationActions.EditObject, PermissionType.Administrator.ToString()));
            BusinessRules.AddRule(typeof(ActivityEdit), new CanChange(AuthorizationActions.EditObject, PermissionType.Administrator.ToString()));
            BusinessRules.AddRule(typeof(IActivityEdit), new CanChange(AuthorizationActions.DeleteObject, PermissionType.Administrator.ToString()));
            BusinessRules.AddRule(typeof(ActivityEdit), new CanChange(AuthorizationActions.DeleteObject, PermissionType.Administrator.ToString()));
        }

        #endregion Rules

        #region Data Access

        internal bool NameExists(int id, string name)
        {
            return ActivityNameExists.NameAlreadyExists(id, name);
        }

        [RunLocal]
        protected override void DataPortal_Create()
        {
            base.DataPortal_Create();
            this.LoadProperty(CreateEmployeeIdProperty, ((ICustomPrincipal)ApplicationContext.User).CustomIdentity().EmployeeId);
            this.LoadProperty(EntryTypeProperty, ActivityEntryType.Any);
        }

        private async Task DataPortal_Fetch(int activityEditId)
        {
            var dal = IoC.Container.Resolve<IActivityEditDAL>();

            var result = await dal.GetActivityByIdAsync(activityEditId);
            this.LoadData(result);
        }

        [Transactional(TransactionalTypes.TransactionScope, TransactionIsolationLevel.ReadCommitted)]
        protected override void DataPortal_Update()
        {
            if (IsDeleted)
            {
                if (!IsNew)
                {
                    this.DataPortal_DeleteSelf();
                }
                return;
            }

            if (IsNew)
            {
                this.DataPortal_Insert();
            }
            else if (IsDirty)
            {
                var dal = IoC.Container.Resolve<IActivityEditDAL>();
                this.LoadData(dal.Update(this.UnloadData()));
                FieldManager.UpdateChildren();
            }
            this.MarkClean();
            this.MarkOld();
        }

        private ActivityEditDTO UnloadData()
        {
            var returnValue = new ActivityEditDTO();
            using (this.BypassPropertyChecks)
            {
                returnValue.Id = this.Id;
                returnValue.Description = this.Description;
                returnValue.Name = this.Name;
                returnValue.RequiresApproval = this.RequiresApproval;
                returnValue.CreateEmployeeId = this.CreateEmployeeId;
                returnValue.EntryType = this.EntryType;
            }
            return returnValue;
        }

        internal void LoadData(ActivityEditDTO item)
        {
            using (this.BypassPropertyChecks)
            {
                this.Id = item.Id;
                this.Name = item.Name;
                this.Description = item.Description;
                this.RequiresApproval = item.RequiresApproval;
                this.CreateEmployeeId = item.CreateEmployeeId;
                this.EntryType = item.EntryType;
            }
        }

        [Transactional(TransactionalTypes.TransactionScope, TransactionIsolationLevel.ReadCommitted)]
        protected override void DataPortal_DeleteSelf()
        {
            base.DataPortal_DeleteSelf();
            var dal = IoC.Container.Resolve<IActivityEditDAL>();

            if (!IsNew)
            {
                this.DeleteChildren();
                dal.Delete(this.Id);
            }

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void DeleteChildren()
        {
        }

        [Transactional(TransactionalTypes.TransactionScope, TransactionIsolationLevel.ReadCommitted)]
        protected override void DataPortal_Insert()
        {
            base.DataPortal_Insert();
            var dal = IoC.Container.Resolve<IActivityEditDAL>();

            this.LoadData(dal.Insert(this.UnloadData()));
            FieldManager.UpdateChildren();
        }

        #endregion Data Access
    }
}