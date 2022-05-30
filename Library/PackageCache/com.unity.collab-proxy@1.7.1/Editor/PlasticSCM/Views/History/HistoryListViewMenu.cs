﻿using UnityEditor;
using UnityEngine;

using PlasticGui;
using PlasticGui.WorkspaceWindow.History;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.Tool;

namespace Unity.PlasticSCM.Editor.Views.History
{
    internal class HistoryListViewMenu
    {
        internal interface IMenuOperations
        {
            long GetSelectedChangesetId();
        }

        internal HistoryListViewMenu(
            IHistoryViewMenuOperations operations,
            IMenuOperations menuOperations,
            bool isGluonMode)
        {
            mOperations = operations;
            mMenuOperations = menuOperations;
            mIsGluonMode = isGluonMode;

            BuildComponents();
        }

        internal void Popup()
        {
            GenericMenu menu = new GenericMenu();

            UpdateMenuItems(menu);

            menu.ShowAsContext();
        }

        internal bool ProcessKeyActionIfNeeded(Event e)
        {
            HistoryMenuOperations operationToExecute = GetMenuOperation(e);

            if (operationToExecute == HistoryMenuOperations.None)
                return false;

            SelectedHistoryGroupInfo info =
                mOperations.GetSelectedHistoryGroupInfo();

            HistoryMenuOperations operations =
                HistoryMenuUpdater.GetAvailableMenuOperations(info);

            if (!operations.HasFlag(operationToExecute))
                return false;

            ProcessMenuOperation(operationToExecute, mOperations, mIsGluonMode);
            return true;
        }

        void OpenRevisionMenu_Click()
        {
            mOperations.OpenRevision();
        }

        void OpenRevisionWithMenu_Click()
        {
            mOperations.OpenRevisionWith();
        }

        void SaveRevisionasMenu_Click()
        {
            mOperations.SaveRevisionAs();
        }

        void DiffWithPreviousMenuItem_Click()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(mIsGluonMode))
                return;

