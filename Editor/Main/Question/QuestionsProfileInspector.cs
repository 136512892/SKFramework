using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(QuestionsProfile))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/125045409?spm=1001.2014.3001.5502")]
    public sealed class QuestionsTargetInspector : AbstractEditor<QuestionsProfile>
    {
        private QuestionType currentType;
        private Color btnNormalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        private readonly GUIContent deleteContent = new GUIContent("-", "delete");
        private Vector2 scroll = Vector2.zero;
        private Dictionary<JudgeQuestion, bool> judgeFoldoutMap;
        private Dictionary<SingleChoiceQuestion, bool> singleChoicesFoldoutMap;
        private Dictionary<MultipleChoiceQuestion, bool> multipleChoicesFoldoutMap;
        private Dictionary<CompletionQuestion, bool> completionFoldoutMap;
        private Dictionary<EssayQuestion, bool> essayFoldoutMap;


        protected override void OnBaseEnable()
        {
            judgeFoldoutMap = new Dictionary<JudgeQuestion, bool>();
            singleChoicesFoldoutMap = new Dictionary<SingleChoiceQuestion, bool>();
            multipleChoicesFoldoutMap = new Dictionary<MultipleChoiceQuestion, bool>();
            completionFoldoutMap = new Dictionary<CompletionQuestion, bool>();
            essayFoldoutMap = new Dictionary<EssayQuestion, bool>();
        }

        protected override void OnBaseInspectorGUI()
        {
            OnTypeGUI();
            OnMenuGUI();
            OnDetailGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnTypeGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUI.color = currentType == QuestionType.JUDGE ? Color.white : btnNormalColor;
                if (GUILayout.Button("判断题", EditorStyles.miniButtonLeft)) currentType = QuestionType.JUDGE;
                GUI.color = currentType == QuestionType.SINGLE_CHOICE ? Color.white : btnNormalColor;
                if (GUILayout.Button("单选题", EditorStyles.miniButtonMid)) currentType = QuestionType.SINGLE_CHOICE;
                GUI.color = currentType == QuestionType.MULTIPLE_CHOICE ? Color.white : btnNormalColor;
                if (GUILayout.Button("多选题", EditorStyles.miniButtonMid)) currentType = QuestionType.MULTIPLE_CHOICE;
                GUI.color = currentType == QuestionType.COMPLETION ? Color.white : btnNormalColor;
                if (GUILayout.Button("填空题", EditorStyles.miniButtonMid)) currentType = QuestionType.COMPLETION;
                GUI.color = currentType == QuestionType.ESSAY ? Color.white : btnNormalColor;
                if (GUILayout.Button("论述题", EditorStyles.miniButtonRight)) currentType = QuestionType.ESSAY;
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
        private void OnMenuGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("展开", EditorStyles.miniButtonLeft))
            {
                switch (currentType)
                {
                    case QuestionType.JUDGE: Target.Judges.ForEach(m => judgeFoldoutMap[m] = true); break;
                    case QuestionType.SINGLE_CHOICE: Target.SingleChoices.ForEach(m => singleChoicesFoldoutMap[m] = true); break;
                    case QuestionType.MULTIPLE_CHOICE: Target.MultipleChoices.ForEach(m => multipleChoicesFoldoutMap[m] = true); break;
                    case QuestionType.COMPLETION: Target.Completions.ForEach(m => completionFoldoutMap[m] = true); break;
                    case QuestionType.ESSAY: Target.Essays.ForEach(m => essayFoldoutMap[m] = true); break;
                    default: break;
                }
            }
            if (GUILayout.Button("收缩", EditorStyles.miniButtonMid))
            {
                switch (currentType)
                {
                    case QuestionType.JUDGE: Target.Judges.ForEach(m => judgeFoldoutMap[m] = false); break;
                    case QuestionType.SINGLE_CHOICE: Target.SingleChoices.ForEach(m => singleChoicesFoldoutMap[m] = false); break;
                    case QuestionType.MULTIPLE_CHOICE: Target.MultipleChoices.ForEach(m => multipleChoicesFoldoutMap[m] = false); break;
                    case QuestionType.COMPLETION: Target.Completions.ForEach(m => completionFoldoutMap[m] = false); break;
                    case QuestionType.ESSAY: Target.Essays.ForEach(m => essayFoldoutMap[m] = false); break;
                    default: break;
                }
            }
            if (GUILayout.Button("添加", EditorStyles.miniButtonMid))
            {
                Undo.RecordObject(Target, "Add New");
                switch (currentType)
                {
                    case QuestionType.JUDGE: Target.Judges.Add(new JudgeQuestion() { Type = QuestionType.JUDGE }); break;
                    case QuestionType.SINGLE_CHOICE: Target.SingleChoices.Add(new SingleChoiceQuestion() { Type = QuestionType.SINGLE_CHOICE }); break;
                    case QuestionType.MULTIPLE_CHOICE: Target.MultipleChoices.Add(new MultipleChoiceQuestion() { Type = QuestionType.MULTIPLE_CHOICE }); break;
                    case QuestionType.COMPLETION: Target.Completions.Add(new CompletionQuestion() { Type = QuestionType.COMPLETION }); break;
                    case QuestionType.ESSAY: Target.Essays.Add(new EssayQuestion() { Type = QuestionType.ESSAY }); break;
                    default: break;
                }
            }
            if (GUILayout.Button("清空", EditorStyles.miniButtonRight))
            {
                Undo.RecordObject(Target, "Clear");
                if (EditorUtility.DisplayDialog("Prompt", "Are you sure clear the questions?", "Yes", "No"))
                {
                    switch (currentType)
                    {
                        case QuestionType.JUDGE: Target.Judges.Clear(); judgeFoldoutMap.Clear(); break;
                        case QuestionType.SINGLE_CHOICE: Target.SingleChoices.Clear(); singleChoicesFoldoutMap.Clear(); break;
                        case QuestionType.MULTIPLE_CHOICE: Target.MultipleChoices.Clear(); multipleChoicesFoldoutMap.Clear(); break;
                        case QuestionType.COMPLETION: Target.Completions.Clear(); completionFoldoutMap.Clear(); break;
                        case QuestionType.ESSAY: Target.Essays.Clear(); essayFoldoutMap.Clear(); break;
                        default: break;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        private void OnDetailGUI()
        {
            scroll = GUILayout.BeginScrollView(scroll);
            switch (currentType)
            {
                #region 判断题
                case QuestionType.JUDGE:
                    for (int i = 0; i < Target.Judges.Count; i++)
                    {
                        var current = Target.Judges[i];
                        if (!judgeFoldoutMap.ContainsKey(current)) judgeFoldoutMap.Add(current, true);

                        GUILayout.BeginHorizontal("IN Title");
                        judgeFoldoutMap[current] = EditorGUILayout.Foldout(judgeFoldoutMap[current], $"第 {current.Sequence} 题", true);
                        if (GUILayout.Button("×", GUILayout.Width(20f)))
                        {
                            Target.Judges.Remove(current);
                            judgeFoldoutMap.Remove(current);
                            break;
                        }
                        GUILayout.EndHorizontal();
                        if (judgeFoldoutMap[current])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题号：", GUILayout.Width(40));
                            var newValue = EditorGUILayout.IntField(current.Sequence, GUILayout.Width(30));
                            if (current.Sequence != newValue)
                            {
                                Undo.RecordObject(Target, "Judge Sequence");
                                current.Sequence = newValue;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("问题：", GUILayout.Width(40));
                            var newQ = EditorGUILayout.TextArea(current.Question, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newQ != current.Question)
                            {
                                Undo.RecordObject(Target, "Judge Question");
                                current.Question = newQ;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("正确：", GUILayout.Width(40));
                            var newP = EditorGUILayout.TextField(current.Positive);
                            if (newP != current.Positive)
                            {
                                Undo.RecordObject(Target, "Positive");
                                current.Positive = newP;
                            }
                            var newAnswer = EditorGUILayout.Toggle(current.Answer == true, GUILayout.Width(15));
                            if (newAnswer != (current.Answer == true))
                            {
                                Undo.RecordObject(Target, "Judge Answer");
                                current.Answer = true;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("错误：", GUILayout.Width(40));
                            var newN = EditorGUILayout.TextField(current.Negative);
                            if (newN != current.Positive)
                            {
                                Undo.RecordObject(Target, "Negative");
                                current.Negative = newN;
                            }
                            var newAns = EditorGUILayout.Toggle(current.Answer == false, GUILayout.Width(15));
                            if (newAns != (current.Answer == false))
                            {
                                Undo.RecordObject(Target, "Judge Answer");
                                current.Answer = false;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题解：", GUILayout.Width(40));
                            var newA = EditorGUILayout.TextArea(current.Analysis, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newA != current.Analysis)
                            {
                                Undo.RecordObject(Target, "Judge Analysis");
                                current.Analysis = newA;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
                #endregion
                #region 单选题
                case QuestionType.SINGLE_CHOICE:
                    for (int i = 0; i < Target.SingleChoices.Count; i++)
                    {
                        var current = Target.SingleChoices[i];
                        if (!singleChoicesFoldoutMap.ContainsKey(current)) singleChoicesFoldoutMap.Add(current, true);

                        GUILayout.BeginHorizontal("IN Title");
                        singleChoicesFoldoutMap[current] = EditorGUILayout.Foldout(singleChoicesFoldoutMap[current], $"第 {current.Sequence} 题", true);
                        if (GUILayout.Button("×", GUILayout.Width(20f)))
                        {
                            Target.SingleChoices.Remove(current);
                            singleChoicesFoldoutMap.Remove(current);
                            break;
                        }
                        GUILayout.EndHorizontal();

                        if (singleChoicesFoldoutMap[current])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题号：", GUILayout.Width(40));
                            var newS = EditorGUILayout.IntField(current.Sequence, GUILayout.Width(30));
                            if (current.Sequence != newS)
                            {
                                Undo.RecordObject(Target, "SingleChoices Sequence");
                                current.Sequence = newS;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("问题：", GUILayout.Width(40));
                            var newQ = EditorGUILayout.TextArea(current.Question, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newQ != current.Question)
                            {
                                Undo.RecordObject(Target, "SingleChoices Question");
                                current.Question = newQ;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("选项：", GUILayout.Width(40));
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("＋", GUILayout.Width(25)))
                            {
                                Undo.RecordObject(Target, "SingleChoices Add");
                                current.Choices.Add(new QuestionChoice("选项描述", null));
                            }
                            GUILayout.FlexibleSpace();
                            current.choiceType = (ChoiceType)EditorGUILayout.EnumPopup(current.choiceType, GUILayout.Width(80f));
                            GUILayout.Label("(类型)");
                            GUILayout.EndHorizontal();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                            for (int k = 0; k < current.Choices.Count; k++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                GUILayout.Label($"{Alphabet.Values[k]}.", GUILayout.Width(20));
                                switch (current.choiceType)
                                {
                                    case ChoiceType.Text:
                                        current.Choices[k].text = GUILayout.TextField(current.Choices[k].text);
                                        break;
                                    case ChoiceType.Pic:
                                        current.Choices[k].pic = EditorGUILayout.ObjectField(current.Choices[k].pic, typeof(Sprite), false) as Sprite;
                                        break;
                                    case ChoiceType.TextAndPic:
                                        current.Choices[k].text = GUILayout.TextField(current.Choices[k].text);
                                        current.Choices[k].pic = EditorGUILayout.ObjectField(current.Choices[k].pic, typeof(Sprite), false, GUILayout.Width(110f)) as Sprite;
                                        break;
                                }
                                var newValue = EditorGUILayout.Toggle(current.Answer == k, GUILayout.Width(15));
                                if (newValue)
                                {
                                    Undo.RecordObject(Target, "SingleChoices Answer");
                                    current.Answer = k;
                                }
                                if (GUILayout.Button(deleteContent, "MiniButton", GUILayout.Width(18)))
                                {
                                    Undo.RecordObject(Target, "Delete SingleChoice");
                                    current.Choices.RemoveAt(k);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题解：", GUILayout.Width(40));
                            var newA = EditorGUILayout.TextArea(current.Analysis, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newA != current.Analysis)
                            {
                                Undo.RecordObject(Target, "SingleChoices Analysis");
                                current.Analysis = newA;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
                #endregion
                #region 多选题
                case QuestionType.MULTIPLE_CHOICE:
                    for (int i = 0; i < Target.MultipleChoices.Count; i++)
                    {
                        var current = Target.MultipleChoices[i];
                        if (!multipleChoicesFoldoutMap.ContainsKey(current)) multipleChoicesFoldoutMap.Add(current, true);

                        GUILayout.BeginHorizontal("IN Title");
                        multipleChoicesFoldoutMap[current] = EditorGUILayout.Foldout(multipleChoicesFoldoutMap[current], $"第 {current.Sequence} 题", true);
                        if (GUILayout.Button("×", GUILayout.Width(20f)))
                        {
                            Target.MultipleChoices.Remove(current);
                            multipleChoicesFoldoutMap.Remove(current);
                            break;
                        }
                        GUILayout.EndHorizontal();

                        if (multipleChoicesFoldoutMap[current])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题号：", GUILayout.Width(40));
                            var newS = EditorGUILayout.IntField(current.Sequence, GUILayout.Width(30));
                            if (newS != current.Sequence)
                            {
                                Undo.RecordObject(Target, "MultipleChoices Sequence");
                                current.Sequence = newS;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("问题：", GUILayout.Width(40));
                            var newQ = EditorGUILayout.TextArea(current.Question, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newQ != current.Question)
                            {
                                Undo.RecordObject(Target, "MultipleChoices Question");
                                current.Question = newQ;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("选项：", GUILayout.Width(40));
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("＋", GUILayout.Width(25)))
                            {
                                Undo.RecordObject(Target, "SingleChoices Add");
                                current.Choices.Add(new QuestionChoice("选项描述", null));
                            }
                            GUILayout.FlexibleSpace();
                            current.choiceType = (ChoiceType)EditorGUILayout.EnumPopup(current.choiceType, GUILayout.Width(80f));
                            GUILayout.Label("(类型)");
                            GUILayout.EndHorizontal();
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                            for (int k = 0; k < current.Choices.Count; k++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                GUILayout.Label($"{Alphabet.Values[k]}.", GUILayout.Width(20));
                                switch (current.choiceType)
                                {
                                    case ChoiceType.Text:
                                        current.Choices[k].text = GUILayout.TextField(current.Choices[k].text);
                                        break;
                                    case ChoiceType.Pic:
                                        current.Choices[k].pic = EditorGUILayout.ObjectField(current.Choices[k].pic, typeof(Sprite), false) as Sprite;
                                        break;
                                    case ChoiceType.TextAndPic:
                                        current.Choices[k].text = GUILayout.TextField(current.Choices[k].text);
                                        current.Choices[k].pic = EditorGUILayout.ObjectField(current.Choices[k].pic, typeof(Sprite), false, GUILayout.Width(110f)) as Sprite;
                                        break;
                                }
                                var newValue = EditorGUILayout.Toggle(current.Answers.Contains(k), GUILayout.Width(15));
                                if (newValue != current.Answers.Contains(k))
                                {
                                    Undo.RecordObject(Target, "MultipleChoices Answers");
                                    if (newValue)
                                        current.Answers.Add(k);
                                    else
                                        current.Answers.Remove(k);
                                }
                                if (GUILayout.Button(deleteContent, "MiniButton", GUILayout.Width(18)))
                                {
                                    Undo.RecordObject(Target, "Delete MultipleChoice");
                                    current.Choices.RemoveAt(k);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题解：", GUILayout.Width(40));
                            var newA = EditorGUILayout.TextArea(current.Analysis, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newA != current.Analysis)
                            {
                                Undo.RecordObject(Target, "MultipleChoices Analysis");
                                current.Analysis = newA;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
                #endregion
                #region 填空题
                case QuestionType.COMPLETION:
                    for (int i = 0; i < Target.Completions.Count; i++)
                    {
                        var current = Target.Completions[i];
                        if (!completionFoldoutMap.ContainsKey(current)) completionFoldoutMap.Add(current, true);

                        GUILayout.BeginHorizontal("IN Title");
                        completionFoldoutMap[current] = EditorGUILayout.Foldout(completionFoldoutMap[current], $"第 {current.Sequence} 题", true);
                        if (GUILayout.Button("×", GUILayout.Width(20f)))
                        {
                            Target.Completions.Remove(current);
                            completionFoldoutMap.Remove(current);
                            break;
                        }
                        GUILayout.EndHorizontal();

                        if (completionFoldoutMap[current])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题号：", GUILayout.Width(40));
                            var newS = EditorGUILayout.IntField(current.Sequence, GUILayout.Width(30));
                            if (newS != current.Sequence)
                            {
                                Undo.RecordObject(Target, "Completion Sequence");
                                current.Sequence = newS;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("问题：", GUILayout.Width(40));
                            var newQ = EditorGUILayout.TextArea(current.Question, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newQ != current.Question)
                            {
                                Undo.RecordObject(Target, "Completion Question");
                                current.Question = newQ;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("答案：", GUILayout.Width(40));
                            if (GUILayout.Button("＋", GUILayout.Width(25)))
                            {
                                Undo.RecordObject(Target, "CompletionAnswers Add");
                                current.Answers.Add(null);
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                            for (int n = 0; n < current.Answers.Count; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                GUILayout.Label($"({n + 1}).", GUILayout.Width(30));
                                var newC = EditorGUILayout.TextField(current.Answers[n]);
                                if (current.Answers[n] != newC)
                                {
                                    Undo.RecordObject(Target, "CompletionAnswer");
                                    current.Answers[n] = newC;
                                }
                                if (GUILayout.Button(deleteContent, "MiniButton", GUILayout.Width(18)))
                                {
                                    Undo.RecordObject(Target, "CompletionAnswers Remove");
                                    current.Answers.RemoveAt(n);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndVertical();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题解：", GUILayout.Width(40));
                            var newA = EditorGUILayout.TextArea(current.Analysis, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newA != current.Analysis)
                            {
                                Undo.RecordObject(Target, "Completion Analysis");
                                current.Analysis = newA;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
                #endregion
                #region 论述题
                case QuestionType.ESSAY:
                    for (int i = 0; i < Target.Essays.Count; i++)
                    {
                        var current = Target.Essays[i];
                        if (!essayFoldoutMap.ContainsKey(current)) essayFoldoutMap.Add(current, true);

                        GUILayout.BeginHorizontal("IN Title");
                        essayFoldoutMap[current] = EditorGUILayout.Foldout(essayFoldoutMap[current], $"第 {current.Sequence} 题", true);
                        if (GUILayout.Button("×", GUILayout.Width(20f)))
                        {
                            Target.Essays.Remove(current);
                            essayFoldoutMap.Remove(current);
                            break;
                        }
                        GUILayout.EndHorizontal();

                        if (essayFoldoutMap[current])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题号：", GUILayout.Width(40));
                            var newS = EditorGUILayout.IntField(current.Sequence, GUILayout.Width(30));
                            if (newS != current.Sequence)
                            {
                                Undo.RecordObject(Target, "Essay Sequence");
                                current.Sequence = newS;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("问题：", GUILayout.Width(40));
                            var newQ = EditorGUILayout.TextArea(current.Question, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newQ != current.Question)
                            {
                                Undo.RecordObject(Target, "Essay Question");
                                current.Question = newQ;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("答案：", GUILayout.Width(40));
                            var newA = EditorGUILayout.TextArea(current.Answer, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newA != current.Answer)
                            {
                                Undo.RecordObject(Target, "Essay Answer");
                                current.Answer = newA;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("题解：", GUILayout.Width(40));
                            var newV = EditorGUILayout.TextArea(current.Analysis, new GUIStyle(GUI.skin.textArea) { stretchWidth = false }, GUILayout.ExpandWidth(true));
                            if (newV != current.Analysis)
                            {
                                Undo.RecordObject(Target, "Essay Analysis");
                                current.Analysis = newV;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
                #endregion
                default:
                    GUILayout.Label("Unknown Question Type.");
                    break;
            }
            GUILayout.EndScrollView();
        }
    }
}