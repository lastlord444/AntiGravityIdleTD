using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using BlockForge.Core;
using BlockForge.Gameplay;
using BlockForge.UI;
using BlockForge.Meta;

namespace BlockForge.Editor
{
    /// <summary>
    /// Sahneleri otomatik kurar. Menu: BlockForge/Setup All Scenes
    /// </summary>
    public static class SceneSetupTool
    {
        [MenuItem("BlockForge/Setup All Scenes")]
        public static void SetupAllScenes()
        {
            SetupBootScene();
            SetupRunScene();
            SetupHomeScene();
            SetupBuildSettings();
            Debug.Log("[SceneSetupTool] Tüm sahneler kuruldu!");
        }

        [MenuItem("BlockForge/Setup Run Scene Only")]
        public static void SetupRunSceneOnly()
        {
            SetupRunScene();
            Debug.Log("[SceneSetupTool] Run sahnesi kuruldu!");
        }

        private static void SetupBootScene()
        {
            string path = "Assets/_Project/Scenes/Boot.unity";
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

            // Boot sahnesi zaten kurulu olabilir, hierarchy kontrol
            var roots = scene.GetRootGameObjects();
            bool hasBootstrapper = false;
            foreach (var go in roots)
            {
                if (go.GetComponent<Core.GameBootstrapper>() != null)
                    hasBootstrapper = true;
            }

            if (!hasBootstrapper)
            {
                // Camera
                var camGo = new GameObject("Main Camera");
                var cam = camGo.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.1f, 0.1f, 0.15f);
                cam.orthographic = true;
                cam.orthographicSize = 5;
                camGo.transform.position = new Vector3(0, 0, -10);
                camGo.tag = "MainCamera";

                // GameBootstrapper
                var bootstrapperGo = new GameObject("GameBootstrapper");
                bootstrapperGo.AddComponent<Core.GameBootstrapper>();

                // Canvas
                var canvasGo = new GameObject("LoadingCanvas");
                var canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var scaler = canvasGo.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1080, 1920);
                scaler.matchWidthOrHeight = 0.5f;
                canvasGo.AddComponent<GraphicRaycaster>();
            }

