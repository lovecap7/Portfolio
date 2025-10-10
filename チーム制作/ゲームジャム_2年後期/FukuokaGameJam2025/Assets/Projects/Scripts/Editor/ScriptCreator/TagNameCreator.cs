using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// タグ名を定数で管理するクラスを作成するスクリプト
/// </summary>
namespace myEditor.Creator
{
    public static class TagNameCreator
    {
        // 無効な文字を管理する配列
        private static readonly string[] INVALUD_CHARS =
        {
                " ", "!", "\"", "#", "$",
                "%", "&", "\'", "(", ")",
                "-", "=", "^",  "~", "\\",
                "|", "[", "{",  "@", "`",
                "]", "}", ":",  "*", ";",
                "+", "/", "?",  ".", ">",
                ",", "<"
        };

        // スクリプト保存パス
        const string SAVE_PATH = "Assets/Projects/Scripts/Game/Definition/TagName.cs";
        // ファイル名(拡張子あり)
        static readonly string FILENAME = Path.GetFileName(SAVE_PATH);
        // ファイル名(拡張子なし)
        static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(SAVE_PATH);

        /// <summary>
        /// タグ名を定数で管理するクラスを作成します
        /// </summary>
        public static void TagNameCreate()
        {
            if (!CanCreate())
            {
                return;
            }

            CreateScript();

            EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
        }

        /// <summary>
        /// スクリプトを作成します
        /// </summary>
        public static void CreateScript()
        {

            // コード生成
            var builder = new StringBuilder();

            builder.AppendLine("/// <summary>");
            builder.AppendLine("/// タグ名を定数で管理するクラス");
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
            builder.AppendLine("{");

            foreach (var n in InternalEditorUtility.tags.
                Select(c => new { var = RemoveInvalidChars(c), val = c }))
            {
                builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", n.var, n.val).AppendLine();
            }

            builder.AppendLine("}");

            // データ保存
            var directoryName = Path.GetDirectoryName(SAVE_PATH);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(SAVE_PATH, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// タグ名を定数で管理するクラスを作成できるかどうかを取得します
        /// </summary>
        public static bool CanCreate()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }

        /// <summary>
        /// 無効な文字を削除します
        /// </summary>
        public static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
            return str;
        }
    }
}