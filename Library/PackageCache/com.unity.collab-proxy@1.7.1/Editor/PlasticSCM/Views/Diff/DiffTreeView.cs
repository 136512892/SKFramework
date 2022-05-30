﻿using System.Collections.Generic;

using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.Client.Commands;
using Codice.Utils;
using PlasticGui;
using PlasticGui.WorkspaceWindow.Diff;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.UI.Tree;

namespace Unity.PlasticSCM.Editor.Views.Diff
{
    internal class DiffTreeView : TreeView
    {
        internal DiffTreeView(DiffTreeViewMenu menu)
            : base(new TreeViewState())
        {
            mMenu = menu;

            customFoldoutYOffset = UnityConstants.TREEVIEW_FOLDOUT_Y_OFFSET;
            rowHeight = UnityConstants.TREEVIEW_ROW_HEIGHT;
            showAlternatingRowBackgrounds = true;

            mCooldownFilterAction = new CooldownWindowDelayer(
                DelayedSearchChanged, UnityConstants.SEARCH_DELAYED_INPUT_ACTION_INTERVAL);
        }

        public override IList<TreeViewItem> GetRows()
        {
            return mRows;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);

            Event e = Event.current;

            if (e.type != EventType.KeyDown)
                return;

            bool isProcessed = mMenu.ProcessKeyActionIfNeeded(e);

            if (isProcessed)
                e.Use();
        }

        protected override bool CanChangeExpandedState(TreeViewItem item)
        {
            return item is ChangeCategoryTreeViewItem
                || item is MergeCategoryTreeViewItem;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem(0, -1, string.Empty);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem rootItem)
        {
            try
            {
                RegenerateRows(
                    mDiffTree,
                    mTreeViewItemIds,
                    this,
                    rootItem,
                    mRows,
                    mExpandCategories);
            }
            finally
            {
                mExpandCategories = false;
            }

            return mRows;
        }

        protected override void CommandEventHandling()
        {
            // NOTE - empty override to prevent crash when pressing ctrl-a in the treeview
        }

        protected override void SearchChanged(string newSearch)
        {
            mCooldownFilterAction.Ping();
        }

        protected override void ContextClickedItem(int id)
        {
            mMenu.Popup();
            Repaint();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            DrawTreeViewItem.InitializeStyles();

            if (args.item is MergeCategoryTreeViewItem)
            {
                MergeCategoryTreeViewItemGUI(
                    args.rowRect,
                    rowHeight,
                    (MergeCategoryTreeViewItem)args.item,
                    args.selected,
                    args.focused);
                return;
            }

            if (args.item is ChangeCategoryTreeViewItem)
            {
                ChangeCategoryTreeViewItemGUI(
                    args.rowRect,
                    rowHeight,
                    (ChangeCategoryTreeViewItem)args.item,
                    args.selected,
                    args.focused);
                return;
            }

            if (args.item is ClientDiffTreeViewItem)
            {
                ClientDiffTreeViewItemGUI(
                    args.rowRect,
                    rowHeight,
                    mDiffTree,
                    (ClientDiffTreeViewItem)args.item,
                    args.selected,
                    args.focused);
                return;
            }

            base.RowGUI(args);
        }

        internal void ClearModel()
        {
            mTreeViewItemIds.Clear();

            mDiffTree = new UnityDiffTree();
        }

        internal void BuildModel(
            List<ClientDiff> diffs,
            bool skipMergeTracking)
        {
            mTreeViewItemIds.Clear();

            mDiffTree.BuildCategories(diffs, skipMergeTracking);
        }

        internal void Refilter()
        {
            Filter filter = new Filter(searchString);
            mDiffTree.Filter(filter, ColumnsNames);

            mExpandCategories = true;
        }

