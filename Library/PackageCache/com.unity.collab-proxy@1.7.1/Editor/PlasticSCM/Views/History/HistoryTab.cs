﻿using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Codice.CM.Common;
using Codice.Client.Common;

using GluonGui;
using PlasticGui;
using PlasticGui.WorkspaceWindow;
using PlasticGui.WorkspaceWindow.Diff;
using PlasticGui.WorkspaceWindow.History;

using GluonRevertOperation = GluonGui.WorkspaceWindow.Views.Details.History.RevertOperation;
using HistoryDescriptor = GluonGui.WorkspaceWindow.Views.Details.History.HistoryDescriptor;

using Unity.PlasticSCM.Editor.AssetUtils;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.UI.Progress;
using Unity.PlasticSCM.Editor.UI.Tree;
using Unity.PlasticSCM.Editor.Views.Changesets;
using Unity.PlasticSCM.Editor.Tool;

namespace Unity.PlasticSCM.Editor.Views.History
{
    internal class HistoryTab :
        IRefreshableView,
        HistoryViewLogic.IHistoryView,
        HistoryListViewMenu.IMenuOperations,
        IHistoryViewMenuOperations
    {
        internal HistoryTab(
            WorkspaceInfo wkInfo,
            IWorkspaceWindow workspaceWindow,
            RepositorySpec repSpec,
            NewIncomingChangesUpdater newIncomingChangesUpdater,
            ViewHost viewHost,
            EditorWindow parentWindow,
            bool isGluonMode)
        {
            mWkInfo = wkInfo;
            mWorkspaceWindow = workspaceWindow;
            mRepSpec = repSpec;
            mNewIncomingChangesUpdater = newIncomingChangesUpdater;
            mViewHost = viewHost;
            mParentWindow = parentWindow;
            mIsGluonMode = isGluonMode;

            mGuiMessage = new UnityPlasticGuiMessage(parentWindow);

            BuildComponents(wkInfo, repSpec);

            mProgressControls = new ProgressControlsForViews();

            mHistoryViewLogic = new HistoryViewLogic(
                wkInfo, this, mProgressControls);
        }

        internal void RefreshForItem(
            long itemId,
            string path,
            bool isDirectory)
        {
            mItemId = itemId;
            mPath = path;
            mIsDirectory = isDirectory;

            ((IRefreshableView)this).Refresh();
        }

        internal void Update()
        {
            mProgressControls.UpdateProgress(mParentWindow);
        }

        internal void OnGUI()
        {
            DoActionsToolbar(
                this,
                mProgressControls,
                mSearchField,
                mHistoryListView,
                GetViewTitle(mPath));

            DoHistoryArea(
                mHistoryListView,
                mProgressControls.IsOperationRunning());
        }

        internal void OnDisable()
        {
            mSearchField.downOrUpArrowKeyPressed -=
                SearchField_OnDownOrUpArrowKeyPressed;

            TreeHeaderSettings.Save(
                mHistoryListView.multiColumnHeader.state,
                UnityConstants.HISTORY_TABLE_SETTINGS_NAME);
        }

        void IRefreshableView.Refresh()
        {
            mHistoryViewLogic.RefreshForItem(mRepSpec, mItemId);
        }

        List<RepObjectInfo> HistoryViewLogic.IHistoryView.GetSelectedRevisions()
        {
            return HistorySelection.GetSelectedRepObjectInfos(mHistoryListView);
        }

        void HistoryViewLogic.IHistoryView.SelectRevisions(
            List<RepObjectInfo> revisionsToSelect)
        {
            HistorySelection.SelectRevisions(
                mHistoryListView, revisionsToSelect);
        }

        void HistoryViewLogic.IHistoryView.UpdateData(
            HistoryRevisionList list, long loadedRevisionId)
        {
            mHistoryListView.BuildModel(list, loadedRevisionId);

            mHistoryListView.Refilter();

            mHistoryListView.Sort();

            mHistoryListView.Reload();
        }

        long HistoryListViewMenu.IMenuOperations.GetSelectedChangesetId()
        {
            HistoryRevision revision = HistorySelection.
                GetSelectedHistoryRevision(mHistoryListView);

            if (revision == null)
                return -1;

            return revision.ChangeSet;
        }

        SelectedHistoryGroupInfo IHistoryViewMenuOperations.GetSelectedHistoryGroupInfo()
        {
            return SelectedHistoryGroupInfo.BuildFromSelection(
                HistorySelection.GetSelectedRepObjectInfos(mHistoryListView),
                HistorySelection.GetSelectedHistoryRevisions(mHistoryListView),
                mHistoryListView.GetLoadedRevisionId(),
                mIsDirectory);
        }

        void IHistoryViewMenuOperations.OpenRevision()
        {
            OpenRevisionOperation.Open(
                mRepSpec,
                Path.GetFileName(mPath),
                HistorySelection.GetSelectedHistoryRevisions(
                    mHistoryListView));
        }

        void IHistoryViewMenuOperations.OpenRevisionWith()
        {
            List<HistoryRevision> revisions = HistorySelection.
                GetSelectedHistoryRevisions(mHistoryListView);

            OpenRevisionOperation.OpenWith(
                mRepSpec,
                FileSystemOperation.GetExePath(),
                Path.GetFileName(mPath),
                revisions);
        }

        void IHistoryViewMenuOperations.SaveRevisionAs()
        {
            HistoryRevision revision = HistorySelection.
                GetSelectedHistoryRevision(mHistoryListView);

            string defaultFileName = DefaultRevisionName.Get(
                Path.GetFileName(mPath), revision.ChangeSet);

            string destinationPath = SaveAction.GetDestinationPath(
                mWkInfo.ClientPath, mPath, defaultFileName);

            if (string.IsNullOrEmpty(destinationPath))
                return;

            SaveRevisionOperation.SaveRevision(
                mRepSpec,
                destinationPath,
                revision,
                mProgressControls);
        }

        void IHistoryViewMenuOperations.DiffWithPrevious()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(mIsGluonMode))
                return;

