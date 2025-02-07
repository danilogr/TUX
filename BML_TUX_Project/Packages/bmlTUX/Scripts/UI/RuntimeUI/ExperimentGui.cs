﻿using System.Data;
using bmlTUX.Scripts.ExperimentParts;
using bmlTUX.Scripts.Managers;
using bmlTUX.Scripts.UI.RuntimeUI.RunnerWindowUI;
using bmlTUX.Scripts.UI.RuntimeUI.SessionSetupWindowUI;
using bmlTUX.Scripts.UI.RuntimeUI.UIUtilities;
using bmlTUX.Scripts.Utilities;
using bmlTUX.Scripts.Utilities.Extensions;
using bmlTUX.Scripts.VariableSystem;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace bmlTUX.Scripts.UI.RuntimeUI {
    public class ExperimentGui : MonoBehaviour {
        
        ExperimentRunner runner;

        [SerializeField]
        SessionSetupPanel SessionSetupPanel = default;
        
        [SerializeField]
        TableViewer.TableViewer TableDisplay = default;

        [SerializeField]
        ExperimentRunnerPanel ExperimentRunnerPanel = default;

        [SerializeField]
        RectTransform ExperimentStartPanel = default;

        [SerializeField] TextMeshProUGUI previewText = default;
        
        DesignPreviewer previewer;
        FileLocationSettings fileLocationSettings;
        public Canvas Canvas;
        public Camera placeholderCamera;
        public void RegisterExperiment(ExperimentRunner experimentRunner) {
            ExperimentEvents.OnInitExperiment += Init;
            ExperimentEvents.OnExperimentStarted += ExperimentHasStarted;
            runner = experimentRunner;
            fileLocationSettings = runner.DesignFile.GetFileLocationSettings;
            
            InitGui(experimentRunner);
        }

        void InitGui(ExperimentRunner experimentRunner) {

            gameObject.SetActive(true);
            int targetDisplay = experimentRunner.DesignFile.GetGuiSettings.TargetDisplay;

            if (Display.displays.Length > targetDisplay || Application.isEditor) {
                if (!Application.isEditor) {
                    Display.displays[targetDisplay].Activate();
                }

                Debug.Log($"{TuxLog.Prefix} Setting UI to show on Display {targetDisplay + 1}. Click here to highlight current settings file in project.", experimentRunner.DesignFile.GetGuiSettings);
                if (targetDisplay > 0 && experimentRunner.DesignFile.GetGuiSettings.WarnUserIfNotDisplayOne) 
                    Debug.LogWarning(TuxLog.Warn("UI is on secondary display. If you can't see UI, adjust settings. You can turn this warning off (click on this message)."),  experimentRunner.DesignFile.GetGuiSettings);
            }
            else {
                Debug.LogWarning($"{TuxLog.Prefix} Not enough displays plugged in to accommodate your UI settings. Reverting UI to display on {Display.displays.Length}");
                targetDisplay = Display.displays.Length;
            }

            Canvas.targetDisplay = targetDisplay;
            if (targetDisplay != 0) {
                placeholderCamera.targetDisplay = targetDisplay;
            }
        }

        void ExperimentHasStarted() {
            if (!runner.DesignFile.GetGuiSettings.ShowRunnerInterfaceAfterStart) {
                this.gameObject.SetActive(false);
            }
        }

        public void OnDisable() {
            ExperimentEvents.OnInitExperiment -= Init;
            ExperimentEvents.OnExperimentStarted -= ExperimentHasStarted;
        }

        void Init(ExperimentRunner unused) {
            
            SessionSetupPanel.Init(runner);

            previewer = new DesignPreviewer(runner.DesignFile);
            DisplayPreview();
        }

        void DisplayPreview() {
            DataTable preview = previewer.GetPreview(SessionSetupPanel.SelectedBlockOrder);
            string tableString = preview.AsString(truncateLength:10, paddingLength:10, separator:" ");
            //Debug.Log($"{TuxLog.Prefix} Click here to see trial table. \n{tableString}");
            previewText.text = tableString;
            TableDisplay.Display(preview);
        }

        [PublicAPI]
        public void ReRandomizePreview() {
            previewer.ReRandomizeTable();
            DisplayPreview();
        }
        

        [PublicAPI]
        public void StartExperimentFromButton() {
            Session session = SessionSetupPanel.GetSession();
            if (SessionSetupPanel.ValidSession) {
                StartRunningExperiment(session);
            }
        }

        void StartRunningExperiment(Session session) {
            ExperimentEvents.StartRunningExperiment(session);
            ExperimentStartPanel.gameObject.SetActive(false);
            ExperimentRunnerPanel.ShowPanel();
        }

        [PublicAPI]
        public void StartDebugExperimentFromButton() {
            if (fileLocationSettings == null) Debug.LogError($"{TuxLog.Prefix} fileLocationSettings null when debug started");
            Session session = new DebugSession(fileLocationSettings);

            foreach (ParticipantVariable variable in runner.DesignFile.GetVariables.ParticipantVariables) {
                variable.SetValueDefaultValue();
            }
            
            StartRunningExperiment(session);
        }

    }
}