        internal void Sort()
        {
            mDiffTree.Sort(
                PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn),
                sortAscending: true);
        }

        internal ClientDiffInfo GetMetaDiff(ClientDiffInfo diff)
        {
            return mDiffTree.GetMetaDiff(diff);
        }

        internal bool SelectionHasMeta()
        {
            if (!HasSelection())
                return false;

            ClientDiffInfo selectedDiff = GetSelectedDiffs(false)[0];

            if (selectedDiff == null)
                return false;

            return mDiffTree.HasMeta(selectedDiff);
        }

        internal List<ClientDiffInfo> GetSelectedDiffs(bool includeMetaFiles)
        {
            List<ClientDiffInfo> result = new List<ClientDiffInfo>();

            IList<int> selectedIds = GetSelection();

            if (selectedIds.Count == 0)
                return result;

            foreach (KeyValuePair<ITreeViewNode, int> item
                in mTreeViewItemIds.GetInfoItems())
            {
                if (!selectedIds.Contains(item.Value))
                    continue;

                if (!(item.Key is ClientDiffInfo))
                    continue;

                result.Add((ClientDiffInfo)item.Key);
            }

            if (includeMetaFiles)
                mDiffTree.FillWithMeta(result);

            return result;
        }

        void DelayedSearchChanged()
        {
            Refilter();

            Sort();

            Reload();

            TableViewOperations.ScrollToSelection(this);
        }

        static void RegenerateRows(
            UnityDiffTree diffTree,
            TreeViewItemIds<IDiffCategory, ITreeViewNode> treeViewItemIds,
            TreeView treeView,
            TreeViewItem rootItem,
            List<TreeViewItem> rows,
            bool expandCategories)
        {
            ClearRows(rootItem, rows);

            List<IDiffCategory> categories = diffTree.GetNodes();

            if (categories == null)
                return;

            foreach (IDiffCategory category in categories)
            {
                if (category is MergeCategory)
                {
                    AddMergeCategory(
                        rootItem,
                        category,
                        rows,
                        treeViewItemIds,
                        treeView,
                        expandCategories);
                }

                if (category is ChangeCategory)
                {
                    AddChangeCategory(
                        rootItem,
                        category,
                        rows,
                        treeViewItemIds,
                        treeView,
                        expandCategories);
                }
            }

            if (!expandCategories)
                return;

            treeView.state.expandedIDs = treeViewItemIds.GetCategoryIds();
        }

        static void ClearRows(
            TreeViewItem rootItem,
            List<TreeViewItem> rows)
        {
            if (rootItem.hasChildren)
                rootItem.children.Clear();

            rows.Clear();
        }

        static void AddMergeCategory(
            TreeViewItem rootItem,
            IDiffCategory category,
            List<TreeViewItem> rows,
            TreeViewItemIds<IDiffCategory, ITreeViewNode> treeViewItemIds,
            TreeView treeView,
            bool expandCategories)
        {
            int categoryId;
            if (!treeViewItemIds.TryGetCategoryItemId(category, out categoryId))
                categoryId = treeViewItemIds.AddCategoryItem(category);

            MergeCategoryTreeViewItem mergeCategoryTreeViewItem =
                new MergeCategoryTreeViewItem(
                    categoryId,
                    rootItem.depth + 1,
                    (MergeCategory)category);

            rootItem.AddChild(mergeCategoryTreeViewItem);
            rows.Add(mergeCategoryTreeViewItem);

            if (!expandCategories &&
                !treeView.IsExpanded(mergeCategoryTreeViewItem.id))
                return;

            for (int i = 0; i < category.GetChildrenCount(); i++)
            {
                IDiffCategory child = (IDiffCategory)((ITreeViewNode)category)
                    .GetChild(i);

                AddChangeCategory(
                    mergeCategoryTreeViewItem,
                    child,
                    rows,
                    treeViewItemIds,
                    treeView,
                    expandCategories);
            }
        }

        static void AddChangeCategory(
            TreeViewItem parentItem,
            IDiffCategory category,
            List<TreeViewItem> rows,
            TreeViewItemIds<IDiffCategory, ITreeViewNode> treeViewItemIds,
            TreeView treeView,
            bool expandCategories)
        {
            int categoryId;
            if (!treeViewItemIds.TryGetCategoryItemId(category, out categoryId))
                categoryId = treeViewItemIds.AddCategoryItem(category);

            ChangeCategoryTreeViewItem changeCategoryTreeViewItem =
                new ChangeCategoryTreeViewItem(
                    categoryId,
                    parentItem.depth + 1,
                    (ChangeCategory)category);

            parentItem.AddChild(changeCategoryTreeViewItem);
            rows.Add(changeCategoryTreeViewItem);

            if (!expandCategories &&
                !treeView.IsExpanded(changeCategoryTreeViewItem.id))
                return;

            AddClientDiffs(
                changeCategoryTreeViewItem,
                (ITreeViewNode)category,
                rows,
                treeViewItemIds);
        }

        static void AddClientDiffs(
            TreeViewItem parentItem,
            ITreeViewNode parentNode,
            List<TreeViewItem> rows,
            TreeViewItemIds<IDiffCategory, ITreeViewNode> treeViewItemIds)
        {
            for (int i = 0; i < parentNode.GetChildrenCount(); i++)
            {
                ITreeViewNode child = parentNode.GetChild(i);

                int nodeId;
                if (!treeViewItemIds.TryGetInfoItemId(child, out nodeId))
                    nodeId = treeViewItemIds.AddInfoItem(child);

                TreeViewItem changeTreeViewItem =
                    new ClientDiffTreeViewItem(
                        nodeId,
                        parentItem.depth + 1,
                        (ClientDiffInfo)child);

                parentItem.AddChild(changeTreeViewItem);
                rows.Add(changeTreeViewItem);
            }
        }

        static void MergeCategoryTreeViewItemGUI(
            Rect rowRect,
            float rowHeight,
            MergeCategoryTreeViewItem item,
            bool isSelected,
            bool isFocused)
        {
            Texture icon = Images.GetImage(Images.Name.IconMergeCategory);
            string label = item.Category.GetHeaderText();

            DrawTreeViewItem.ForCategoryItem(
                rowRect,
                rowHeight,
                item.depth,
                icon, label,
                isSelected,
                isFocused);
        }

        static void ChangeCategoryTreeViewItemGUI(
            Rect rowRect,
            float rowHeight,
            ChangeCategoryTreeViewItem item,
            bool isSelected,
            bool isFocused)
        {
            Texture icon = GetChangeCategoryIcon(item.Category);
            string label = item.Category.GetHeaderText();

            DrawTreeViewItem.ForCategoryItem(
                rowRect,
                rowHeight,
                item.depth,
                icon, label,
                isSelected,
                isFocused);
        }

        static void ClientDiffTreeViewItemGUI(
            Rect rowRect,
            float rowHeight,
            UnityDiffTree diffTree,
            ClientDiffTreeViewItem item,
            bool isSelected,
            bool isFocused)
        {
            string label = ClientDiffView.GetColumnText(
                item.Difference.DiffWithMount.Mount.RepSpec,
                item.Difference.DiffWithMount.Difference,
                PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn));

            if (diffTree.HasMeta(item.Difference))
                label = string.Concat(label, UnityConstants.TREEVIEW_META_LABEL);

            Texture icon = GetClientDiffIcon(
                item.Difference.DiffWithMount.Difference.IsDirectory,
                label);

            DrawTreeViewItem.ForItemCell(
                rowRect,
                rowHeight,
                item.depth,
                icon,
                null,
                label,
                isSelected,
                isFocused,
                false,
                false);
        }

        static Texture GetChangeCategoryIcon(ChangeCategory category)
        {
            switch (category.Type)
            {
                case ChangeCategoryType.Merged:
                    return Images.GetImage(Images.Name.IconMerged);
                case ChangeCategoryType.Changed:
                    return Images.GetImage(Images.Name.IconChanged);
                case ChangeCategoryType.Moved:
                    return Images.GetImage(Images.Name.IconMoved);
                case ChangeCategoryType.Deleted:
                    return Images.GetImage(Images.Name.IconDeleted);
                case ChangeCategoryType.FSProtection:
                    return Images.GetImage(Images.Name.IconFsChanged);
                case ChangeCategoryType.Added:
                    return Images.GetImage(Images.Name.IconAdded);
                default:
                    return null;
            }
        }

        static Texture GetClientDiffIcon(bool isDirectory, string path)
        {
            if (isDirectory)
                return Images.GetDirectoryIcon();

            return Images.GetFileIconFromCmPath(path);
        }

        bool mExpandCategories;

        TreeViewItemIds<IDiffCategory, ITreeViewNode> mTreeViewItemIds =
            new TreeViewItemIds<IDiffCategory, ITreeViewNode>();
        List<TreeViewItem> mRows = new List<TreeViewItem>();

        UnityDiffTree mDiffTree = new UnityDiffTree();

        readonly CooldownWindowDelayer mCooldownFilterAction;

        static readonly List<string> ColumnsNames = new List<string> {
            PlasticLocalization.GetString(PlasticLocalization.Name.PathColumn)};
        readonly DiffTreeViewMenu mMenu;
    }
}
