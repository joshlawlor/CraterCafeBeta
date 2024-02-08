using UnityEngine.SceneManagement;
using System;
using System.IO;
using DPUtils.Systems.DateTime;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;


namespace DPUtils.Systems.SaveSystem
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

        public override string ToString()
        {
            return $"CurrentDateTimeInfo: Hour - {CurrentDateTimeInfo.Hour}, IsAM - {CurrentDateTimeInfo.IsAM}, Minutes - {CurrentDateTimeInfo.Minutes}, Year - {CurrentDateTimeInfo.Year}, Season - {CurrentDateTimeInfo.Season}, Total Weeks - {CurrentDateTimeInfo.TotalNumWeeks}, Total Days - {CurrentDateTimeInfo.TotalNumDays}\nPlayerBankScore: {PlayerBankScore}\nTotalCustomers: {TotalCustomers}";
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
        string saveFilePath = Application.persistentDataPath + "/savedGameFile.dat";

            CurrentDateTimeInfo currentDateTimeInfo = timeManager.CurrentDateTimeInfo;
            int playerBankScore = bankScoreController.GetBankScore();
            int totalCustomers = statsTracker.GetTotalSpawnedNPCs();

            SaveData saveData = new SaveData(currentDateTimeInfo, playerBankScore, totalCustomers);

            // Save using binary serialization
            saveData.SaveToFile(saveFilePath);

            Debug.Log($"Game saved!\n{saveData}");
        }

        public void LoadGame()
        {
            string loadFilePath = Application.persistentDataPath + "/savedGameFile.dat";

            SaveData loadedData = SaveData.LoadFromFile(loadFilePath);

            if (loadedData != null)
            {
                // Need to use the loaded data here. For example:
                timeManager.SetDateTime(loadedData.CurrentDateTimeInfo);
                bankScoreController.SetBankScore(loadedData.PlayerBankScore);
                statsTracker.SetTotalSpawnedNPCs(loadedData.TotalCustomers);


                Debug.Log($"Game loaded!\n{loadedData}");
            }
        }

        public void ContinueGame()
        {
            string loadFilePath = Application.persistentDataPath + "/savedGameFile.dat";

            SaveData loadedData = SaveData.LoadFromFile(loadFilePath);

            if (loadedData != null)
            {
                TransitionToNextScene();
                // Use the loaded data
                timeManager.SetDateTime(loadedData.CurrentDateTimeInfo);
                bankScoreController.SetBankScore(loadedData.PlayerBankScore);
                statsTracker.SetTotalSpawnedNPCs(loadedData.TotalCustomers);

                Debug.Log($"Game loaded!\n{loadedData}");

                // Transition to the next scene
                
            }
        }
        private void TransitionToNextScene()
        {
            // Load the next scene by its build index
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }
}
