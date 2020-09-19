using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace MVC.core
{
    public class EventManager
    {
        public class Event
        {
            public string type;
            public object target;
            public object value;
        }
        private EventManager() { }

        public delegate void eventHandler(Event e);
        static Dictionary<string, eventHandler> events = new Dictionary<string, eventHandler>();

        public static void AddEventListen(string eventName, eventHandler callback)
        {
            if (!events.ContainsKey(eventName))
            {
                events.Add(eventName, callback);
            }
            else
            {
                events[eventName] += callback;
            }
        }
        public static void RemoveEventListen(string eventName, eventHandler callback)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] -= callback;

                if (events[eventName] == null)
                {
                    events.Remove(eventName);
                }
            }
        }
        public static void DispatchEvent(string eventName, Event e)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName](e);
            }
        }
    }
    public class AudioManager
    {

        //最大音效数量
        static int max_effect_num;

        static bool music_mute;
        static bool effect_mute;

        static float music_volume;
        static float effect_volume;

        static AudioSource music_as;
        static AudioSource[] effect_as;

        static int curr_effect_as;

        static bool isInit = false;

        public static void init(GameObject parent)
        {
            init(parent, 8);
        }
        public static void init(GameObject parent, int _max_effect_num)
        {
            if (isInit == true)
            {
                return;
            }
            //init attr        
            max_effect_num = _max_effect_num;
            music_mute = false;
            effect_mute = false;

            music_volume = 1;
            effect_volume = 1;

            curr_effect_as = 0;

            //create music AudioSource
            music_as = parent.AddComponent<AudioSource>();
            //create effect AudioSource
            effect_as = new AudioSource[max_effect_num];
            for (int i = 0; i < max_effect_num; i++)
            {
                effect_as[i] = parent.AddComponent<AudioSource>();
                effect_as[i].mute = effect_mute;
                effect_as[i].volume = effect_volume;
            }

            //记录是否初始化过了
            isInit = true;
        }

        public static float musicVolume
        {
            set
            {
                music_volume = value;
                music_as.volume = value;
            }
            get
            {
                return music_volume;
            }
        }
        public static float effectVolume
        {
            set
            {
                effect_volume = value;
                foreach (AudioSource item in effect_as)
                {
                    item.volume = value;
                }
            }
            get
            {
                return effect_volume;
            }
        }
        public static bool effectMute
        {
            set
            {
                effect_mute = value;
                foreach (AudioSource item in effect_as)
                {
                    item.mute = value;
                }
            }
            get
            {
                return effect_mute;
            }
        }
        public static bool musicMute
        {
            set
            {
                music_mute = value;
                music_as.mute = value;
            }
            get
            {
                return effect_mute;
            }
        }
        //开始/继续/重新 播放bgm
        public static void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            music_as.clip = clip;
            music_as.Play();
        }
        //暂停bgm
        public static void PauseMusic()
        {
            music_as.Pause();
        }
        //停止播放
        public static void StopMusic()
        {
            music_as.Stop();
        }
        //播放音效
        public static void PlayEffect(AudioClip clip)
        {
            if (clip == null) return;

            AudioSource audio = effect_as[curr_effect_as];
            audio.clip = clip;
            audio.Play();
            curr_effect_as++;
            if (curr_effect_as >= max_effect_num)
            {
                curr_effect_as = 0;
            }
        }
    }
    public class FileManager
    {

        public static JObject LoadJsonForPorject(string filePath)
        {
            return LoadJson(Application.dataPath + "/" + filePath);
        }
        public static void SaveJsonForPorject(string filePath, JObject job)
        {
            SaveJson(Application.dataPath + "/" + filePath, job);
        }


        public static JObject LoadJsonForGame(string filePath)
        {
            return LoadJson(Application.persistentDataPath + "/" + filePath);
        }
        public static void SaveJsonForGame(string filePath, JObject job)
        {
            SaveJson(Application.persistentDataPath + "/" + filePath, job);
        }


        /// <summary>
        /// 加载绝对路径的json文件
        /// </summary>
        public static JObject LoadJson(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string text = File.ReadAllText(filePath);
            JObject obj = JObject.Parse(text);
            return obj;
        }
        public static void SaveJson(string filePath, JObject job)
        {
            if (Path.IsPathRooted(filePath))
            {
                string json = JsonConvert.SerializeObject(job);
                File.WriteAllText(filePath, job.ToString(), Encoding.UTF8); //utf8 万国码避免乱码
            }
        }
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        
    }
}