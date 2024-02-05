using System;
using System.IO;
using DPUtils.Systems.DateTime;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;


namespace SaveGameSettings
{
    [Serializable]
    public class SaveData
    {
        public CurrentDateTimeInfo CurrentDateTimeInfo;
        public int PlayerBankScore;
        public int TotalCustomers;

        public SaveData(CurrentDateTimeInfo currentDateTimeInfo, int playerBankScore, int totalCustomers)
        {
            CurrentDateTimeInfo = currentDateTimeInfo;
            PlayerBankScore = playerBankScore;
            TotalCustomers = totalCustomers;
        }

        public void SaveToFile(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Create);

            formatter.Serialize(stream, this);

            stream.Close();
        }

        public static SaveData LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);

                SaveData saveData = formatter.Deserialize(stream) as SaveData;

                stream.Close();

                return saveData;
            }
            else
            {
                Debug.LogError("Save file not found: " + filePath);
                return null;
            }
        }

    }

    public class SaveGameScript : MonoBehaviour
    {
        public TimeManagerScript timeManager;
        public BankScoreController bankScoreController;
        public StatsTracker statsTracker;


        public void SaveGame()
        {
            if (bankScoreController == null || statsTracker == null)
            {
                Debug.LogError("One or more references in SaveGameScript are null.");
                return;
            }
            string saveFilePath = "Assets/SaveFiles/saveData.dat"; // Use .dat or another binary extension

            CurrentDateTimeInfo currentDateTimeInfo = timeManager.CurrentDateTimeInfo;
            int playerBankScore = bankScoreController.GetBankScore();
            int totalCustomers = statsTracker.GetTotalSpawnedNPCs();

            SaveData saveData = new SaveData(currentDateTimeInfo, playerBankScore, totalCustomers);
            
            // Save using binary serialization
            saveData.SaveToFile(saveFilePath);

            Debug.Log("Game saved!  " + saveData.CurrentDateTimeInfo.Hour);
        }
    }
}