            mOperations.DiffWithPrevious();
        }

        void DiffSelectedRevisionsMenu_Click()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(mIsGluonMode))
                return;

            mOperations.DiffSelectedRevisions();
        }

        void DiffChangesetMenu_Click()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(mIsGluonMode))
                return;

            mOperations.DiffChangeset();
        }
        void RevertToThisRevisionMenu_Click()
        {
            mOperations.RevertToThisRevision();
        }

        void UpdateMenuItems(GenericMenu menu)
        {
            SelectedHistoryGroupInfo info =
                mOperations.GetSelectedHistoryGroupInfo();

            HistoryMenuOperations operations =
                HistoryMenuUpdater.GetAvailableMenuOperations(info);

            if (operations == HistoryMenuOperations.None)
            {
                menu.AddDisabledItem(GetNoActionMenuItemContent(), false);
                return;
            }

            if (operations.HasFlag(HistoryMenuOperations.Open))
                menu.AddItem(mOpenRevisionMenuItemContent, false, OpenRevisionMenu_Click);
            else
                menu.AddDisabledItem(mOpenRevisionMenuItemContent, false);

            if (operations.HasFlag(HistoryMenuOperations.OpenWith))
                menu.AddItem(mOpenRevisionWithMenuItemContent, false, OpenRevisionWithMenu_Click);
            else
                menu.AddDisabledItem(mOpenRevisionWithMenuItemContent, false);

            if (operations.HasFlag(HistoryMenuOperations.SaveAs))
                menu.AddItem(mSaveRevisionAsMenuItemContent, false, SaveRevisionasMenu_Click);
            else
                menu.AddDisabledItem(mSaveRevisionAsMenuItemContent, false);

            menu.AddSeparator(string.Empty);

            if (operations.HasFlag(HistoryMenuOperations.DiffWithPrevious))
                menu.AddItem(mDiffWithPreviousMenuItemContent, false, DiffWithPreviousMenuItem_Click);

            if (operations.HasFlag(HistoryMenuOperations.DiffSelected))
                menu.AddItem(mDiffSelectedRevisionsMenuItemContent, false, DiffSelectedRevisionsMenu_Click);

            mDiffChangesetMenuItemContent.text =
                PlasticLocalization.GetString(PlasticLocalization.Name.HistoryMenuItemDiffChangeset) +
                string.Format(" {0}", GetSelectedChangesetName(mMenuOperations));

            if (operations.HasFlag(HistoryMenuOperations.DiffChangeset))
                menu.AddItem(mDiffChangesetMenuItemContent, false, DiffChangesetMenu_Click);
            else
                menu.AddDisabledItem(mDiffChangesetMenuItemContent, false);

            menu.AddSeparator(string.Empty);

            if (operations.HasFlag(HistoryMenuOperations.RevertTo))
                menu.AddItem(mRevertToThisRevisionMenuItemContent, false, RevertToThisRevisionMenu_Click);
            else
                menu.AddDisabledItem(mRevertToThisRevisionMenuItemContent, false);
        }

        static HistoryMenuOperations GetMenuOperation(Event e)
        {
            if (Keyboard.IsControlOrCommandKeyPressed(e) && Keyboard.IsKeyPressed(e, KeyCode.D))
                return HistoryMenuOperations.DiffWithPrevious;

            return HistoryMenuOperations.None;
        }

        static void ProcessMenuOperation(
            HistoryMenuOperations operationToExecute,
            IHistoryViewMenuOperations operations,
            bool isGluonMode)
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(isGluonMode))
                return;

            if (operationToExecute == HistoryMenuOperations.DiffWithPrevious)
            {
                operations.DiffWithPrevious();
                return;
            }
        }

        GUIContent GetNoActionMenuItemContent()
        {
            if (mNoActionMenuItemContent == null)
            {
                mNoActionMenuItemContent = new GUIContent(
                    PlasticLocalization.GetString(PlasticLocalization.
                        Name.NoActionMenuItem));
            }

            return mNoActionMenuItemContent;
        }

        static string GetSelectedChangesetName(IMenuOperations menuOperations)
        {
            long selectedChangesetId = menuOperations.GetSelectedChangesetId();

            if (selectedChangesetId == -1)
                return string.Empty;

            return selectedChangesetId.ToString();
        }

        void BuildComponents()
        {
            mOpenRevisionMenuItemContent = new GUIContent(PlasticLocalization.
                GetString(PlasticLocalization.Name.HistoryMenuItemOpen));
            mOpenRevisionWithMenuItemContent = new GUIContent(PlasticLocalization.
                GetString(PlasticLocalization.Name.HistoryMenuItemOpenWith));
            mSaveRevisionAsMenuItemContent = new GUIContent(PlasticLocalization.
                GetString(PlasticLocalization.Name.SaveThisRevisionAs));
            mDiffWithPreviousMenuItemContent = new GUIContent(string.Format("{0} {1}",
                PlasticLocalization.GetString(PlasticLocalization.Name.HistoryMenuItemDiffWithPrevious),
                GetPlasticShortcut.ForDiff()));
            mDiffSelectedRevisionsMenuItemContent = new GUIContent(PlasticLocalization.
                GetString(PlasticLocalization.Name.HistoryMenuItemDiffSelectedRevisions));
            mDiffChangesetMenuItemContent = new GUIContent();
            mRevertToThisRevisionMenuItemContent = new GUIContent(PlasticLocalization.
                GetString(PlasticLocalization.Name.HistoryMenuItemRevertToThisRevision));
        }

        GUIContent mNoActionMenuItemContent;

        GUIContent mOpenRevisionMenuItemContent;
        GUIContent mOpenRevisionWithMenuItemContent;
        GUIContent mSaveRevisionAsMenuItemContent;
        GUIContent mDiffWithPreviousMenuItemContent;
        GUIContent mDiffSelectedRevisionsMenuItemContent;
        GUIContent mDiffChangesetMenuItemContent;
        GUIContent mRevertToThisRevisionMenuItemContent;

        readonly IHistoryViewMenuOperations mOperations;
        readonly IMenuOperations mMenuOperations;
        readonly bool mIsGluonMode;
    }
}