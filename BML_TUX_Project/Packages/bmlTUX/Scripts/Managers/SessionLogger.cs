﻿using System;
using System.IO;
using System.Text;
using bmlTUX.Scripts.ExperimentParts;
using UnityEngine;

namespace bmlTUX.Scripts.Managers {
    public static class SessionLogger {

        /// <summary>
        /// Logs a Session to file
        /// </summary>
        /// <param name="session"></param>
        public static void Log(Session session) {

            Directory.CreateDirectory(session.FileLocations.SessionFolder);
            string logString = CreateLogString(session);

            using (StreamWriter streamWriter = new StreamWriter(session.FileLocations.SessionLogFilePath, true)) {
                streamWriter.Write(logString);
            }
        }

        static string CreateLogString(Session session) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("---------------------");
            sb.AppendLine($"Session Logged: {DateTime.Now:yyyy/MM/dd hh:mm}");
            sb.AppendLine(JsonUtility.ToJson(session));
            return sb.ToString();
        }

        /// <summary>
        /// Adds a line to log to mark Session as complete
        /// </summary>
        /// <param name="session"></param>
        public static void LogComplete(Session session) {

            //ensure log exists
            if (File.Exists(session.FileLocations.SessionLogFilePath)) {
                using (StreamWriter streamWriter = new StreamWriter(session.FileLocations.SessionLogFilePath, true)) {
                    streamWriter.Write($"Confirmed Complete at: {DateTime.Now:hh:mm}");
                }
            }
            else {
                throw new FileNotFoundException("Session log file not found");
            }
        }
    }
}