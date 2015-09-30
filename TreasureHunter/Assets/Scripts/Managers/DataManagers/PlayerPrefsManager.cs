using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Treasure_Hunter.Models;
using System;

namespace Treasure_Hunter.Managers
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        public static PlayerPrefsManager Instance { get; private set; }

        #region CLASS SETTINGS

        //encryption consts
        private byte[] RIJNDAEL_KEY { get { return new byte[] { 52, 240, 155, 214, 101, 164, 77, 108, 56, 31, 47, 182, 38, 227, 158, 32, 85, 250, 146, 153, 193, 73, 240, 179, 248, 209, 212, 255, 87, 35, 129, 202 }; } }
        private byte[] RIJNDAEL_IV  { get { return new byte[] { 196, 216, 227, 186, 238, 234, 238, 165, 131, 52, 33, 103, 172, 171, 158, 105 }; } }
        //key names
        private const string ACHIEVEMENTS_KEY = "achievements";

        #endregion

        #region CONVERSION DURING ENCRYPTION

        private string ArrayToString(byte[] array)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append(((int)array[i]).ToString() + ",");
            }
            return builder.ToString();
        }

        private byte[] StringToArray(string array)
        {
            string[] bytes = array.Split(',');
            byte[] returnedArray = new byte[bytes.Length - 1];
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                returnedArray[i] = (byte)Convert.ToInt32(bytes[i]);
            }
            return returnedArray;
        }

        #endregion

        #region ACHIEVEMENTS

        public AchievementsData Achievements;

        public void LoadAchievements()
        {
            if (PlayerPrefs.HasKey(ACHIEVEMENTS_KEY))
            {
                byte[] cipherText = StringToArray(PlayerPrefs.GetString(ACHIEVEMENTS_KEY));
                string plaintext = null;
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = RIJNDAEL_KEY;
                    rijAlg.IV = RIJNDAEL_IV;
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                Achievements.AsignDataToCorrectDictionaries(plaintext);
            }
        }

        public void SaveAchievements()
        {
            string textToSave = Achievements.GetStringToSaveDictionaries();
            if (textToSave!="")
            {
                byte[] encrypted;
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = RIJNDAEL_KEY;
                    rijAlg.IV = RIJNDAEL_IV;
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(textToSave);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                PlayerPrefs.SetString(ACHIEVEMENTS_KEY, ArrayToString(encrypted));
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region TREASURES

        #endregion

        #region ACTIONS

        public ActionsData Actions;

        public void LoadActions()
        {

        }

        public void SaveActions()
        {
            
        }

        #endregion

        #region MONO BEHAVIOUR

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        public void Init()
        {
            Achievements = new AchievementsData();
            LoadAchievements();
            Actions = new ActionsData();
            LoadActions();
        }
    }
}