            HistoryRevision revision = HistorySelection.
                GetSelectedHistoryRevision(mHistoryListView);

            DiffOperation.DiffWithPrevious(
                mWkInfo,
                mRepSpec,
                Path.GetFileName(mPath),
                string.Empty,
                revision.Id,
                mItemId,
                revision.ChangeSet,
                mProgressControls,
                null,
                null);
        }

        void IHistoryViewMenuOperations.DiffSelectedRevisions()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(mIsGluonMode))
                return;

            List<HistoryRevision> revisions = HistorySelection.
                GetSelectedHistoryRevisions(mHistoryListView);

            bool areReversed = revisions[0].Id > revisions[1].Id;

            DiffOperation.DiffRevisions(
                mWkInfo,
                mRepSpec,
                Path.GetFileName(mPath),
                string.Empty,
                mItemId,
                revisions[(areReversed) ? 1 : 0],
                revisions[(areReversed) ? 0 : 1],
                mProgressControls,
                null,
                null);
        }

        void IHistoryViewMenuOperations.DiffChangeset()
        {
            HistoryRevision revision = HistorySelection.
                GetSelectedHistoryRevision(mHistoryListView);

            LaunchDiffOperations.DiffChangeset(
                mRepSpec, revision.ChangeSet, mIsGluonMode);
        }

        void IHistoryViewMenuOperations.RevertToThisRevision()
        {
            HistoryRevision revision = HistorySelection.
                GetSelectedHistoryRevision(mHistoryListView);

            string fullPath = GetFullPath(mWkInfo.ClientPath, mPath);

            if (mIsGluonMode)
            {
                HistoryDescriptor historyDescriptor = new HistoryDescriptor(
                    mRepSpec, fullPath, mItemId, revision.Id, mIsDirectory);

                GluonRevertOperation.RevertToThisRevision(
                    mWkInfo,
                    mViewHost,
                    mProgressControls,
                    historyDescriptor,
                    revision,
                    mGuiMessage,
                    RefreshAsset.UnityAssetDatabase);
                return;
            }

            RevertOperation.RevertToThisRevision(
                mWkInfo,
                mProgressControls,
                mWorkspaceWindow,
                mRepSpec,
                revision,
                fullPath,
                mGuiMessage,
                mNewIncomingChangesUpdater,
                RefreshAsset.UnityAssetDatabase);
        }

        void SearchField_OnDownOrUpArrowKeyPressed()
        {
            mHistoryListView.SetFocusAndEnsureSelectedItem();
        }

        static string GetFullPath(string wkPath, string path)
        {
            if (PathHelper.IsContainedOn(path, wkPath))
                return path;

            return WorkspacePath.GetWorkspacePathFromCmPath(
                wkPath, path, Path.DirectorySeparatorChar);
        }

        static void DoActionsToolbar(
            IRefreshableView refreshableView,
            ProgressControlsForViews progressControls,
            SearchField searchField,
            HistoryListView listView,
            string viewTitle)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Label(
                viewTitle,
                UnityStyles.HistoryTab.HeaderLabel);

            if (progressControls.IsOperationRunning())
            {
                DrawProgressForViews.ForIndeterminateProgress(
                    progressControls.ProgressData);
            }

            GUILayout.FlexibleSpace();

            DrawSearchField.For(
                searchField,
                listView,
                UnityConstants.SEARCH_FIELD_WIDTH);

            DoRefreshButton(
                refreshableView,
                progressControls.IsOperationRunning());

            EditorGUILayout.EndHorizontal();
        }

        static void DoRefreshButton(
            IRefreshableView refreshableView,
            bool isOperationRunning)
        {
            EditorGUI.BeginDisabledGroup(isOperationRunning);

            if (GUILayout.Button(new GUIContent(
                    Images.GetRefreshIcon()), EditorStyles.toolbarButton))
                refreshableView.Refresh();

            EditorGUI.EndDisabledGroup();
        }

        static void DoHistoryArea(
            HistoryListView historyListView,
            bool isOperationRunning)
        {
            GUI.enabled = !isOperationRunning;

            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);

            historyListView.OnGUI(rect);

            GUI.enabled = true;
        }

        static string GetViewTitle(string path)
        {
            path = PathHelper.RemoveLastSlash(
                path, Path.DirectorySeparatorChar);

            return PlasticLocalization.GetString(
                PlasticLocalization.Name.HistoryViewTitle,
                Path.GetFileName(path));
        }

        void BuildComponents(
            WorkspaceInfo wkInfo,
            RepositorySpec repSpec)
        {
            mSearchField = new SearchField();
            mSearchField.downOrUpArrowKeyPressed += SearchField_OnDownOrUpArrowKeyPressed;

            HistoryListHeaderState headerState =
                HistoryListHeaderState.GetDefault();
            TreeHeaderSettings.Load(headerState,
                UnityConstants.HISTORY_TABLE_SETTINGS_NAME,
                (int)HistoryListColumn.CreationDate,
                false);

            mHistoryListView = new HistoryListView(
                wkInfo.ClientPath,
                repSpec,
                headerState,
                new HistoryListViewMenu(this, this, mIsGluonMode),
                HistoryListHeaderState.GetColumnNames());

            mHistoryListView.Reload();
        }

        SearchField mSearchField;
        HistoryListView mHistoryListView;

        long mItemId;
        string mPath;
        bool mIsDirectory;

        readonly WorkspaceInfo mWkInfo;
        readonly HistoryViewLogic mHistoryViewLogic;
        readonly ProgressControlsForViews mProgressControls;
        readonly IWorkspaceWindow mWorkspaceWindow;
        readonly RepositorySpec mRepSpec;
        readonly bool mIsGluonMode;
        readonly EditorWindow mParentWindow;
        readonly ViewHost mViewHost;
        readonly GuiMessage.IGuiMessage mGuiMessage;
        readonly NewIncomingChangesUpdater mNewIncomingChangesUpdater;
    }
}