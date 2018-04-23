Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid
Imports DevExpress.ExpressApp
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.ExpressApp.Win.Editors

Namespace WinSolution.Module.Win
	Public Class GridClearRowSelectionAppearanceViewController
		Inherits ViewController(Of ListView)
		Private grid As GridControl
		Private gridView As GridView
		Private shouldCustomizeGridFirstTime As Boolean
		Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			Dim gridListEditor As GridListEditor = TryCast(View.Editor, GridListEditor)
			If gridListEditor IsNot Nothing Then
				shouldCustomizeGridFirstTime = True
				grid = gridListEditor.Grid
				gridView = gridListEditor.GridView
				If (Not View.IsRoot) Then
					AddHandler grid.HandleCreated, AddressOf Grid_HandleCreated
				End If
				AddHandler grid.Enter, AddressOf Grid_Enter
				AddHandler grid.Leave, AddressOf Grid_Leave
				AddHandler grid.GotFocus, AddressOf Grid_GotFocus
				AddHandler gridView.CustomDrawRowIndicator, AddressOf GridView_CustomDrawRowIndicator
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			If grid IsNot Nothing Then
				If (Not View.IsRoot) Then
					RemoveHandler grid.HandleCreated, AddressOf Grid_HandleCreated
				End If
				RemoveHandler grid.Enter, AddressOf Grid_Enter
				RemoveHandler grid.Leave, AddressOf Grid_Leave
				RemoveHandler grid.GotFocus, AddressOf Grid_GotFocus
				grid = Nothing
			End If
			If gridView IsNot Nothing Then
				RemoveHandler gridView.CustomDrawRowIndicator, AddressOf GridView_CustomDrawRowIndicator
				gridView = Nothing
			End If
			MyBase.OnDeactivated()
		End Sub
		Private Sub Grid_HandleCreated(ByVal sender As Object, ByVal e As EventArgs)
			CustomizeGrid()
			shouldCustomizeGridFirstTime = False
		End Sub
		Private Sub Grid_Enter(ByVal sender As Object, ByVal e As EventArgs)
			CustomizeGrid()
		End Sub
		Private Sub Grid_Leave(ByVal sender As Object, ByVal e As EventArgs)
			If gridView.ActiveEditor IsNot Nothing Then
				AddHandler grid.Validated, AddressOf Grid_Validated
			Else
				CustomizeGrid()
			End If
		End Sub
		Private Sub Grid_Validated(ByVal sender As Object, ByVal e As EventArgs)
			RemoveHandler grid.Validated, AddressOf Grid_Validated
			CustomizeGrid()
		End Sub
		Private Sub Grid_GotFocus(ByVal sender As Object, ByVal e As EventArgs)
			If shouldCustomizeGridFirstTime Then
				shouldCustomizeGridFirstTime = False
				If grid.Parent IsNot Nothing Then
					grid.Parent.Focus()
				End If
				Return
			End If
			CustomizeGrid()
		End Sub
		Private Sub GridView_CustomDrawRowIndicator(ByVal sender As Object, ByVal e As RowIndicatorCustomDrawEventArgs)
			If (Not grid.Focused) AndAlso gridView.ActiveEditor Is Nothing Then
				e.Info.ImageIndex = -1
			End If
		End Sub
		Private Sub CustomizeGrid()
			CustomizeGrid(grid.Focused)
		End Sub
		Private Sub CustomizeGrid(ByVal isGridFocused As Boolean)
			gridView.BeginUpdate()
			gridView.FocusRectStyle = If(isGridFocused, DrawFocusRectStyle.CellFocus, DrawFocusRectStyle.None)
			gridView.OptionsSelection.EnableAppearanceFocusedCell = isGridFocused
			gridView.OptionsSelection.EnableAppearanceFocusedRow = isGridFocused
			gridView.OptionsSelection.EnableAppearanceHideSelection = isGridFocused
			gridView.EndUpdate()
		End Sub
	End Class
End Namespace