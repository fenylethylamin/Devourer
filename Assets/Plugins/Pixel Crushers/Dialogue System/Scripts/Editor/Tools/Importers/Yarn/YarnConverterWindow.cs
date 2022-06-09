#if USE_YARN

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


namespace PixelCrushers.DialogueSystem.Yarn
{
    [Serializable]
    public class YarnConverterPrefs : AbstractConverterWindowPrefs
    {
        public const string DefaultPlayerName = "Player";
        public const string DefaultActorRegex = "^(.+?)\\:";
        public const string DefaultLinePrefixRegex = "^.+?\\:\\s*";
        public const string DefaultLocaleRegex = ".+\\((.+)\\)\\.csv";
        public const string DefaultCustomCommandsSourceFile = "Assets/Scripts/YarnCustomCommands.cs";

        /// <summary>
        /// The name of the player's actor.
        /// </summary>
        public string playerName = DefaultPlayerName;

        /// <summary>
        /// The regular expression used to extract actor names from Yarn strings.
        /// </summary>
        public string actorRegex = DefaultActorRegex;

        /// <summary>
        /// The regular expression used to strip line prefixes from Yarn strings.
        /// </summary>
        public string linePrefixRegex = DefaultLinePrefixRegex;

        /// <summary>
        /// List of all Yarn source files for the project.
        /// </summary>
        public List<string> sourceFiles = new List<string>();

        /// <summary>
        /// The regular expression used to extract locale from localized string filenames.
        /// May possibly want to expose this to the user? Not really sure, probably best not to,
        /// since the Yarn Unity tool creates these filenames and expects a specific pattern.
        /// </summary>
        public string localeRegex = DefaultLocaleRegex;

        /// <summary>
        /// The list of localized string filenames.
        /// </summary>
        public List<string> localizedStringFiles = new List<string>();

        /// <summary>
        /// The path for the auto-generated custom commands source file.
        /// </summary>
        // public string customCommandsSourceFile;
        public string customCommandsSourceFile = DefaultCustomCommandsSourceFile;

        /// <summary>
        /// The name of the player's actor.
        /// </summary>
        public bool debug = false;
    }

    public class YarnConverterWindow : AbstractConverterWindow<YarnConverterPrefs>
    {

        [MenuItem("Tools/Pixel Crushers/Dialogue System/Import/Yarn...", false, 1)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(YarnConverterWindow), false, "Yarn Importer");
            window.minSize = new Vector2(400, 400);
        }

        public override string sourceFileExtension { get { return "yarn"; } }

        public const string PrefsKeyName = "PixelCrushers.DialogueSystem.YarnConverterSettings";
        public override string prefsKey { get { return PrefsKeyName; } }

        public static readonly string[] YarnExampleFiles = new string[]
        {
            // SallyPath,
            // ShipPath
            "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Sally.yarn",
            "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Ship.yarn",
        };

        public static readonly string[] RuntimeTestFiles = new string[]
        {
            // "Assets/Test/Yarn/RuntimeTestScripts/AllOperatorsTest.yarn",
            "Assets/Test/Yarn/RuntimeTestScripts/RunCommand.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/FormattedRunLineFromStart.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/ShortcutOptionToRunNode.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/UnreachableCodeFromRunNode.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/UnreachableCodeFromStop.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/Wait.yarn",
        };

        public static readonly string[] SyntaxTestFiles = new string[]
        {
            // "Assets/Test/Yarn/YarnExamples/SyntaxTest.yarn",
            "Assets/Test/Yarn/YarnExamples/Sally_Test.yarn",
            "Assets/Test/Yarn/YarnExamples/Ship_Test.yarn",
        };

        public static readonly string[] RopeworksYarnFiles = new string[]
        {
            // "Assets/Test/Yarn/Ropeworks/RopeworkBlank.yarn",
            "Assets/Test/Yarn/Ropeworks/RopeworkComplexExample.yarn",
            "Assets/Test/Yarn/Ropeworks/RopeworkSimpleExample.yarn",
            // "Assets/Test/Yarn/Ropeworks/RopeworksTestScript.yarn",
        };

        public static readonly string[] CustomerTestFiles = new string[]
        {
            "Assets/Test/Yarn/CustomerTest/Carla.yarn",
            "Assets/Test/Yarn/CustomerTest/Estatua.yarn",
            "Assets/Test/Yarn/CustomerTest/Hector.yarn",
            "Assets/Test/Yarn/CustomerTest/Livro.yarn",
            "Assets/Test/Yarn/CustomerTest/Peixoto.yarn",
            "Assets/Test/Yarn/CustomerTest/Tuto.yarn",
        };