            EditorSceneManager.SaveScene(scene, path);
            Debug.Log("[SceneSetupTool] Boot sahnesi kuruldu.");
        }

        private static void SetupRunScene()
        {
            string path = "Assets/_Project/Scenes/Run.unity";
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

            // Sahneyi temizle (varsa mevcut objeler)
            foreach (var go in scene.GetRootGameObjects())
            {
                Object.DestroyImmediate(go);
            }

            // === CAMERA ===
            var camGo = new GameObject("Main Camera");
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.08f, 0.08f, 0.12f);
            cam.orthographic = true;
            cam.orthographicSize = 5;
            camGo.transform.position = new Vector3(0, 0, -10);
            camGo.tag = "MainCamera";

            // === EventSystem ===
            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            // === UI CANVAS ===
            var canvasGo = new GameObject("UICanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();

            // -- Top Bar --
            var topBar = CreateUIPanel("TopBar", canvasGo.transform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), 
                new Vector2(0, -20), new Vector2(0, 120), new Color(0.12f, 0.12f, 0.18f, 0.9f));
            
            var scoreText = CreateTMPText("ScoreText", topBar.transform, "0", 48, TextAlignmentOptions.Center,
                new Vector2(0.3f, 0), new Vector2(0.7f, 1));
            
            var linesText = CreateTMPText("LinesClearedText", topBar.transform, "Lines: 0", 24, TextAlignmentOptions.Left,
                new Vector2(0, 0), new Vector2(0.3f, 1));
            
            var energyText = CreateTMPText("EnergyText", topBar.transform, "E: 0", 24, TextAlignmentOptions.Right,
                new Vector2(0.7f, 0), new Vector2(1, 1));

            // -- Grid Container --
            var gridContainer = new GameObject("GridContainer");
            gridContainer.transform.SetParent(canvasGo.transform, false);
            var gridRect = gridContainer.AddComponent<RectTransform>();
            gridRect.anchorMin = new Vector2(0.05f, 0.3f);
            gridRect.anchorMax = new Vector2(0.95f, 0.85f);
            gridRect.offsetMin = Vector2.zero;
            gridRect.offsetMax = Vector2.zero;
            var gridImage = gridContainer.AddComponent<Image>();
            gridImage.color = new Color(0.15f, 0.15f, 0.2f, 0.5f);

            // -- Shape Slots Bar --
            var shapeSlotsBar = CreateUIPanel("ShapeSlotsBar", canvasGo.transform, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0),
                new Vector2(0, 20), new Vector2(0, 200), new Color(0.1f, 0.1f, 0.15f, 0.8f));

            // 3 Shape Slot
            for (int i = 0; i < 3; i++)
            {
                var slot = new GameObject($"ShapeSlot_{i}");
                slot.transform.SetParent(shapeSlotsBar.transform, false);
                var slotRect = slot.AddComponent<RectTransform>();
                float xStart = 0.05f + i * 0.33f;
                float xEnd = xStart + 0.27f;
                slotRect.anchorMin = new Vector2(xStart, 0.1f);
                slotRect.anchorMax = new Vector2(xEnd, 0.9f);
                slotRect.offsetMin = Vector2.zero;
                slotRect.offsetMax = Vector2.zero;
                var slotImage = slot.AddComponent<Image>();
                slotImage.color = new Color(0.2f, 0.2f, 0.3f, 0.6f);
                
                // Shape icon
                var iconGo = new GameObject("ShapeIcon");
                iconGo.transform.SetParent(slot.transform, false);
                var iconRect = iconGo.AddComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0.1f, 0.1f);
                iconRect.anchorMax = new Vector2(0.9f, 0.9f);
                iconRect.offsetMin = Vector2.zero;
                iconRect.offsetMax = Vector2.zero;
                var icon = iconGo.AddComponent<Image>();
                icon.color = new Color(0.4f, 0.6f, 1f, 0.3f);
                
                slot.AddComponent<Gameplay.ShapeSlot>();
            }

            // -- Continue Panel (gizli) --
            var continuePanel = CreateUIPanel("ContinuePanel", canvasGo.transform, new Vector2(0.1f, 0.3f), new Vector2(0.9f, 0.7f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0.05f, 0.05f, 0.1f, 0.95f));
            continuePanel.SetActive(false);
            
            var continueTitle = CreateTMPText("ContinueScoreText", continuePanel.transform, "Devam etmek ister misin?", 28, TextAlignmentOptions.Center,
                new Vector2(0.1f, 0.5f), new Vector2(0.9f, 0.9f));
            
            var continueBtn = CreateButton("ContinueButton", continuePanel.transform, "Reklam İzle", 
                new Vector2(0.1f, 0.15f), new Vector2(0.48f, 0.45f), new Color(0.2f, 0.7f, 0.3f));
            var declineBtn = CreateButton("DeclineButton", continuePanel.transform, "Vazgeç", 
                new Vector2(0.52f, 0.15f), new Vector2(0.9f, 0.45f), new Color(0.7f, 0.2f, 0.2f));

            // -- Game Over Panel (gizli) --
            var gameOverPanel = CreateUIPanel("GameOverPanel", canvasGo.transform, new Vector2(0.1f, 0.25f), new Vector2(0.9f, 0.75f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0.05f, 0.05f, 0.1f, 0.95f));
            gameOverPanel.SetActive(false);
            
            var goTitle = CreateTMPText("GameOverTitle", gameOverPanel.transform, "OYUN BİTTİ", 40, TextAlignmentOptions.Center,
                new Vector2(0.1f, 0.7f), new Vector2(0.9f, 0.95f));
            var goScore = CreateTMPText("GameOverScoreText", gameOverPanel.transform, "Skor: 0", 32, TextAlignmentOptions.Center,
                new Vector2(0.1f, 0.5f), new Vector2(0.9f, 0.7f));
            var goLines = CreateTMPText("GameOverLinesText", gameOverPanel.transform, "Temizlenen Hat: 0", 24, TextAlignmentOptions.Center,
                new Vector2(0.1f, 0.35f), new Vector2(0.9f, 0.5f));
            var restartBtn = CreateButton("RestartButton", gameOverPanel.transform, "Tekrar Oyna",
                new Vector2(0.1f, 0.08f), new Vector2(0.48f, 0.3f), new Color(0.2f, 0.5f, 0.8f));
            var homeBtn = CreateButton("HomeButton", gameOverPanel.transform, "Ana Sayfa",
                new Vector2(0.52f, 0.08f), new Vector2(0.9f, 0.3f), new Color(0.5f, 0.5f, 0.5f));

            // -- Preview Container --
            var previewContainer = new GameObject("PreviewContainer");
            previewContainer.transform.SetParent(canvasGo.transform, false);
            var previewRect = previewContainer.AddComponent<RectTransform>();
            previewRect.anchorMin = Vector2.zero;
            previewRect.anchorMax = Vector2.one;
            previewRect.offsetMin = Vector2.zero;
            previewRect.offsetMax = Vector2.zero;

            // === GAME MANAGERS ===
            var managersGo = new GameObject("---Managers---");

            // GridManager
            var gridManagerGo = new GameObject("GridManager");
            gridManagerGo.transform.SetParent(managersGo.transform);
            var gridManager = gridManagerGo.AddComponent<Gameplay.GridManager>();

            // LineClearSystem
            var lineClearGo = new GameObject("LineClearSystem");
            lineClearGo.transform.SetParent(managersGo.transform);
            var lineClearSystem = lineClearGo.AddComponent<Gameplay.LineClearSystem>();

            // PlacementPreview
            var placementPreviewGo = new GameObject("PlacementPreview");
            placementPreviewGo.transform.SetParent(managersGo.transform);
            var placementPreview = placementPreviewGo.AddComponent<Gameplay.PlacementPreview>();

            // RunManager
            var runManagerGo = new GameObject("RunManager");
            runManagerGo.transform.SetParent(managersGo.transform);
            var runManager = runManagerGo.AddComponent<Gameplay.RunManager>();

            // RunUIController
            var runUIGo = new GameObject("RunUIController");
            runUIGo.transform.SetParent(managersGo.transform);
            var runUI = runUIGo.AddComponent<Gameplay.RunUIController>();

            // === Wire References (SerializedObject) ===
            // GridManager → gridContainer
            var gridManagerSO = new SerializedObject(gridManager);
            gridManagerSO.FindProperty("_gridContainer").objectReferenceValue = gridRect;
            gridManagerSO.ApplyModifiedProperties();

            // LineClearSystem → gridManager
            var lineClearSO = new SerializedObject(lineClearSystem);
            lineClearSO.FindProperty("_gridManager").objectReferenceValue = gridManager;
            lineClearSO.ApplyModifiedProperties();

            // PlacementPreview → gridManager, previewContainer
            var previewSO = new SerializedObject(placementPreview);
            previewSO.FindProperty("_gridManager").objectReferenceValue = gridManager;
            previewSO.FindProperty("_previewContainer").objectReferenceValue = previewRect;
            previewSO.ApplyModifiedProperties();

            // RunManager → gridManager, lineClearSystem, runUI
            var runManagerSO = new SerializedObject(runManager);
            runManagerSO.FindProperty("_gridManager").objectReferenceValue = gridManager;
            runManagerSO.FindProperty("_lineClearSystem").objectReferenceValue = lineClearSystem;
            runManagerSO.FindProperty("_runUI").objectReferenceValue = runUI;
            runManagerSO.ApplyModifiedProperties();

            // RunUIController → scoreText, linesText, energyText, panels, buttons, runManager
            var runUISO = new SerializedObject(runUI);
            runUISO.FindProperty("_scoreText").objectReferenceValue = scoreText.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_linesClearedText").objectReferenceValue = linesText.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_energyText").objectReferenceValue = energyText.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_continuePanel").objectReferenceValue = continuePanel;
            runUISO.FindProperty("_continueScoreText").objectReferenceValue = continueTitle.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_continueButton").objectReferenceValue = continueBtn.GetComponent<Button>();
            runUISO.FindProperty("_continueDeclineButton").objectReferenceValue = declineBtn.GetComponent<Button>();
            runUISO.FindProperty("_gameOverPanel").objectReferenceValue = gameOverPanel;
            runUISO.FindProperty("_gameOverScoreText").objectReferenceValue = goScore.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_gameOverLinesText").objectReferenceValue = goLines.GetComponent<TextMeshProUGUI>();
            runUISO.FindProperty("_restartButton").objectReferenceValue = restartBtn.GetComponent<Button>();
            runUISO.FindProperty("_homeButton").objectReferenceValue = homeBtn.GetComponent<Button>();
            runUISO.FindProperty("_runManager").objectReferenceValue = runManager;
            runUISO.ApplyModifiedProperties();

            EditorSceneManager.SaveScene(scene, path);
            Debug.Log("[SceneSetupTool] Run sahnesi kuruldu.");
        }

        private static void SetupHomeScene()
        {
            string path = "Assets/_Project/Scenes/Home.unity";
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

            foreach (var go in scene.GetRootGameObjects())
            {
                Object.DestroyImmediate(go);
            }

            // === CAMERA ===
            var camGo = new GameObject("Main Camera");
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.06f, 0.06f, 0.1f);
            cam.orthographic = true;
            cam.orthographicSize = 5;
            camGo.transform.position = new Vector3(0, 0, -10);
            camGo.tag = "MainCamera";

            // === EventSystem ===
            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            // === UI CANVAS ===
            var canvasGo = new GameObject("UICanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();

            // -- Top Bar --
            var topBar = CreateUIPanel("TopBar", canvasGo.transform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, -20), new Vector2(0, 100), new Color(0.12f, 0.12f, 0.18f, 0.9f));
            var coinsText = CreateTMPText("CoinsText", topBar.transform, "0", 36, TextAlignmentOptions.Right,
                new Vector2(0.5f, 0), new Vector2(0.95f, 1));
            CreateTMPText("TitleText", topBar.transform, "ATÖLYE", 36, TextAlignmentOptions.Left,
                new Vector2(0.05f, 0), new Vector2(0.5f, 1));

            // -- Kontrat Kartı --
            var contractPanel = CreateUIPanel("ContractPanel", canvasGo.transform, new Vector2(0.05f, 0.65f), new Vector2(0.95f, 0.9f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0.15f, 0.15f, 0.25f, 0.9f));
            var contractTitle = CreateTMPText("ContractTitleText", contractPanel.transform, "Kontrat: ...", 28, TextAlignmentOptions.Left,
                new Vector2(0.05f, 0.6f), new Vector2(0.7f, 0.95f));
            var contractDesc = CreateTMPText("ContractDescText", contractPanel.transform, "Açıklama", 18, TextAlignmentOptions.Left,
                new Vector2(0.05f, 0.35f), new Vector2(0.7f, 0.6f));
            var rewardText = CreateTMPText("RewardText", contractPanel.transform, "+100 Coin", 22, TextAlignmentOptions.Right,
                new Vector2(0.7f, 0.6f), new Vector2(0.95f, 0.95f));
            
            // Progress bar
            var progressBg = CreateUIPanel("ProgressBg", contractPanel.transform, new Vector2(0.05f, 0.05f), new Vector2(0.65f, 0.3f), new Vector2(0, 0),
                Vector2.zero, Vector2.zero, new Color(0.1f, 0.1f, 0.1f));
            var progressSlider = progressBg.AddComponent<Slider>();
            progressSlider.minValue = 0;
            progressSlider.maxValue = 1;
            progressSlider.value = 0;
            
            var progressText = CreateTMPText("ProgressText", contractPanel.transform, "0%", 20, TextAlignmentOptions.Center,
                new Vector2(0.05f, 0.05f), new Vector2(0.65f, 0.3f));

            var deliverBtn = CreateButton("DeliverButton", contractPanel.transform, "Teslim Et",
                new Vector2(0.7f, 0.05f), new Vector2(0.95f, 0.3f), new Color(0.2f, 0.7f, 0.3f));

            // Requirements container
            var reqContainer = new GameObject("RequirementsContainer");
            reqContainer.transform.SetParent(contractPanel.transform, false);
            var reqRect = reqContainer.AddComponent<RectTransform>();
            reqRect.anchorMin = new Vector2(0.05f, 0.3f);
            reqRect.anchorMax = new Vector2(0.65f, 0.6f);
            reqRect.offsetMin = Vector2.zero;
            reqRect.offsetMax = Vector2.zero;

            // -- Makineler Paneli --
            var machinesPanel = CreateUIPanel("MachinesPanel", canvasGo.transform, new Vector2(0.05f, 0.2f), new Vector2(0.95f, 0.62f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0.12f, 0.12f, 0.2f, 0.7f));
            CreateTMPText("MachinesTitle", machinesPanel.transform, "Makineler", 24, TextAlignmentOptions.Left,
                new Vector2(0.05f, 0.85f), new Vector2(0.5f, 1f));
            
            var machinesContainer = new GameObject("MachinesContainer");
            machinesContainer.transform.SetParent(machinesPanel.transform, false);
            var mcRect = machinesContainer.AddComponent<RectTransform>();
            mcRect.anchorMin = new Vector2(0.02f, 0.02f);
            mcRect.anchorMax = new Vector2(0.98f, 0.85f);
            mcRect.offsetMin = Vector2.zero;
            mcRect.offsetMax = Vector2.zero;

            // -- Play Button --
            var playBtn = CreateButton("PlayButton", canvasGo.transform, "OYNA",
                new Vector2(0.2f, 0.04f), new Vector2(0.8f, 0.16f), new Color(0.2f, 0.5f, 0.9f));
            var playTMP = playBtn.GetComponentInChildren<TextMeshProUGUI>();
            if (playTMP != null) playTMP.fontSize = 40;

            // === HOME UI CONTROLLER ===
            var homeUIGo = new GameObject("HomeUIController");
            var homeUI = homeUIGo.AddComponent<UI.HomeUIController>();

            // Wire references
            var homeUISO = new SerializedObject(homeUI);
            homeUISO.FindProperty("_coinsText").objectReferenceValue = coinsText.GetComponent<TextMeshProUGUI>();
            homeUISO.FindProperty("_contractPanel").objectReferenceValue = contractPanel;
            homeUISO.FindProperty("_contractTitleText").objectReferenceValue = contractTitle.GetComponent<TextMeshProUGUI>();
            homeUISO.FindProperty("_contractDescText").objectReferenceValue = contractDesc.GetComponent<TextMeshProUGUI>();
            homeUISO.FindProperty("_requirementsContainer").objectReferenceValue = reqContainer.transform;
            homeUISO.FindProperty("_progressSlider").objectReferenceValue = progressSlider;
            homeUISO.FindProperty("_progressText").objectReferenceValue = progressText.GetComponent<TextMeshProUGUI>();
            homeUISO.FindProperty("_rewardText").objectReferenceValue = rewardText.GetComponent<TextMeshProUGUI>();
            homeUISO.FindProperty("_deliverButton").objectReferenceValue = deliverBtn.GetComponent<Button>();
            homeUISO.FindProperty("_machinesContainer").objectReferenceValue = machinesContainer.transform;
            homeUISO.FindProperty("_playButton").objectReferenceValue = playBtn.GetComponent<Button>();
            homeUISO.ApplyModifiedProperties();

            EditorSceneManager.SaveScene(scene, path);
            Debug.Log("[SceneSetupTool] Home sahnesi kuruldu.");
        }

        private static void SetupBuildSettings()
        {
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/_Project/Scenes/Boot.unity", true),
                new EditorBuildSettingsScene("Assets/_Project/Scenes/Home.unity", true),
                new EditorBuildSettingsScene("Assets/_Project/Scenes/Run.unity", true)
            };
            Debug.Log("[SceneSetupTool] Build Settings güncellendi (Boot=0, Home=1, Run=2).");
        }

        // ===== HELPER METHODS =====

        private static GameObject CreateUIPanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 offsetMin, Vector2 sizeDelta, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.offsetMin = offsetMin;
            rect.sizeDelta = sizeDelta;
            // Stretch mode ise offset kullan
            if (anchorMin != anchorMax)
            {
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
            var img = go.AddComponent<Image>();
            img.color = color;
            return go;
        }

        private static GameObject CreateTMPText(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment,
            Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = new Vector2(10, 5);
            rect.offsetMax = new Vector2(-10, -5);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.alignment = alignment;
            tmp.color = Color.white;
            return go;
        }

        private static GameObject CreateButton(string name, Transform parent, string label,
            Vector2 anchorMin, Vector2 anchorMax, Color bgColor)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            img.color = bgColor;
            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(go.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 24;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;

            return go;
        }
    }
}
