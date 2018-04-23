using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module.Win {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Person p = ObjectSpace.CreateObject<Person>();
            p.FirstName = "Test";
            PhoneNumber pn = ObjectSpace.CreateObject<PhoneNumber>();
            pn.Party = p;
            pn.Number = "123";
        }
    }
}