        public static readonly string[] SpecialTestCaseFiles = new string[]
        {
            "Assets/Test/Yarn/SpecialTestCases/ConditionalShortcutOptions.yarn",
        };

        // public const string StanleyParableExample = "Assets/Test/Yarn/StanleyParable/StanleyParable.yarn";
        public static readonly string[] StanleyParableYarnFiles = new string[]
        {
            // StanleyParableExample // Has syntax error
            "Assets/Test/Yarn/StanleyParable/StanleyParable.yarn",
        };

        public static readonly string[] YarnSpinnerTestFiles = new string[]
        {
            // // AnalysisTest,
            // // Basic,
            // // Compiler,
            // // Example,
            // // Headers,
            // // InvalidNodeTitle, -- Illegal syntax
            // // Options,
            // RandomOptions,
            // // SkippedOptions,
            // // Strings,
            // // Tagging, -- Illegal syntax
            // "Assets/Test/Yarn/YarnSpinnerTests/AnalysisTest.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Basic.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Compiler.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Example.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Headers.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/InvalidNodeTitle.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Options.yarn",
            "Assets/Test/Yarn/YarnSpinnerTests/RandomOptions.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/SkippedOptions.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Strings.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Tagging.yarn",
        };

        private ReorderableList uiYarnSourceFileList;
        private ReorderableList uiLocalizedFileList;
        private string lastVisitedYarnSourceDirectory;
        private string lastVisitedLocalizedFileDirectory;

        public void PopulateDefaultPrefs()
        {
            prefs.playerName = YarnConverterPrefs.DefaultPlayerName;
            prefs.actorRegex = YarnConverterPrefs.DefaultActorRegex;
            prefs.linePrefixRegex = YarnConverterPrefs.DefaultLinePrefixRegex;
            prefs.localeRegex = YarnConverterPrefs.DefaultLocaleRegex;
            prefs.customCommandsSourceFile = YarnConverterPrefs.DefaultCustomCommandsSourceFile;

            prefs.sourceFiles = new List<string>();
            // prefs.sourceFiles = new List<string>(SyntaxTestFiles);
            // prefs.sourceFiles = new List<string>(RuntimeTestFiles);
            // prefs.sourceFiles = new List<string>(YarnExampleFiles);
            // prefs.sourceFiles = new List<string>(RopeworksYarnFiles);
            // prefs.sourceFiles = new List<string>(CustomerTestFiles);

            prefs.localizedStringFiles = new List<string>();
            //{
            //    "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Sally (de).csv",
            //    "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Ship (de).csv"
            //};

            prefs.outputFolder = "Assets";
            prefs.databaseFilename = "Dialogue Database";
            prefs.overwrite = true;
            prefs.merge = false;
            prefs.encodingType = EncodingType.Default;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            // PopulateDefaultPrefs(); // Comment this out for production, only use for testing

            InitializeReorderableLists();
        }

        protected void InitializeReorderableLists()
        {
            uiYarnSourceFileList = new ReorderableList(prefs.sourceFiles, typeof(string), true, true, true, true);
            uiYarnSourceFileList.drawHeaderCallback += DrawYarnSourceFileListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
            uiYarnSourceFileList.drawElementCallback += DrawYarnSourceFileListItem; // Delegate to draw the elements on the list
            uiYarnSourceFileList.onAddCallback += AddYarnSourceFileListItem; // Add stuff
            lastVisitedYarnSourceDirectory = (prefs.sourceFiles.Count > 0) ? Path.GetDirectoryName(prefs.sourceFiles[prefs.sourceFiles.Count - 1]) : "Assets";

            uiLocalizedFileList = new ReorderableList(prefs.localizedStringFiles, typeof(string), true, true, true, true);
            uiLocalizedFileList.drawHeaderCallback += DrawLocalizedFileListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
            uiLocalizedFileList.drawElementCallback += DrawLocalizedFileListItem; // Delegate to draw the elements on the list
            uiLocalizedFileList.onAddCallback += AddLocalizedFileListItem; // Add stuff
            lastVisitedLocalizedFileDirectory = (prefs.localizedStringFiles.Count > 0) ? Path.GetDirectoryName(prefs.localizedStringFiles[prefs.localizedStringFiles.Count - 1]) : "Assets";
        }

        protected override void DrawControls()
        {
            DrawSourceSection();
            DrawDestinationSection();
            DrawConversionButtons();
        }

