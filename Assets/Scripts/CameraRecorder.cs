using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public class CameraRecorder : MonoBehaviour
{
    private RecorderController recorderController;

    void Update()
    {
        // Press 'R' to start recording
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }

        // Press 'S' to stop recording
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }
    }

    void StartRecording()
    {
        if (recorderController != null && recorderController.IsRecording())
        {
            Debug.LogWarning("Already recording!");
            return;
        }


        // Ensure the Recordings folder exists
        string recordingsPath = Path.Combine(Application.dataPath, "Recordings");
        if (!Directory.Exists(recordingsPath))
        {
            Directory.CreateDirectory(recordingsPath);
        }

        Debug.Log("Starting recording..." + recordingsPath);
        
        // Create a new recorder controller
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);

        // Create Movie Recorder settings
        var movieRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        movieRecorder.name = "Camera Recorder";
        movieRecorder.Enabled = true;

        // Configure video settings
        movieRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        movieRecorder.VideoBitRateMode = VideoBitrateMode.High;
        movieRecorder.OutputFile = Path.Combine(recordingsPath, $"Video_{System.DateTime.Now:yyyyMMdd_HHmmss}.mp4");

        // Set camera input (Main Camera)
        var inputSettings = new CameraInputSettings
        {
            Source = ImageSource.MainCamera,
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        movieRecorder.ImageInputSettings = inputSettings;

        // Add recorder to the controller
        controllerSettings.AddRecorderSettings(movieRecorder);
        controllerSettings.SetRecordModeToManual();

        // Start recording
        recorderController.PrepareRecording();
        recorderController.StartRecording();
    }

    void StopRecording()
    {
        if (recorderController == null || !recorderController.IsRecording())
        {
            Debug.LogWarning("No recording in progress!");
            return;
        }

        recorderController.StopRecording();
        Debug.Log("Recording stopped.");
    }
}