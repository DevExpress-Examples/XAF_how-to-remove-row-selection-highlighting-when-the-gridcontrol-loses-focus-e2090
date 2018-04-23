Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl

Namespace WinSolution.Module.Win
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim p As Person = ObjectSpace.CreateObject(Of Person)()
			p.FirstName = "Test"
			Dim pn As PhoneNumber = ObjectSpace.CreateObject(Of PhoneNumber)()
			pn.Party = p
			pn.Number = "123"
		End Sub
	End Class
End Namespace