        protected static GUIContent PlayerNameLabel = new GUIContent("Player Name", "Actor name to use for default player actor.");
        protected static GUIContent ActorRegexLabel = new GUIContent("Actor Regex", "How to extract actors names from Yarn dialogue lines. Defaults to 'NAME: Dialogue text'.");
        protected static GUIContent LinePrefixRegexLabel = new GUIContent("Line Prefix Regex", "How to extract dialogue text from Yarn dialogue lines. Defaults to 'Ignore-this: Dialogue text'.");
        protected static GUIContent CustomCommandsFileLabel = new GUIContent("Custom Commands File", "Filename to use when auto-generating script containing custom commands referenced in your Yarn stories.");
        protected static GUIContent DebugLabel = new GUIContent("Debug", "Log detailed debug info to the Console.");
        protected static GUIContent YarnSourceFilesLabel = new GUIContent("Yarn Source Files", "Yarn files to import.");
        protected static GUIContent LocalizedStringFilesLabel = new GUIContent("Localized String Files", "Yarn localization files.");

        protected override void DrawSourceSection()
        {
            prefs.playerName = EditorGUILayout.TextField(PlayerNameLabel, prefs.playerName)?.Trim();
            prefs.actorRegex = EditorGUILayout.TextField(ActorRegexLabel, prefs.actorRegex);
            prefs.linePrefixRegex = EditorGUILayout.TextField(LinePrefixRegexLabel, prefs.linePrefixRegex);
            prefs.customCommandsSourceFile = EditorGUILayout.TextField(CustomCommandsFileLabel, prefs.customCommandsSourceFile)?.Trim();
            uiYarnSourceFileList.DoLayoutList();
            uiLocalizedFileList.DoLayoutList();
            prefs.debug = EditorGUILayout.Toggle(DebugLabel, prefs.debug);
        }

        void DrawYarnSourceFileListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, YarnSourceFilesLabel);
        }

        void DrawYarnSourceFileListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= prefs.sourceFiles.Count)
            {
                return;
            }

            prefs.sourceFiles[index] = EditorGUI.TextField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), GUIContent.none, prefs.sourceFiles[index]);
        }

        private void AddYarnSourceFileListItem(ReorderableList list)
        {
            var fullPath = EditorUtility.OpenFilePanel("Select Yarn File", lastVisitedYarnSourceDirectory, "yarn");
            if (string.IsNullOrEmpty(fullPath)) return;

            // Truncate this if it's in the assets directory, it makes things way easier to read
            var displayPath = fullPath;
            if (fullPath.StartsWith(Application.dataPath))
            {
                displayPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            if (!prefs.sourceFiles.Contains(displayPath))
            {
                prefs.sourceFiles.Add(displayPath);
                lastVisitedYarnSourceDirectory = Path.GetDirectoryName(fullPath);
            }
        }

        private void DrawLocalizedFileListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, LocalizedStringFilesLabel);
        }

        private void DrawLocalizedFileListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= prefs.localizedStringFiles.Count)
            {
                return;
            }

            prefs.localizedStringFiles[index] = EditorGUI.TextField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), GUIContent.none, prefs.localizedStringFiles[index]);
        }

        private void AddLocalizedFileListItem(ReorderableList list)
        {
            var fullPath = EditorUtility.OpenFilePanel("Select localized file", lastVisitedLocalizedFileDirectory, "csv");
            if (string.IsNullOrEmpty(fullPath)) return;

            // Truncate this if it's in the assets directory, it makes things way easier to read
            var displayPath = fullPath;
            if (fullPath.StartsWith(Application.dataPath))
            {
                displayPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            if (!prefs.localizedStringFiles.Contains(displayPath))
            {
                prefs.localizedStringFiles.Add(displayPath);
                lastVisitedLocalizedFileDirectory = Path.GetDirectoryName(fullPath);
            }
        }

        protected override void ClearPrefs()
        {
            base.ClearPrefs();
            InitializeReorderableLists();
        }

        protected override bool IsReadyToConvert()
        {
            return
                prefs.sourceFiles.Count > 0 &&
                !string.IsNullOrEmpty(prefs.customCommandsSourceFile) &&
                !string.IsNullOrEmpty(prefs.databaseFilename) &&
                !string.IsNullOrEmpty(prefs.outputFolder);
        }

        protected override void Convert()
        {
            RunConverter();
        }

        private void WriteDialogueSystemChanges(DialogueDatabase database)
        {
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }

        private void RunConverter()
        {
            Debug.Log($"Starting Yarn project import ...");
            var yarnProject = new YarnProject(prefs);
            var dialogueDb = LoadOrCreateDatabase();
            var converter = new YarnConverter();
            converter.Convert(prefs, yarnProject, dialogueDb);
            WriteDialogueSystemChanges(dialogueDb);
            converter.GenerateCustomCommandBaseClass();
            Debug.Log($"Yarn project import complete - database written to: {AssetDatabase.GetAssetPath(dialogueDb)}");
        }
    }
}

#endif