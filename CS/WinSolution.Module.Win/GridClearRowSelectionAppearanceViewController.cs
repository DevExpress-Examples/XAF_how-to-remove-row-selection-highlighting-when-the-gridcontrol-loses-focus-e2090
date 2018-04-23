using System;
using DevExpress.XtraGrid;
using DevExpress.ExpressApp;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.Win.Editors;

namespace WinSolution.Module.Win {
    public class GridClearRowSelectionAppearanceViewController : ViewController<ListView> {
        private GridControl grid;
        private GridView gridView;
        private bool shouldCustomizeGridFirstTime;
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            GridListEditor gridListEditor = View.Editor as GridListEditor;
            if(gridListEditor != null) {
                shouldCustomizeGridFirstTime = true;
                grid = gridListEditor.Grid;
                gridView = gridListEditor.GridView;
                if(!View.IsRoot) {
                    grid.HandleCreated += Grid_HandleCreated;
                }
                grid.Enter += Grid_Enter;
                grid.Leave += Grid_Leave;
                grid.GotFocus += Grid_GotFocus;
                gridView.CustomDrawRowIndicator += GridView_CustomDrawRowIndicator;
            }
        }
        protected override void OnDeactivated() {
            if(grid != null) {
                if(!View.IsRoot) {
                    grid.HandleCreated -= Grid_HandleCreated;
                }
                grid.Enter -= Grid_Enter;
                grid.Leave -= Grid_Leave;
                grid.GotFocus -= Grid_GotFocus;
                grid = null;
            }
            if(gridView != null) {
                gridView.CustomDrawRowIndicator -= GridView_CustomDrawRowIndicator;
                gridView = null;
            }
            base.OnDeactivated();
        }
        private void Grid_HandleCreated(object sender, EventArgs e) {
            CustomizeGrid();
            shouldCustomizeGridFirstTime = false;
        }
        private void Grid_Enter(object sender, EventArgs e) {
            CustomizeGrid();
        }
        private void Grid_Leave(object sender, EventArgs e) {
            if(gridView.ActiveEditor != null) {
                grid.Validated += Grid_Validated;
            }
            else {
                CustomizeGrid();
            }
        }
        private void Grid_Validated(object sender, EventArgs e) {
            grid.Validated -= Grid_Validated;
            CustomizeGrid();
        }
        private void Grid_GotFocus(object sender, EventArgs e) {
            if(shouldCustomizeGridFirstTime) {
                shouldCustomizeGridFirstTime = false;
                if(grid.Parent != null) {
                    grid.Parent.Focus();
                }
                return;
            }
            CustomizeGrid();
        }
        private void GridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e) {
            if(!grid.Focused && gridView.ActiveEditor == null) {
                e.Info.ImageIndex = -1;
            }
        }
        private void CustomizeGrid() {
            CustomizeGrid(grid.Focused);
        }
        private void CustomizeGrid(bool isGridFocused) {
            gridView.BeginUpdate();
            gridView.FocusRectStyle = isGridFocused ? DrawFocusRectStyle.CellFocus : DrawFocusRectStyle.None;
            gridView.OptionsSelection.EnableAppearanceFocusedCell = isGridFocused;
            gridView.OptionsSelection.EnableAppearanceFocusedRow = isGridFocused;
            gridView.OptionsSelection.EnableAppearanceHideSelection = isGridFocused;
            gridView.EndUpdate();
        }
    }
}